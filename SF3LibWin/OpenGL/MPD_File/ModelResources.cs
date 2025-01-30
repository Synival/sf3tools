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
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Types;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class ModelResources : IDisposable {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                Reset();

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ModelResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public void Reset() {
            foreach (var model in ModelsByMemoryAddress)
                model.Value.Dispose();
            ModelsByMemoryAddress.Clear();
            Models = null;
        }

        public void UpdateModels(IMPD_File mpdFile) {
            Reset();

            if (mpdFile.ModelCollections == null)
                return;

            var modelsList = new List<Model>();

            foreach (var models in mpdFile.ModelCollections) {
                modelsList.AddRange(models.ModelTable.Rows);

                var uniquePData1Addresses = models.ModelTable
                    .Select(x => x.PData1)
                    .Where(x => x != 0)
                    .Distinct();

                foreach (var address in uniquePData1Addresses)
                    AddModel(mpdFile, models.TextureCollection, address);
            }

            Models = modelsList.ToArray();
        }

        private TValue GetFromAnyCollection<TValue>(IMPD_File mpdFile, Func<Models.Files.MPD.ModelCollection, Dictionary<int, TValue>> tableGetter, int address) where TValue : class {
            foreach (var mc in mpdFile.ModelCollections) {
                if (mc != null) {
                    var dict = tableGetter(mc);
                    if (dict.TryGetValue(address, out TValue value))
                        return value;
                }
            }
            return null;
        }

        public void AddModel(IMPD_File mpdFile, TextureCollectionType texCollection, int pdataAddressInMemory) {
            TextureFlipType ToggleHorizontalFlipping(TextureFlipType flip)
                => (flip & ~TextureFlipType.Horizontal) | (TextureFlipType) (TextureFlipType.Horizontal - (flip & TextureFlipType.Horizontal));

            var pdata = GetFromAnyCollection(mpdFile, mc => mc.PDatasByMemoryAddress, pdataAddressInMemory);
            if (pdata == null)
                return;

            var vertices = GetFromAnyCollection(mpdFile, mc => mc.VertexTablesByMemoryAddress,  pdata.VerticesOffset);
            var polygons = GetFromAnyCollection(mpdFile, mc => mc.PolygonTablesByMemoryAddress, pdata.PolygonsOffset);
            var attrs    = GetFromAnyCollection(mpdFile, mc => mc.AttrTablesByMemoryAddress,    pdata.AttributesOffset);
            if (vertices == null || polygons == null || attrs == null)
                return;

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
                .ToDictionary(x => (int) x.TextureID, x => new { Textures = x.Frames.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray(), x.FrameTimerStart })
                : [];

            bool modelExists = false;
            var quads = new List<Quad>();
            var semiTransparentQuads = new List<Quad>();

            for (var i = 0; i < polygons.Length; i++) {
                var polygon = polygons[i];
                var attr = attrs[i];

                // Get texture. Fetch animated textures if possible.
                var textureId = attr.TextureNo;
                TextureAnimation anim = null;
                if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                    if (animationsById.ContainsKey(textureId))
                        anim = new TextureAnimation(textureId, animationsById[textureId].Textures, animationsById[textureId].FrameTimerStart);
                    else if (texturesById.ContainsKey(textureId))
                        anim = new TextureAnimation(textureId, [texturesById[textureId]], 0);
                }

                // Get texture flipping. Manually flip them horizontally to account for the weird thing where the X coordinates are reversed.
                var flip = (TextureFlipType) (attr.Dir & 0x0030);
                flip = ToggleHorizontalFlipping(flip);

                var color = new Vector4(1);

                // Apply semi-transparency for the appropriate draw mode.
                if (attr.Mode_DrawMode == DrawMode.CL_Trans)
                    color[3] /= 2;

                // TODO: wtf do these mean???
                if (attr.Mode_ECdis && attr.Mode_SPdis) {
                    var colorChannels = PixelConversion.ABGR1555toChannels(attr.ColorNo);
                    color = new Vector4(colorChannels.r, colorChannels.g, colorChannels.b, 1.0f);
                }
                // TODO: There isn't always a texture. What to do?
                else if (anim == null)
                    continue;

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

                var applyLighting = attr.ApplyLighting ? 1.0f : 0.0f;
                var applyLightingVboData = new float[,] {{applyLighting}, {applyLighting}, {applyLighting}, {applyLighting}};

                void AddQuad() {
                    var newQuad = new Quad(polyVertices, anim, TextureRotateType.NoRotation, flip, color);
                    newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                    newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, applyLightingVboData));

                    if (attr.Mode_DrawMode == DrawMode.CL_Trans)
                        semiTransparentQuads.Add(newQuad);
                    else
                        quads.Add(newQuad);
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

            var solidTexturedModel           = (quads.Count > 0)                ? new QuadModel(quads.ToArray()) : null;
            var semiTransparentTexturedModel = (semiTransparentQuads.Count > 0) ? new QuadModel(semiTransparentQuads.ToArray()) : null;

            if (modelExists) {
                ModelsByMemoryAddress[pdataAddressInMemory] = new ModelGroup(
                    solidTexturedModel,
                    semiTransparentTexturedModel
                );
            }
        }

        public Dictionary<int, ModelGroup> ModelsByMemoryAddress { get; } = [];
        public Model[] Models { get; private set; }
    }
}
