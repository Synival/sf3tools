using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.SGL;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class ModelResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            foreach (var model in ModelsByMemoryAddress)
                model.Value.Dispose();

            ModelsByMemoryAddress.Clear();
            PDatasByAddress.Clear();
            VerticesByAddress.Clear();
            PolygonsByAddress.Clear();
            AttributesByAddress.Clear();

            Models = null;
        }

        public void Update(IMPD_File mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var modelsList = new List<ModelBase>();
            foreach (var models in mpdFile.ModelCollections) {
                if (models.ModelTable != null)
                    modelsList.AddRange(models.ModelTable.Rows);
                if (models.MovableModelTable != null)
                    modelsList.AddRange(models.MovableModelTable.Rows);

                var uniquePData0Addresses = modelsList
                    .Select(x => x.PData0)
                    .Where(x => x != 0)
                    .Distinct()
                    .ToArray();

                foreach (var address in uniquePData0Addresses) {
                    var pdata = PDatasByAddress.TryGetValue(address, out var pdataVal) ? pdataVal : null;
                    if (pdata == null) {
                        pdata = GetFromAnyCollection(mpdFile, mc => mc.PDatasByMemoryAddress, address);
                        PDatasByAddress[address] = pdata;
                    }
                    if (pdata == null)
                        continue;

                    if (!VerticesByAddress.ContainsKey(pdata.VerticesOffset)) {
                        var table = GetFromAnyCollection(mpdFile, mc => mc.VertexTablesByMemoryAddress, pdata.VerticesOffset);
                        VerticesByAddress[pdata.VerticesOffset] = table.ToArray();
                    }

                    if (!PolygonsByAddress.ContainsKey(pdata.PolygonsOffset)) {
                        var table = GetFromAnyCollection(mpdFile, mc => mc.PolygonTablesByMemoryAddress, pdata.PolygonsOffset);
                        PolygonsByAddress[pdata.PolygonsOffset] = table.ToArray();
                    }

                    if (!AttributesByAddress.ContainsKey(pdata.AttributesOffset)) {
                        var table = GetFromAnyCollection(mpdFile, mc => mc.AttrTablesByMemoryAddress, pdata.AttributesOffset);
                        AttributesByAddress[pdata.AttributesOffset] = table.ToArray();
                    }

                    AddModel(mpdFile, models.CollectionType, pdata);
                }
            }

            Models = modelsList.ToArray();
        }

        public void Update(IMPD_File mpdFile, ModelCollection models, PDataModel pdata) {
            Reset();
            if (models == null || pdata == null)
                return;

            PDatasByAddress[pdata.RamAddress]           = pdata;
            VerticesByAddress[pdata.VerticesOffset]     = models.VertexTablesByMemoryAddress .TryGetValue(pdata.VerticesOffset,   out var vv) ? vv.ToArray() : null;
            PolygonsByAddress[pdata.PolygonsOffset]     = models.PolygonTablesByMemoryAddress.TryGetValue(pdata.PolygonsOffset,   out var pv) ? pv.ToArray() : null;
            AttributesByAddress[pdata.AttributesOffset] = models.AttrTablesByMemoryAddress   .TryGetValue(pdata.AttributesOffset, out var av) ? av.ToArray() : null;

            AddModel(mpdFile, models.CollectionType, pdata);

            var model = new Model(new ByteData.ByteData(new ByteArray(256)), 0, "Model", 0, true);
            model.PData0 = pdata.RamAddress;
            model.PositionX = -32 * 32;
            model.PositionZ = -32 * 32;
            model.ScaleX = 1.0f;
            model.ScaleY = 1.0f;
            model.ScaleZ = 1.0f;

            Models = [model];
        }

        private TValue GetFromAnyCollection<TValue>(IMPD_File mpdFile, Func<ModelCollection, Dictionary<uint, TValue>> tableGetter, uint address) where TValue : class {
            foreach (var mc in mpdFile.ModelCollections) {
                if (mc != null) {
                    var dict = tableGetter(mc);
                    if (dict.TryGetValue(address, out TValue value))
                        return value;
                }
            }
            return null;
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

        public void AddModel(IMPD_File mpdFile, ModelCollectionType modelCollection, PDataModel pdata) {
            TextureFlipType ToggleHorizontalFlipping(TextureFlipType flip)
                => (flip & ~TextureFlipType.Horizontal) | (TextureFlipType) (TextureFlipType.Horizontal - (flip & TextureFlipType.Horizontal));

            VertexModel[]  vertices = null;
            PolygonModel[] polygons = null;
            AttrModel[]    attrs    = null;

            try {
                vertices = VerticesByAddress[pdata.VerticesOffset];
                polygons = PolygonsByAddress[pdata.PolygonsOffset];
                attrs    = AttributesByAddress[pdata.AttributesOffset];
            }
            catch {
                // TODO: what to do in this case??
                return;
            }

            if (vertices == null || polygons == null || attrs == null)
                return;

            var texCollection = GetTextureCollection(modelCollection);

            var texturesById = mpdFile.TextureCollections != null ? mpdFile.TextureCollections
                .Where(x => x?.TextureTable != null && x.TextureTable.Collection == texCollection)
                .SelectMany(x => x.TextureTable)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = (texCollection == TextureCollectionType.PrimaryTextures && mpdFile.TextureAnimations != null) ? mpdFile.TextureAnimations
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => new { Textures = x.FrameTable.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray(), x.FrameTimerStart })
                : [];

            bool modelExists = false;

            var solidTexturedQuads             = new List<Quad>();
            var solidUntexturedQuads           = new List<Quad>();
            var semiTransparentTexturedQuads   = new List<Quad>();
            var semiTransparentUntexturedQuads = new List<Quad>();

            for (var i = 0; i < polygons.Length; i++) {
                var polygon = polygons[i];
                var attr = attrs[i];

                // Get texture. Fetch animated textures if possible.
                var textureId = attr.TextureNo;

                // Get texture flipping. Manually flip them horizontally to account for the weird thing where the X coordinates are reversed.
                var flip = (TextureFlipType) (attr.Dir & 0x0030);
                flip = ToggleHorizontalFlipping(flip);

                var color = new Vector4(1);

                // Apply semi-transparency for the appropriate draw mode.
                var isSemiTransparent = attr.Mode_DrawMode == DrawMode.CL_Trans;

                TextureAnimation anim = null;

                if (!attr.HasTexture) {
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
                        else if (texturesById.ContainsKey(textureId))
                            anim = new TextureAnimation(textureId, [texturesById[textureId]], 0);
                    }
                    // TODO: what to do for missing textures?
                    if (anim == null)
                        continue;
                    else if (anim.Frames.Length > 0 && anim.Frames[0].PixelFormat == TexturePixelFormat.Palette3) {
                        var transparency = mpdFile.LightAdjustment != null ? (mpdFile.LightAdjustment.Palette3Transparency & 0x1F) / (float) 0x1F : 0.5f;
                        color[3] *= 1.0f - transparency;
                    }
                }

                if (isSemiTransparent)
                    color[3] /= 2;

                VECTOR[] polyVertexModels = [
                    vertices[polygon.Vertex1].Vector,
                    vertices[polygon.Vertex2].Vector,
                    vertices[polygon.Vertex3].Vector,
                    vertices[polygon.Vertex4].Vector,
                ];

                var polyVertices = polyVertexModels
                    .Select(x => new Vector3(-x.X.Float, -x.Y.Float, x.Z.Float) * new Vector3(1 / 32.0f))
                    .ToArray();

                var normal = new Vector3(-polygon.NormalX, -polygon.NormalY, polygon.NormalZ);
                var vertexNormals = new Vector3[] { normal, normal, normal, normal };
                var normalVboData = vertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                var applyLighting = (attr.ApplyLighting || anim == null) ? 1.0f : 0.0f;
                var applyLightingVboData = new float[,] {{applyLighting}, {applyLighting}, {applyLighting}, {applyLighting}};

                void AddQuad() {
                    var newQuad = new Quad(polyVertices, anim, TextureRotateType.NoRotation, flip, color);
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

                // Add quads first...
                AddQuad();

                // ...then add the other side, if it's there.
                if (attr.TwoSided) {
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

            var solidTexturedModel             = (solidTexturedQuads.Count > 0)             ? new QuadModel(solidTexturedQuads.ToArray())              : null;
            var solidUntexturedModel           = (solidUntexturedQuads.Count > 0)           ? new QuadModel(solidUntexturedQuads.ToArray())            : null;
            var semiTransparentTexturedModel   = (semiTransparentTexturedQuads.Count > 0)   ? new QuadModel(semiTransparentTexturedQuads.ToArray())   : null;
            var semiTransparentUntexturedModel = (semiTransparentUntexturedQuads.Count > 0) ? new QuadModel(semiTransparentUntexturedQuads.ToArray()) : null;

            if (modelExists) {
                    ModelsByMemoryAddress[pdata.RamAddress] = new ModelGroup(
                    solidTexturedModel,
                    solidUntexturedModel,
                    semiTransparentTexturedModel,
                    semiTransparentUntexturedModel
                );
            }
        }

        public Dictionary<uint, ModelGroup> ModelsByMemoryAddress { get; } = [];
        public Dictionary<uint, PDataModel> PDatasByAddress { get; } = [];
        public Dictionary<uint, VertexModel[]> VerticesByAddress { get; } = [];
        public Dictionary<uint, PolygonModel[]> PolygonsByAddress { get; } = [];
        public Dictionary<uint, AttrModel[]> AttributesByAddress { get; } = [];
        public ModelBase[] Models { get; private set; }
    }
}
