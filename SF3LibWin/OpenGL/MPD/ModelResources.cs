using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.SGL;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Types;
using SF3.Win.Extensions;
using SF3.Imaging;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;
using SF3.MPD.Extensions;

namespace SF3.Win.OpenGL.MPD {
    public class ModelResources : ResourcesBase, IMPD_Resources {
        public ModelResources(bool applyShadowTags, bool applyHideTags) : base() {
            ApplyShadowTags = applyShadowTags;
            ApplyHideTags   = applyHideTags;
        }

        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            foreach (var modelsDict in ModelsByIDByCollection.Values)
                foreach (var model in modelsDict.Values)
                    model.Dispose();

            ModelsByIDByCollection.Clear();
            MPD_ModelsByIDByCollection.Clear();

            ModelInstances = null;
        }

        private void InitDictsForType(MPD_CollectionType collection) {
            // TODO: Just have one structure with all this info!
            if (!ModelsByIDByCollection.ContainsKey(collection))
                ModelsByIDByCollection[collection] = [];
            if (!MPD_ModelsByIDByCollection.ContainsKey(collection))
                MPD_ModelsByIDByCollection[collection] = [];
        }

        private Dictionary<int, IMPD_AnimatableTexture> GetTextureDictionaryByCollection(IMPD_ModelCollection modelCollection, IMPD mpdFile) {
            var hasIgnored = !mpdFile.Settings.AreIgnoredTexturesDummiedOut;
            return modelCollection.Textures
                .Where(x => !hasIgnored || !x.IsIgnored)
                .ToDictionary(x => x.ID, x => x);
        }

        public void Update(IMPD mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var modelInstanceList = new List<IMPD_ModelInstance>();
            foreach (var mc in mpdFile.ModelCollections.Values) {
                if (mc.Collection.IsHeaderModelCollection())
                    continue;

                // Get all instances of models in this collection.
                var instances = mc.ModelInstances;
                modelInstanceList.AddRange(instances);

                // There is a function that scans for models with the tag '2000' and forcibly changes all the textures
                // in their PDATAs to be semi-transparent. Yes, this is redundant to have on the *model* instead of the *PDATA*,
                // so who knows why it works this way.
                var modelsWith2000Tag = ApplyShadowTags ? instances
                    .Where(x => x.Tag >= 2000 && x.Tag < 2100)
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToHashSet()
                    : [];

                // There are some (usually) bright-red models in Scenario 3 that are removed when
                // the 3000 tag is present. They are used to crop out models so the ground texture (VDP2) is visible instead.
                var modelsWith3000Tag = ApplyHideTags ? instances
                    .Where(x => x.Tag == 3000)
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToHashSet()
                    : [];

                InitDictsForType(mc.Collection);
                var mpdModelsByID = MPD_ModelsByIDByCollection[mc.Collection];

                var uniqueModelIDs = instances
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToArray();

                var texturesById = GetTextureDictionaryByCollection(mc, mpdFile);

                foreach (var id in uniqueModelIDs) {
                    if (id == -1)
                        continue;

                    var mpdModel = mpdModelsByID.TryGetValue(id, out var mpdModelOut) ? mpdModelOut : null;
                    if (mpdModel == null)
                        mpdModelsByID[id] = mpdModel = mc.GetModel(id, 0);
                    if (mpdModel == null)
                        continue;

                    // Don't render movable models; they're not placed on the map in that way.
                    if (!mc.IsHeaderModelCollection()) {
                        var isForcedSemiTransparent = modelsWith2000Tag.Contains(id);
                        var isHideMesh = modelsWith3000Tag.Contains(id);
                        CreateAndAddQuadModels(mpdFile, mc.Collection, mpdModel, texturesById, isForcedSemiTransparent, isHideMesh);
                    }
                }
            }

            ModelInstances = modelInstanceList.ToArray();
        }

