using System;
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

namespace SF3.Win.OpenGL.GLResources.Shared {
    public class ModelResources : ResourcesBase {
        public ModelResources(bool applyShadowTags, bool applyHideTags) : base() {
            ApplyShadowTags = applyShadowTags;
            ApplyHideTags   = applyHideTags;
        }

        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            foreach (var modelsDict in ModelGroupsByIDByCollection.Values)
                foreach (var model in modelsDict.Values)
                    model.Dispose();

            foreach (var anim in _mockAnims)
                anim.Dispose();
            _mockAnims.Clear();

            ModelGroupsByIDByCollection.Clear();
            SGL_ModelsByIDByCollection.Clear();

            ModelInstances = null;
        }

        protected void InitDictsForType(int collection) {
            // TODO: Just have one structure with all this info!
            if (!ModelGroupsByIDByCollection.ContainsKey(collection))
                ModelGroupsByIDByCollection[collection] = [];
            if (!SGL_ModelsByIDByCollection.ContainsKey(collection))
                SGL_ModelsByIDByCollection[collection] = [];
        }

        public void Update(
            ISGL_Model sglModel, Dictionary<int, IAnimatableTexture> texturesById, Func<ISGL_ModelInstance> instCreator,
            float? forceSemiTransparentValue, bool isHideMesh, bool? forceLighting
        ) {
            Reset();
            if (sglModel == null || texturesById == null || instCreator == null)
                return;

            var collectionId = sglModel.ModelCollectionID;
            InitDictsForType(collectionId);
            SGL_ModelsByIDByCollection[collectionId][sglModel.ModelID] = sglModel;
            CreateAndAddQuadModels(collectionId, sglModel, texturesById, forceSemiTransparentValue, isHideMesh, forceLighting);
            ModelInstances = [instCreator()];
        }

