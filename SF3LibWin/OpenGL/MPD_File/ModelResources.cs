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
            foreach (var modelsDict in ModelsByMemoryAddressByCollection.Values)
                foreach (var model in modelsDict.Values)
                    model.Dispose();

            ModelsByMemoryAddressByCollection.Clear();
            PDatasByAddressByCollection.Clear();
            VerticesByAddressByCollection.Clear();
            PolygonsByAddressByCollection.Clear();
            AttributesByAddressByCollection.Clear();

            Models = null;
        }

        private void InitDictsForType(ModelCollectionType collectionType) {
            // TODO: Just have one structure with all this info!
            if (!ModelsByMemoryAddressByCollection.ContainsKey(collectionType))
                ModelsByMemoryAddressByCollection[collectionType] = [];
            if (!PDatasByAddressByCollection.ContainsKey(collectionType))
                PDatasByAddressByCollection[collectionType] = [];
            if (!VerticesByAddressByCollection.ContainsKey(collectionType))
                VerticesByAddressByCollection[collectionType] = [];
            if (!PolygonsByAddressByCollection.ContainsKey(collectionType))
                PolygonsByAddressByCollection[collectionType] = [];
            if (!AttributesByAddressByCollection.ContainsKey(collectionType))
                AttributesByAddressByCollection[collectionType] = [];
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

        private Dictionary<int, ITexture> GetTexturesByID(IMPD_File mpdFile, TextureCollectionType texCollection) {
            return mpdFile.TextureCollections != null ? mpdFile.TextureCollections
                .Where(x => x?.TextureTable != null && x.TextureTable.Collection == texCollection)
                .SelectMany(x => x.TextureTable)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];
        }

        private Dictionary<int, ModelAnimationInfo> GetAnimationsByID(IMPD_File mpdFile, TextureCollectionType texCollection) {
            return (texCollection == TextureCollectionType.PrimaryTextures && mpdFile.TextureAnimations != null) ? mpdFile.TextureAnimations
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => new ModelAnimationInfo {
                    Textures = x.FrameTable.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray(),
                    FrameTimerStart = x.FrameTimerStart
                }) : [];
        }

        public void Update(IMPD_File mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var modelsList = new List<ModelBase>();
            foreach (var models in mpdFile.ModelCollections) {
                if (models.CollectionType != ModelCollectionType.Chunk19Model && models.CollectionType != ModelCollectionType.PrimaryModels)
                    continue;

                InitDictsForType(models.CollectionType);
                var pdatasByAddress     = PDatasByAddressByCollection[models.CollectionType];
                var verticesByAddress   = VerticesByAddressByCollection[models.CollectionType];
                var polygonsByAddress   = PolygonsByAddressByCollection[models.CollectionType];
                var attributesByAddress = AttributesByAddressByCollection[models.CollectionType];

                if (models.ModelTable != null)
                    modelsList.AddRange(models.ModelTable.Rows);
                if (models.MovableModelTable != null)
                    modelsList.AddRange(models.MovableModelTable.Rows);

                var uniquePData0Addresses = modelsList
                    .Select(x => x.PData0)
                    .Where(x => x != 0)
                    .Distinct()
                    .ToArray();

                var texCollection = GetTextureCollection(models.CollectionType);
                var texturesById = GetTexturesByID(mpdFile, texCollection);
                var animationsById = GetAnimationsByID(mpdFile, texCollection);

                foreach (var address in uniquePData0Addresses) {
                    var pdata = pdatasByAddress.TryGetValue(address, out var pdataVal) ? pdataVal : null;
                    if (pdata == null) {
                        pdata = GetFromAnyCollection(mpdFile, mc => mc.PDatasByMemoryAddress, address);
                        pdatasByAddress[address] = pdata;
                    }
                    if (pdata == null)
                        continue;

                    if (!verticesByAddress.ContainsKey(pdata.VerticesOffset)) {
                        var table = GetFromAnyCollection(mpdFile, mc => mc.VertexTablesByMemoryAddress, pdata.VerticesOffset);
                        verticesByAddress[pdata.VerticesOffset] = table.ToArray();
                    }

                    if (!polygonsByAddress.ContainsKey(pdata.PolygonsOffset)) {
                        var table = GetFromAnyCollection(mpdFile, mc => mc.PolygonTablesByMemoryAddress, pdata.PolygonsOffset);
                        polygonsByAddress[pdata.PolygonsOffset] = table.ToArray();
                    }

                    if (!attributesByAddress.ContainsKey(pdata.AttributesOffset)) {
                        var table = GetFromAnyCollection(mpdFile, mc => mc.AttrTablesByMemoryAddress, pdata.AttributesOffset);
                        attributesByAddress[pdata.AttributesOffset] = table.ToArray();
                    }

                    // Don't render movable models; they're not placed on the map in that way.
                    if (!models.IsMovableModelCollection)
                        CreateAndAddQuadModels(mpdFile, models.CollectionType, pdata, texturesById, animationsById);
                }
            }

            Models = modelsList.ToArray();
        }

        public void Update(IMPD_File mpdFile, ModelCollection models, PDataModel pdata) {
            Reset();
            if (models == null || pdata == null)
                return;

            InitDictsForType(models.CollectionType);
            PDatasByAddressByCollection[models.CollectionType][pdata.RamAddress]           = pdata;
            VerticesByAddressByCollection[models.CollectionType][pdata.VerticesOffset]     = models.VertexTablesByMemoryAddress .TryGetValue(pdata.VerticesOffset,   out var vv) ? vv.ToArray() : null;
            PolygonsByAddressByCollection[models.CollectionType][pdata.PolygonsOffset]     = models.PolygonTablesByMemoryAddress.TryGetValue(pdata.PolygonsOffset,   out var pv) ? pv.ToArray() : null;
            AttributesByAddressByCollection[models.CollectionType][pdata.AttributesOffset] = models.AttrTablesByMemoryAddress   .TryGetValue(pdata.AttributesOffset, out var av) ? av.ToArray() : null;

            var texCollection = GetTextureCollection(models.CollectionType);
            var texturesById = GetTexturesByID(mpdFile, texCollection);
            var animationsById = GetAnimationsByID(mpdFile, texCollection);

            CreateAndAddQuadModels(mpdFile, models.CollectionType, pdata, texturesById, animationsById);

            var model = new Model(new ByteData.ByteData(new ByteArray(256)), 0, "Model", 0, true, models.CollectionType);
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

        private void CreateAndAddQuadModels(
            IMPD_File mpdFile,
            ModelCollectionType modelCollection,
            PDataModel pdata,
            Dictionary<int, ITexture> texturesById,
            Dictionary<int, ModelAnimationInfo> animationsById
        ) {
            TextureFlipType ToggleHorizontalFlipping(TextureFlipType flip)
                => (flip & ~TextureFlipType.Horizontal) | (TextureFlipType) (TextureFlipType.Horizontal - (flip & TextureFlipType.Horizontal));

            VertexModel[]  vertices = null;
            PolygonModel[] polygons = null;
            AttrModel[]    attrs    = null;

            try {
                vertices = VerticesByAddressByCollection[modelCollection][pdata.VerticesOffset];
                polygons = PolygonsByAddressByCollection[modelCollection][pdata.PolygonsOffset];
                attrs    = AttributesByAddressByCollection[modelCollection][pdata.AttributesOffset];
            }
            catch {
                // TODO: what to do in this case??
                return;
            }

            if (vertices == null || polygons == null || attrs == null)
                return;

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
                ModelsByMemoryAddressByCollection[modelCollection][pdata.RamAddress] = new ModelGroup(
                    solidTexturedModel,
                    solidUntexturedModel,
                    semiTransparentTexturedModel,
                    semiTransparentUntexturedModel
                );
            }
        }

        public Dictionary<ModelCollectionType, Dictionary<uint, ModelGroup>> ModelsByMemoryAddressByCollection { get; } = [];
        public Dictionary<ModelCollectionType, Dictionary<uint, PDataModel>> PDatasByAddressByCollection { get; } = [];
        public Dictionary<ModelCollectionType, Dictionary<uint, VertexModel[]>> VerticesByAddressByCollection { get; } = [];
        public Dictionary<ModelCollectionType, Dictionary<uint, PolygonModel[]>> PolygonsByAddressByCollection { get; } = [];
        public Dictionary<ModelCollectionType, Dictionary<uint, AttrModel[]>> AttributesByAddressByCollection { get; } = [];
        public ModelBase[] Models { get; private set; }
    }
}