        public void Update(IMPD mpdFile, IMPD_ModelCollection models, IMPD_ModelLoD mpdModel,
            bool forceSemiTransparent = false, bool isHideMesh = false,
            float rotX = 0f, float rotY = 0f, float rotZ = 0f,
            float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f
        ) {
            Reset();
            if (models == null || mpdModel == null)
                return;

            InitDictsForType(models.Collection);
            MPD_ModelsByIDByCollection[models.Collection][mpdModel.ModelID] = mpdModel;

            var texturesById = GetTextureDictionaryByCollection(models, mpdFile);
            CreateAndAddQuadModels(mpdFile, models.Collection, mpdModel, texturesById, forceSemiTransparent, isHideMesh);

            var modelInstance = new MPD_ModelInstance() {
                Collection = models,
                ID = 0,
                ModelID = mpdModel.ModelID,
                PositionX = 32 * 32,
                PositionZ = 32 * 32,
                AngleX = rotX,
                AngleY = rotY,
                AngleZ = rotZ,
                ScaleX = scaleX,
                ScaleY = scaleY,
                ScaleZ = scaleZ,
            };

            ModelInstances = [modelInstance];
        }

        private void CreateAndAddQuadModels(
            IMPD mpdFile,
            MPD_CollectionType modelCollection,
            IMPD_ModelLoD mpdModel,
            Dictionary<int, IMPD_AnimatableTexture> texturesById,
            bool forceSemiTransparent,
            bool isHideMesh
        ) {
            TextureFlipType ToggleHorizontalFlipping(TextureFlipType flip)
                => flip & ~TextureFlipType.Horizontal | (TextureFlipType) (TextureFlipType.Horizontal - (flip & TextureFlipType.Horizontal));

            var vertices = mpdModel.Vertices;
            var faces = mpdModel.Faces;

            var modelExists = false;

            var hideQuads                      = new List<Quad>();
            var solidTexturedQuads             = new List<Quad>();
            var solidUntexturedQuads           = new List<Quad>();
            var semiTransparentTexturedQuads   = new List<Quad>();
            var semiTransparentUntexturedQuads = new List<Quad>();

            for (var i = 0; i < faces.Count; i++) {
                var polygon = faces[i];
                var attr = polygon.Attributes;

                var color = new Vector4(1);
                var useTexture = attr.UseTexture;
                IMPD_Animation anim = null;
                var isSemiTransparent = false;
                var flip = TextureFlipType.NoFlip;

                if (!isHideMesh) {
                    // Get texture. Fetch animated textures if possible.
                    var textureId = attr.TextureNo;

                    // Get texture flipping. Manually flip them horizontally to account for the weird thing where the X coordinates are reversed.
                    flip = (TextureFlipType) (attr.Dir & 0x0030);
                    flip = ToggleHorizontalFlipping(flip);

                    // Apply semi-transparency for the appropriate draw mode.
                    var transparency = 1.0f;

                    var drawMode = (DrawMode) ((int) attr.Mode_DrawMode & 0x03);

                    if (drawMode == DrawMode.CL_Trans || drawMode == DrawMode.CL_Shadow)
                        transparency *= 0.5f;
                    else if (forceSemiTransparent)
                        transparency *= 1.0f - (mpdFile.BinaryReproductionFlags.PaletteAdjustmentIsTruncated ? 0x0F : mpdFile.Settings.ShadowTransparency / (float) 0x1F);

                    if (!useTexture) {
                        var colorChannels = PixelConversion.ABGR1555toChannels(attr.ColorNo);
                        color = new Vector4(colorChannels.R / 255.0f, colorChannels.G / 255.0f, colorChannels.B / 255.0f, 1.0f);
                    }
                    else {
                        if (textureId != 0xFF && texturesById.ContainsKey(textureId))
                            if (texturesById.TryGetValue(textureId, out var tex))
                                anim = tex.Animation ?? new MPD_MockAnimation(tex);

                        // If the texture is missing, mark this polygon bright red.
                        if (anim == null) {
                            useTexture = false;
                            color = new Vector4(1f, 0f, 0f, 1f);
                        }
                    }

                    // If forcing semi-transparency, and there aren't any already-indexed textures, force color to black.
                    // (This isn't how this actually works, but this is fine for display.)
                    if (forceSemiTransparent && (anim == null || anim.Frames.All(x => x.BytesPerPixel == 2)))
                        color[0] = color[1] = color[2] = 0.0f;

                    color[3] *= transparency;
                    isSemiTransparent = color[3] < 0.99f;
                    if (!isSemiTransparent && color[3] < 1.00f)
                        color[3] = 1.00f;

                    // "Half" draw mode darkens textures by 50%.
                    if (drawMode == DrawMode.CL_Half) {
                        color[0] *= 0.5f;
                        color[1] *= 0.5f;
                        color[2] *= 0.5f;
                    }
                }

                VECTOR[] polyVertexModels = [
                    vertices[polygon.VertexIndices[0]],
                    vertices[polygon.VertexIndices[1]],
                    vertices[polygon.VertexIndices[2]],
                    vertices[polygon.VertexIndices[3]],
                ];

                var polyVertices = polyVertexModels
                    .Select(x => new Vector3(-x.X.Float, -x.Y.Float, x.Z.Float) * new Vector3(1 / 32.0f))
                    .ToArray();

                var normal = new Vector3(-polygon.Normal.X.Float, -polygon.Normal.Y.Float, polygon.Normal.Z.Float);
                var vertexNormals = new Vector3[] { normal, normal, normal, normal };
                var normalVboData = vertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                var useGouraud = attr.CL_Gouraud && useTexture;
                var applyLighting = (attr.UseLight || anim == null) && !useGouraud ? 1.0f : 0.0f;
                var applyLightingVboData = new float[,] {{applyLighting}, {applyLighting}, {applyLighting}, {applyLighting}};

                var mesh = attr.Mode_MESHon ? 1.00f : 0.00f;
                var meshVboData = new float[,] {{mesh}, {mesh}, {mesh}, {mesh}};

                void AddQuad() {
                    var newQuad = new Quad(polyVertices, anim, TextureRotateType.NoRotation, flip, color);

                    if (isHideMesh)
                        hideQuads.Add(newQuad);
                    else {
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, applyLightingVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "mesh", 4, meshVboData));

                        if (isSemiTransparent) {
                            if (anim != null)
                                semiTransparentTexturedQuads.Add(newQuad);
                            else
                                semiTransparentUntexturedQuads.Add(newQuad);
                        }
                        else {
                            if (anim != null)
                                solidTexturedQuads.Add(newQuad);
                            else
                                solidUntexturedQuads.Add(newQuad);
                        }
                    }
                }