        protected void CreateAndAddQuadModels(
            int modelCollection,
            ISGL_Model model,
            Dictionary<int, IAnimatableTexture> texturesById,
            float? forceSemiTransparentAlpha,
            bool isHideMesh,
            bool? forceLighting
        ) {
            TextureFlipType ToggleHorizontalFlipping(TextureFlipType flip)
                => flip & ~TextureFlipType.Horizontal | (TextureFlipType) (TextureFlipType.Horizontal - (flip & TextureFlipType.Horizontal));

            var vertices      = model.Vertices;
            var faces         = model.Faces;
            var vertexNormals = model.VertexNormals;

            var modelExists = false;

            var hideQuads                      = new List<Quad>();
            var solidTexturedQuads             = new List<Quad>();
            var solidUntexturedQuads           = new List<Quad>();
            var semiTransparentTexturedQuads   = new List<Quad>();
            var semiTransparentUntexturedQuads = new List<Quad>();

            var forceLightingValue = forceLighting.HasValue ? (forceLighting.Value ? 1.0f : 0.0f) : (float?) null;

            for (var i = 0; i < faces.Count; i++) {
                var polygon = faces[i];
                var attr = polygon.Attributes;

                var color = new Vector4(1);
                var useTexture = attr.UseTexture;
                IAnimatedTexture anim = null;
                var isSemiTransparent = false;
                var flip = TextureFlipType.NoFlip;
                MockAnimatedTexture mockAnim = null;

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
                    else if (forceSemiTransparentAlpha.HasValue)
                        transparency *= forceSemiTransparentAlpha.Value;

                    if (!useTexture) {
                        var colorChannels = PixelConversion.ABGR1555toChannels(attr.ColorNo);
                        color = new Vector4(colorChannels.R / 255.0f, colorChannels.G / 255.0f, colorChannels.B / 255.0f, 1.0f);
                    }
                    else {
                        if (textureId != 0xFF && texturesById.ContainsKey(textureId))
                            if (texturesById.TryGetValue(textureId, out var tex))
                                anim = tex.Animation ?? (mockAnim = new MockAnimatedTexture(tex));

                        // If the texture is missing, mark this polygon bright red.
                        if (anim == null) {
                            useTexture = false;
                            color = new Vector4(1f, 0f, 0f, 1f);
                        }
                    }

                    // If forcing semi-transparency, and there aren't any already-indexed textures, force color to black.
                    // (This isn't how this actually works, but this is fine for display.)
                    if (forceSemiTransparentAlpha.HasValue && (anim == null || anim.Frames.All(x => x.BytesPerPixel == 2)))
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

                var vertexIndices = polygon.VertexIndices;
                VECTOR[] polyVerticesOrig = [
                    vertices[vertexIndices[0]],
                    vertices[vertexIndices[1]],
                    vertices[vertexIndices[2]],
                    vertices[vertexIndices[3]],
                ];

                var polyVertices = polyVerticesOrig
                    .Select(x => new Vector3(-x.X.Float, -x.Y.Float, x.Z.Float) * new Vector3(1 / 32.0f))
                    .ToArray();

                Vector3[] quadVertexNormals;
                if (vertexNormals == null) {
                    var normal = new Vector3(-polygon.Normal.X.Float, -polygon.Normal.Y.Float, polygon.Normal.Z.Float);
                    quadVertexNormals = [normal, normal, normal, normal];
                }
                else {
                    VECTOR[] quadVertexNormalsOrig = [
                        vertexNormals[vertexIndices[0]],
                        vertexNormals[vertexIndices[1]],
                        vertexNormals[vertexIndices[2]],
                        vertexNormals[vertexIndices[3]],
                    ];
                    quadVertexNormals = quadVertexNormalsOrig
                        .Select(x => new Vector3(-x.X.Float, -x.Y.Float, x.Z.Float))
                        .ToArray();
                }

                var normalVboData = quadVertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                var useGouraud = attr.CL_Gouraud && useTexture;
                var applyLighting = forceLightingValue ?? ((attr.UseLight || anim == null) && !useGouraud ? 1.0f : 0.0f);
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
                    for (var j = 0; j < 4; j++)
                        quadVertexNormals[j] = -quadVertexNormals[j];
                    normalVboData = quadVertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                    // Add the flipped quad.
                    AddQuad();
                }

                // Clean-up.
                if (mockAnim != null)
                    _mockAnims.Add(mockAnim);

                modelExists = true;
            }

            var hideModel                      = hideQuads.Count > 0                      ? new QuadModel(hideQuads.ToArray())                   : null;
            var solidTexturedModel             = solidTexturedQuads.Count > 0             ? new QuadModel(solidTexturedQuads.ToArray())             : null;
            var solidUntexturedModel           = solidUntexturedQuads.Count > 0           ? new QuadModel(solidUntexturedQuads.ToArray())           : null;
            var semiTransparentTexturedModel   = semiTransparentTexturedQuads.Count > 0   ? new QuadModel(semiTransparentTexturedQuads.ToArray())   : null;
            var semiTransparentUntexturedModel = semiTransparentUntexturedQuads.Count > 0 ? new QuadModel(semiTransparentUntexturedQuads.ToArray()) : null;

            if (modelExists) {
                ModelGroupsByIDByCollection[(int) modelCollection][model.ModelID] = new ModelGroup(
                    solidTexturedModel,
                    solidUntexturedModel,
                    semiTransparentTexturedModel,
                    semiTransparentUntexturedModel,
                    hideModel
                );
            }
        }

        public Dictionary<int, Dictionary<int, ModelGroup>> ModelGroupsByIDByCollection { get; } = [];
        public Dictionary<int, Dictionary<int, ISGL_Model>> SGL_ModelsByIDByCollection { get; } = [];
        public ISGL_ModelInstance[] ModelInstances { get; protected set; }

        public bool ApplyShadowTags { get; set; } = false;
        public bool ApplyHideTags { get; set; } = false;

        private List<MockAnimatedTexture> _mockAnims = new List<MockAnimatedTexture>();
    }
}
