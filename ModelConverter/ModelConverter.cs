using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json;
using CommonLib.Extensions;
using CommonLib.SGL;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;

namespace ModelConverter {
    public class ModelConverter {
        public byte[] ModelToGLB(ISGL_Model model) {
            var modelRoot = ModelRoot.CreateModel();

            Accessor CreateVector3Accessor(string name, Vector3[] data) {
                var bufferView = modelRoot.CreateBufferView(data.Length * sizeof(float) * 3);
                MemoryMarshal.Cast<Vector3, byte>(data.AsSpan()).CopyTo(bufferView.Content.AsSpan());

                var accessor = modelRoot.CreateAccessor(name);
                accessor.SetData(
                    bufferView,
                    0,
                    data.Length,
                    new AttributeFormat(
                        DimensionType.VEC3,
                        EncodingType.FLOAT,
                        nrm: false
                    )
                );
                accessor.UpdateBounds();

                return accessor;
            }

            Accessor CreateTriangeIndiciesAccessor(string name, ushort[,] data) {
                var bufferView = modelRoot.CreateBufferView(data.Length * sizeof(ushort));
                MemoryMarshal.Cast<ushort, byte>(data.To1DArray().AsSpan()).CopyTo(bufferView.Content.AsSpan());

                var accessor = modelRoot.CreateAccessor(name);
                accessor.SetData(
                    bufferView,
                    0,
                    data.Length,
                    new AttributeFormat(
                        DimensionType.SCALAR,
                        EncodingType.UNSIGNED_SHORT,
                        nrm: false
                    )
                );
                accessor.UpdateBounds();

                return accessor;
            }

            var mesh = modelRoot.CreateMesh();
            var primitive = mesh.CreatePrimitive();

            var vertexData = model.Vertices.Select(x => x.ToNumericsVector3()).ToArray();
            var vertexAccessor = CreateVector3Accessor("vertices", vertexData);
            primitive.SetVertexAccessor("POSITION", vertexAccessor);

            if (model.VertexNormals != null) {
                var vertexNormalData = model.VertexNormals.Select(x => x.ToNumericsVector3()).ToArray();
                var vertexNormalAccessor = CreateVector3Accessor("vertexNormals", vertexNormalData);
                primitive.SetVertexAccessor("NORMAL", vertexNormalAccessor);
            }

            var faceIndexData = model.Faces
                .SelectMany(x => {
                    var indices = x.VertexIndices;
                    return new ushort[] {
                        (ushort) indices[0], (ushort) indices[1], (ushort) indices[2],
                        (ushort) indices[2], (ushort) indices[3], (ushort) indices[0],
                    };
                })
                .ToArray()
                .To2DArray(model.Faces.Count * 2, 3);

            var indexAccessor = CreateTriangeIndiciesAccessor("indices", faceIndexData);
            primitive.IndexAccessor = indexAccessor;

            var scene = modelRoot.UseScene("scene");
            scene.CreateNode("node").WithMesh(mesh);

            var settings = new WriteSettings {
                JsonIndented = true,
                JsonOptions = new JsonWriterOptions() { NewLine = "\n" }
            };

            using (var stream = new MemoryStream()) {
                modelRoot.WriteGLB(stream, settings);
                return stream.ToArray();
            }
        }

        public ISGL_Model GLB_ToModel(byte[] glbFile, int? modelCollectionId, int? modelId, int? levelOfDetail) {
            var modelRoot = ModelRoot.ParseGLB(new ArraySegment<byte>(glbFile));

            var vertexAccessor = modelRoot.LogicalMeshes[0].Primitives[0].GetVertexAccessor("POSITION");
            var vertices = new Vector3[vertexAccessor.Count];
            vertexAccessor.AsVector3Array().CopyTo(vertices, 0);

            var vertexNormalAccessor = modelRoot.LogicalMeshes[0].Primitives[0].GetVertexAccessor("NORMAL");
            var vertexNormals = (vertexNormalAccessor == null) ? null : new Vector3[vertexNormalAccessor.Count];
            if (vertexNormalAccessor != null)
                vertexNormalAccessor.AsVector3Array().CopyTo(vertexNormals, 0);

            var indexAccessor = modelRoot.LogicalMeshes[0].Primitives[0].IndexAccessor;
            var indices = modelRoot.LogicalMeshes[0].Primitives[0].GetTriangleIndices().ToArray();

            var quadIndices = new int[indices.Length / 2][];
            int idx = 0;
            for (int i = 0; i < quadIndices.Length; i++) {
                quadIndices[i] = new int[] {
                    indices[idx].A,
                    indices[idx].B,
                    indices[idx].C,
                    indices[idx + 1].B
                };
                idx += 2;
            }

            return new SGL_Model(modelCollectionId ?? 0, modelId ?? 0, levelOfDetail ?? 0,
                vertices.Select(x => x.ToVECTOR()).ToArray(),
                quadIndices.Select(x => new SGL_ModelFace(x, new VECTOR(0, -1, 0), new ATTR())).ToArray(),
                vertexNormals.Select(x => x.ToVECTOR()).ToArray()
            );
        }
    }
}
