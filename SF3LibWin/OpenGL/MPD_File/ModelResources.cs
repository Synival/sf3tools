using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
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
                        anim = new TextureAnimation(textureId, animationsById[textureId]);
                    else if (texturesById.ContainsKey(textureId))
                        anim = new TextureAnimation(textureId, [texturesById[textureId]]);
                }

                var flip = (TextureFlipType) (attr.Dir & 0x00F0);

                var color = new Vector4(1);

                // Apply semi-transparency for the appropriate draw mode.
                if (attr.Mode_DrawMode == DrawMode.CL_Trans)
                    color[3] /= 2;

                // TODO: Proper color handling!

                // TODO: There isn't always a texture. What to do?
                if (anim == null)
                    continue;

                VertexModel[] polyVertexModels = [
                    vertices[polygon.Vertex4],
                    vertices[polygon.Vertex3],
                    vertices[polygon.Vertex2],
                    vertices[polygon.Vertex1],
                ];

                var polyVertices = polyVertexModels
                    .Select(x => new Vector3(-x.X, -x.Y, x.Z) * new Vector3(1 / 32.0f))
                    .ToArray();

                var twoSided = ((attr.Flag & 0x01) == 0x01) ? 1.0f : 0.0f;
                var twoSidedVboData = new float[,] {{twoSided}, {twoSided}, {twoSided}, {twoSided}};

                var normal = new Vector3(-polygon.NormalX, -polygon.NormalY, polygon.NormalZ);
                var vertexNormals = new Vector3[] { normal, normal, normal, normal };
                var normalVboData = vertexNormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                var applyLighting = ((attr.Sort & 0x08) == 0x08) ? 1.0f : 0.0f;
                var applyLightingVboData = new float[,] {{applyLighting}, {applyLighting}, {applyLighting}, {applyLighting}};

                var newQuad = new Quad(polyVertices, anim, TextureRotateType.NoRotation, flip, color);
                newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "twoSided", 4, twoSidedVboData));
                newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, applyLightingVboData));

                if (attr.Mode_DrawMode == DrawMode.CL_Trans)
                    semiTransparentQuads.Add(newQuad);
                else
                    quads.Add(newQuad);

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
