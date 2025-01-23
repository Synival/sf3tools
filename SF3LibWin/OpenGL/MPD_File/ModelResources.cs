using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class ModelResources {
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

            if (mpdFile.Models?.ModelTable == null)
                return;

            Models = mpdFile.Models.ModelTable.Rows;
            var uniquePData1Addresses = mpdFile.Models.ModelTable
                .Select(x => x.PData1)
                .Where(x => x != 0)
                .Distinct();

            foreach (var address in uniquePData1Addresses)
                AddModel(mpdFile, address);
        }

        public void AddModel(IMPD_File mpdFile, int pdataAddressInMemory) {
            var pdata    = mpdFile.Models.PDatasByMemoryAddress[pdataAddressInMemory];
            var vertices = mpdFile.Models.VertexTablesByMemoryAddress[pdata.VerticesOffset];
            var polygons = mpdFile.Models.PolygonTablesByMemoryAddress[pdata.PolygonsOffset];
            var attrs    = mpdFile.Models.AttrTablesByMemoryAddress[pdata.AttributesOffset];

            var texturesById = mpdFile.TextureCollections != null ? mpdFile.TextureCollections
                .Where(x => x?.TextureTable != null && x.TextureTable.Collection == TextureCollectionType.PrimaryTextures)
                .SelectMany(x => x.TextureTable)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = mpdFile.TextureAnimations != null ? mpdFile.TextureAnimations
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => x.Frames.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray())
                : [];

            var quads = new List<Quad>();
            for (var i = 0; i < polygons.Length; i++) {
                var polygon = polygons[i];
                var attr = attrs[i];

                // Skip transparent polygons.
                if ((attr.ColorNo & 0x8000) == 0x8000)
                    continue;

                // Get texture. Fetch animated textures if possible.
                // TODO: is there a way to specify UV coordinates?
                var textureId = attr.TextureNo;
                TextureAnimation anim = null;
                if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                    if (animationsById.ContainsKey(textureId))
                        anim = new TextureAnimation(textureId, animationsById[textureId]);
                    else if (texturesById.ContainsKey(textureId))
                        anim = new TextureAnimation(textureId, [texturesById[textureId]]);
                }

                var flip = (TextureFlipType) (attr.Dir & 0x00F0);

                var color = new Vector4(1);
                if (attr.ColorNo != 0) {
                    var c = PixelConversion.ABGR1555toChannels(attr.ColorNo);
                    color = new Vector4(c.r / 248.0f, c.g / 248.0f, c.b / 248.0f, 1.0f);
                }

                if (anim == null)
                    continue;

                VertexModel[] polyVertexModels = [
                    vertices[polygon.Vertex4],
                    vertices[polygon.Vertex3],
                    vertices[polygon.Vertex2],
                    vertices[polygon.Vertex1],
                ];

                var polyVertices = polyVertexModels
                    .Select(x => new Vector3(-x.X.Float, -x.Y.Float, x.Z.Float) * new Vector3(1 / 32.0f))
                    .ToArray();

                var twoSided = ((attr.Flag & 0x01) == 0x01) ? 1.0f : 0.0f;
                var twoSidedVboData = new float[,] {{twoSided}, {twoSided}, {twoSided}, {twoSided}};

                // TODO: Why are the normals backwards???
                var normal = new Vector3(-polygon.NormalX.Float, -polygon.NormalY.Float, polygon.NormalZ.Float);

                var vertexNormals = new Vector3[] { normal, normal, normal, normal };
                var normalVboData = vertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                var newQuad = new Quad(polyVertices, anim, TextureRotateType.NoRotation, flip, color);
                newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "twoSided", 4, twoSidedVboData));
                quads.Add(newQuad);
            }

            if (quads.Count > 0)
                ModelsByMemoryAddress[pdataAddressInMemory] = new QuadModel(quads.ToArray());
        }

        public Dictionary<int, QuadModel> ModelsByMemoryAddress { get; } = [];
        public Model[] Models { get; private set; }
    }
}
