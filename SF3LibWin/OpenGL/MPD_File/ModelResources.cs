using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.SGL;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;
using SF3.Win.Extensions;
using SF3.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
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
            SGL_ModelsByIDByCollection.Clear();

            ModelInstances = null;
        }

        private void InitDictsForType(ModelCollectionType collection) {
            // TODO: Just have one structure with all this info!
            if (!ModelsByIDByCollection.ContainsKey(collection))
                ModelsByIDByCollection[collection] = [];
            if (!SGL_ModelsByIDByCollection.ContainsKey(collection))
                SGL_ModelsByIDByCollection[collection] = [];
        }

        private TextureCollectionType GetTextureCollection(ModelCollectionType modelCollection) {
            switch (modelCollection) {
                case ModelCollectionType.PrimaryModels:
                    return TextureCollectionType.PrimaryTextures;
                case ModelCollectionType.Chunk19Model:
                    return TextureCollectionType.Chunk19ModelTextures;
                case ModelCollectionType.Chunk1Model:
                    return TextureCollectionType.Chunk1ModelTextures;
                case ModelCollectionType.MovableModels1:
                    return TextureCollectionType.MovableModels1;
                case ModelCollectionType.MovableModels2:
                    return TextureCollectionType.MovableModels2;
                case ModelCollectionType.MovableModels3:
                    return TextureCollectionType.MovableModels3;
                default:
                    throw new ArgumentException(nameof(modelCollection));
            }
        }

        public struct ModelAnimationInfo {
            public ITexture[] Textures;
            public int FrameTimerStart;
        }

        private Dictionary<int, ITexture> GetTextureDictionaryByCollection(IMPD_File mpdFile, TextureCollectionType texCollection) {
            return mpdFile.TextureCollections != null ? mpdFile.TextureCollections
                .Where(x => x?.TextureTable != null && x.TextureTable.Collection == texCollection)
                .SelectMany(x => x.TextureTable)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];
        }

        private Dictionary<int, ModelAnimationInfo> GetAnimationDictionaryByCollection(IMPD_File mpdFile, TextureCollectionType texCollection) {
            if (texCollection != TextureCollectionType.PrimaryTextures || mpdFile.TextureAnimations == null)
                return [];

            return mpdFile.TextureAnimations
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => new ModelAnimationInfo {
                    Textures = x.FrameTable.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray(),
                    FrameTimerStart = x.FrameTimerStart
                });
        }

        public void Update(IMPD_File mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var modelInstanceList = new List<IMPD_ModelInstance>();
            foreach (IMPD_ModelCollection mc in mpdFile.ModelCollections) {
                if (mc.Collection != ModelCollectionType.Chunk19Model &&
                    mc.Collection != ModelCollectionType.PrimaryModels &&
                    mc.Collection != ModelCollectionType.Chunk1Model)
                {
                    continue;
                }

                // Get all instances of models in this collection.
                var instances = mc.GetModelInstances();
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
                var sglModelsByID = SGL_ModelsByIDByCollection[mc.Collection];

                var uniqueModelIDs = instances
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToArray();

                var texCollection = GetTextureCollection(mc.Collection);
                var texturesById = GetTextureDictionaryByCollection(mpdFile, texCollection);
                var animationsById = GetAnimationDictionaryByCollection(mpdFile, texCollection);

                foreach (var id in uniqueModelIDs) {
                    if (id == -1)
                        continue;

                    var sglModel = sglModelsByID.TryGetValue(id, out var sglModelOut) ? sglModelOut : null;
                    if (sglModel == null)
                        sglModelsByID[id] = sglModel = mc.GetSGLModel(id);
                    if (sglModel == null)
                        continue;

                    // Don't render movable models; they're not placed on the map in that way.
                    if (!mc.IsMovableModelCollection()) {
                        bool isForcedSemiTransparent = modelsWith2000Tag.Contains(id);
                        bool isHideMesh = modelsWith3000Tag.Contains(id);
                        CreateAndAddQuadModels(mpdFile, mc.Collection, sglModel, texturesById, animationsById, isForcedSemiTransparent, isHideMesh);
                    }
                }
            }

            ModelInstances = modelInstanceList.ToArray();
        }

        public void Update(IMPD_File mpdFile, IMPD_ModelCollection models, ISGL_Model sglModel,
            bool forceSemiTransparent = false, bool isHideMesh = false,
            float rotX = 0f, float rotY = 0f, float rotZ = 0f,
            float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f
        ) {
            Reset();
            if (models == null || sglModel == null)
                return;

            InitDictsForType(models.Collection);
            SGL_ModelsByIDByCollection[models.Collection][sglModel.ID] = sglModel;

            var texCollection  = GetTextureCollection(models.Collection);
            var texturesById   = GetTextureDictionaryByCollection(mpdFile, texCollection);
            var animationsById = GetAnimationDictionaryByCollection(mpdFile, texCollection);

            CreateAndAddQuadModels(mpdFile, models.Collection, sglModel, texturesById, animationsById, forceSemiTransparent, isHideMesh);

            var modelInstance = new MPD_ModelInstance() {
                Collection = models.Collection,
                ID = 0,
                ModelID = sglModel.ID,
                PositionX = -32 * 32,
                PositionZ = -32 * 32,
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
            IMPD_File mpdFile,
            ModelCollectionType modelCollection,
            ISGL_Model sglModel,
            Dictionary<int, ITexture> texturesById,
            Dictionary<int, ModelAnimationInfo> animationsById,
            bool forceSemiTransparent,
            bool isHideMesh
        ) {
            TextureFlipType ToggleHorizontalFlipping(TextureFlipType flip)
                => (flip & ~TextureFlipType.Horizontal) | (TextureFlipType) (TextureFlipType.Horizontal - (flip & TextureFlipType.Horizontal));

            var vertices = sglModel.Vertices;
            var faces = sglModel.Faces;

            bool modelExists = false;

            var hideQuads                      = new List<Quad>();
            var solidTexturedQuads             = new List<Quad>();
            var solidUntexturedQuads           = new List<Quad>();
            var semiTransparentTexturedQuads   = new List<Quad>();
            var semiTransparentUntexturedQuads = new List<Quad>();

            for (var i = 0; i < faces.Count; i++) {
                var polygon = faces[i];
                var attr = polygon.Attributes;

                var color = new Vector4(1);
                bool useTexture = attr.UseTexture;
                TextureAnimation anim = null;
                bool isSemiTransparent = false;
                TextureFlipType flip = TextureFlipType.NoFlip;

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
                    else if (forceSemiTransparent) {
                        var lightAdjTransparency = (mpdFile.LightAdjustment?.HasShadowTransparency == true)
                            ? (mpdFile.LightAdjustment.ShadowTransparency & 0x1F) / (float) 0x1F
                            : 0.5f;
                        transparency *= 1.0f - lightAdjTransparency;
                    }
                    if (attr.Mode_MESHon)
                        transparency *= 0.5f;

                    if (!useTexture) {
                        var colorChannels = PixelConversion.ABGR1555toChannels(attr.ColorNo);
                        color = new Vector4(colorChannels.r / 255.0f, colorChannels.g / 255.0f, colorChannels.b / 255.0f, 1.0f);
                    }
                    else {
                        if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                            if (animationsById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, animationsById[textureId].Textures, animationsById[textureId].FrameTimerStart);
                            // TODO: this isn't quite right... it should check to see if it's in the "Palette3" list
                            else if (animationsById.ContainsKey(textureId + 0x100))
                                anim = new TextureAnimation(textureId + 0x100, animationsById[textureId + 0x100].Textures, animationsById[textureId + 0x100].FrameTimerStart);
                            else if (texturesById.ContainsKey(textureId)) {
                                var tex = texturesById[textureId];
                                anim = new TextureAnimation(tex.ID, [tex], 0);
                            }
                        }

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
                var applyLighting = ((attr.UseLight || anim == null) && !useGouraud) ? 1.0f : 0.0f;
                var applyLightingVboData = new float[,] {{applyLighting}, {applyLighting}, {applyLighting}, {applyLighting}};

                void AddQuad() {
                    var newQuad = new Quad(polyVertices, anim, TextureRotateType.NoRotation, flip, color);

                    if (isHideMesh)
                        hideQuads.Add(newQuad);
                    else {
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, applyLightingVboData));
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

            var hideModel                      = (hideQuads.Count > 0)                      ? new QuadModel(hideQuads.ToArray())                   : null;
            var solidTexturedModel             = (solidTexturedQuads.Count > 0)             ? new QuadModel(solidTexturedQuads.ToArray())             : null;
            var solidUntexturedModel           = (solidUntexturedQuads.Count > 0)           ? new QuadModel(solidUntexturedQuads.ToArray())           : null;
            var semiTransparentTexturedModel   = (semiTransparentTexturedQuads.Count > 0)   ? new QuadModel(semiTransparentTexturedQuads.ToArray())   : null;
            var semiTransparentUntexturedModel = (semiTransparentUntexturedQuads.Count > 0) ? new QuadModel(semiTransparentUntexturedQuads.ToArray()) : null;

            if (modelExists) {
                ModelsByIDByCollection[modelCollection][sglModel.ID] = new ModelGroup(
                    solidTexturedModel,
                    solidUntexturedModel,
                    semiTransparentTexturedModel,
                    semiTransparentUntexturedModel,
                    hideModel
                );
            }
        }

        public Dictionary<ModelCollectionType, Dictionary<int, ModelGroup>> ModelsByIDByCollection { get; } = [];
        public Dictionary<ModelCollectionType, Dictionary<int, ISGL_Model>> SGL_ModelsByIDByCollection { get; } = [];
        public IMPD_ModelInstance[] ModelInstances { get; private set; }

        public bool ApplyShadowTags { get; set; } = false;
        public bool ApplyHideTags { get; set; } = false;
    }
}