                // Add quads first...
                AddQuad();

                // ...then add the other side, if it's there.
                if (attr.IsTwoSided) {
                    // Flip the coordinates in the polygon horizontally.
                    (polyVertices[0], polyVertices[1], polyVertices[2], polyVertices[3]) =
                        (polyVertices[1], polyVertices[0], polyVertices[3], polyVertices[2]);

                    // Flip the texture.
                    flip = ToggleHorizontalFlipping(flip);

                    // Reverse the normal.
                    normal = -normal;
                    vertexNormals = [normal, normal, normal, normal];
                    normalVboData = vertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                    // Add the flipped quad.
                    AddQuad();
                }

                modelExists = true;
            }

            var hideModel                      = hideQuads.Count > 0                      ? new QuadModel(hideQuads.ToArray())                   : null;
            var solidTexturedModel             = solidTexturedQuads.Count > 0             ? new QuadModel(solidTexturedQuads.ToArray())             : null;
            var solidUntexturedModel           = solidUntexturedQuads.Count > 0           ? new QuadModel(solidUntexturedQuads.ToArray())           : null;
            var semiTransparentTexturedModel   = semiTransparentTexturedQuads.Count > 0   ? new QuadModel(semiTransparentTexturedQuads.ToArray())   : null;
            var semiTransparentUntexturedModel = semiTransparentUntexturedQuads.Count > 0 ? new QuadModel(semiTransparentUntexturedQuads.ToArray()) : null;

            if (modelExists) {
                ModelsByIDByCollection[modelCollection][mpdModel.ModelID] = new ModelGroup(
                    solidTexturedModel,
                    solidUntexturedModel,
                    semiTransparentTexturedModel,
                    semiTransparentUntexturedModel,
                    hideModel
                );
            }
        }

        public Dictionary<MPD_CollectionType, Dictionary<int, ModelGroup>> ModelsByIDByCollection { get; } = [];
        public Dictionary<MPD_CollectionType, Dictionary<int, IMPD_ModelLoD>> MPD_ModelsByIDByCollection { get; } = [];
        public IMPD_ModelInstance[] ModelInstances { get; private set; }

        public bool ApplyShadowTags { get; set; } = false;
        public bool ApplyHideTags { get; set; } = false;
    }
}
