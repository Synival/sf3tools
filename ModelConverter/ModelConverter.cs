using System;
using System.Collections.Generic;
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
        private struct ConvertedVertex {
            public ConvertedVertex(int index, int originalIndex, Vector3 position, Vector3? normal, Vector2? texCoord0) {
                Index         = index;
                OriginalIndex = originalIndex;
                Position      = position;
                Normal        = normal;
                TexCoord0     = texCoord0;
            }

            public readonly int Index;
            public readonly int OriginalIndex;
            public readonly Vector3 Position;
            public readonly Vector3? Normal;
            public readonly Vector2? TexCoord0;
        }

        private struct Quad {
            public Quad(int index, ConvertedVertex[] vertices) {
                Index    = index;
                Vertices = vertices;
            }

            public readonly int Index;
            public readonly ConvertedVertex[] Vertices;
        }

        public byte[] ModelToGLB(ISGL_Model[] models) {
            // TODO: actual UV coordinates!

            var modelRoot = ModelRoot.CreateModel();

            Accessor CreateUShortAccessor(string name, BufferView bufferView, int offset, int count) {
                var accessor = modelRoot.CreateAccessor(name);
                var attrFormat = new AttributeFormat(DimensionType.SCALAR, EncodingType.UNSIGNED_SHORT, nrm: false);
                accessor.SetData(bufferView, offset, count, attrFormat);
                accessor.UpdateBounds();
                return accessor;
            }

            Accessor CreateVector2Accessor(string name, BufferView bufferView, int offset, int count) {
                var accessor = modelRoot.CreateAccessor(name);
                var attrFormat = new AttributeFormat(DimensionType.VEC2, EncodingType.FLOAT, nrm: false);
                accessor.SetData(bufferView, offset, count, attrFormat);                    
                accessor.UpdateBounds();
                return accessor;
            }

            Accessor CreateVector3Accessor(string name, BufferView bufferView, int offset, int count) {
                var accessor = modelRoot.CreateAccessor(name);
                var attrFormat = new AttributeFormat(DimensionType.VEC3, EncodingType.FLOAT, nrm: false);
                accessor.SetData(bufferView, offset, count, attrFormat);                    
                accessor.UpdateBounds();
                return accessor;
            }

            Accessor CreateTriangeIndiciesAccessor(string name, ushort[,] data) {
                var bufferView = modelRoot.CreateBufferView(data.Length * sizeof(ushort), 0, BufferMode.ELEMENT_ARRAY_BUFFER);
                MemoryMarshal.Cast<ushort, byte>(data.To1DArray().AsSpan()).CopyTo(bufferView.Content.AsSpan());
                var accessor = modelRoot.CreateAccessor(name);
                var attrFormat = new AttributeFormat(DimensionType.SCALAR, EncodingType.UNSIGNED_SHORT, nrm: false);
                accessor.SetData(bufferView, 0, data.Length, attrFormat);
                accessor.UpdateBounds();
                return accessor;
            }

            // Default scene.
            var scene = modelRoot.UseScene("scene");

            foreach (var model in models) {
                var mesh = modelRoot.CreateMesh();
                var primitive = mesh.CreatePrimitive();

                // Build quads, each with its own vertices. We're not going to have *ANY* shared vertices because we
                // *must* store unique ATTR data per-polygon. (We can at least share them between triangles)
                // Swap Y/Z coordinates to match the standard coordinate system.
                var quadList = new List<Quad>();
                int quadIdx = 0;
                int vertexIdx = 0;

                Vector3? GetVertexNormal(int idx) => (model.VertexNormals != null) ? model.VertexNormals[idx].ToNumericsVector3().ToSwappedYZ() : (Vector3?) null;
                Vector2 GetTexCoord0(int idx) => new Vector2(
                    ((idx + 1) / 2) % 2,
                    ((idx + 0) / 2) % 2
                );

                foreach (var face in model.Faces) {
                    var vertices = face.VertexIndices
                        .Select((x, i) => new ConvertedVertex(
                            vertexIdx + i,
                            x,
                            model.Vertices[x].ToNumericsVector3().ToSwappedYZ(),
                            GetVertexNormal(x),
                            GetTexCoord0(i)
                        ))
                        .ToArray();

                    vertexIdx += 4;
                    quadList.Add(new Quad(quadIdx++, vertices));
                }

                // Create a big buffer for the entire model.
                var vertexCount = quadList.Count * 4;
                var stride = (12 /*pos*/ + 12 /*normal*/ + 2/*quadIdx*/ + 2/*padding*/ + 2/*normalIdx*/ + 2/*padding*/ + 8/*texcoord_0*/);
                var bufferViewData = new byte[vertexCount * stride];
                var bufferView = modelRoot.UseBufferView(bufferViewData, 0, byteStride: stride, target: BufferMode.ARRAY_BUFFER);

                // Vertex attribute for position.
                var vertexData = quadList.SelectMany(x => x.Vertices.Select(y => y.Position)).ToArray();
                for (int i = 0; i < vertexCount; i++) {
                    var dataFloats = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride, 12));
                    dataFloats[0] = vertexData[i].X;
                    dataFloats[1] = vertexData[i].Y;
                    dataFloats[2] = vertexData[i].Z;
                }
                var vertexAccessor = CreateVector3Accessor("vertices", bufferView, 0, vertexCount);
                primitive.SetVertexAccessor("POSITION", vertexAccessor);

                // Vertex attribute for normals, if available.
                if (model.VertexNormals != null) {
                    var vertexNormalData = quadList.SelectMany(x => x.Vertices.Select(y => y.Normal.Value)).ToArray();
                    for (int i = 0; i < vertexCount; i++) {
                        var dataFloats = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride + 12, 12));
                        dataFloats[0] = vertexNormalData[i].X;
                        dataFloats[1] = vertexNormalData[i].Y;
                        dataFloats[2] = vertexNormalData[i].Z;
                    }
                    var vertexNormalAccessor = CreateVector3Accessor("vertexNormals", bufferView, 12, vertexCount);
                    primitive.SetVertexAccessor("NORMAL", vertexNormalAccessor);
                }

                // Vertex attribute that associates each vertex with a particular quad.
                var vertexQuadData = quadList.SelectMany(x => x.Vertices.Select(y => (ushort) x.Index)).ToArray();
                for (int i = 0; i < vertexCount; i++) {
                    var dataUShorts = MemoryMarshal.Cast<byte, ushort>(bufferViewData.AsSpan().Slice(i * stride + 24, 2));
                    dataUShorts[0] = vertexQuadData[i];
                }
                var vertexQuadAccessor = CreateUShortAccessor("quadIndices", bufferView, 24, vertexCount);
                primitive.SetVertexAccessor("_QUAD_INDEX", vertexQuadAccessor);

                // Vertex attribute that associates each vertex with a particular quad.
                var vertexIndexData = quadList.SelectMany(x => x.Vertices.Select(y => (ushort) y.OriginalIndex)).ToArray();
                for (int i = 0; i < vertexCount; i++) {
                    var dataUShorts = MemoryMarshal.Cast<byte, ushort>(bufferViewData.AsSpan().Slice(i * stride + 28, 2));
                    dataUShorts[0] = vertexIndexData[i];
                }
                var vertexIndexAccessor = CreateUShortAccessor("originalIndices", bufferView, 28, vertexCount);
                primitive.SetVertexAccessor("_ORIGINAL_INDEX", vertexIndexAccessor);

                // Vertex attribute for texture coordinates.
                var texCoord0Data = quadList.SelectMany(x => x.Vertices.Select(y => y.TexCoord0)).ToArray();
                for (int i = 0; i < vertexCount; i++) {
                    var dataFloats = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride + 32, 8));
                    dataFloats[0] = texCoord0Data[i].Value.X;
                    dataFloats[1] = texCoord0Data[i].Value.Y;
                }
                var texCoord0Accessor = CreateVector2Accessor("texCoords0", bufferView, 32, vertexCount);
                primitive.SetVertexAccessor("TEXCOORD_0", texCoord0Accessor);

                // Build faces, breaking down quads into triangles.
                var faceIndexData = quadList
                    .SelectMany(x => {
                        var firstIndex = x.Vertices[0].Index;
                        return new ushort[] {
                            (ushort) (firstIndex + 0), (ushort) (firstIndex + 1), (ushort) (firstIndex + 2),
                            (ushort) (firstIndex + 2), (ushort) (firstIndex + 3), (ushort) (firstIndex + 0),
                        };
                    })
                    .ToArray()
                    .To2DArray(model.Faces.Count * 2, 3);

                var indexAccessor = CreateTriangeIndiciesAccessor("indices", faceIndexData);
                primitive.IndexAccessor = indexAccessor;

                // Make our model visible.
                scene.CreateNode("node").WithMesh(mesh);
            }

            // Write GLTF, ignoring errors (we don't care if they're broken).
            var settings = new WriteSettings {
                JsonIndented = true,
                JsonOptions = new JsonWriterOptions() { NewLine = "\n" },
                Validation = SharpGLTF.Validation.ValidationMode.Skip,
            };
            using (var stream = new MemoryStream()) {
                modelRoot.WriteGLB(stream, settings);
                return stream.ToArray();
            }
        }

        public ISGL_Model GLB_ToModel(byte[] glbFile, int? modelCollectionId, int? modelId, int? levelOfDetail) {
            // TODO: merge duplicate vertices based on extra data provided
            // TODO: we need to reassemble quads in a much better fashion!

            var modelRoot = ModelRoot.ParseGLB(new ArraySegment<byte>(glbFile));

            // Fetch vertices.
            var vertexAccessor = modelRoot.LogicalMeshes[0].Primitives[0].GetVertexAccessor("POSITION");
            var vertices = new Vector3[vertexAccessor.Count];
            vertexAccessor.AsVector3Array().CopyTo(vertices, 0);

            // Fetch vertex normals, if available.
            var vertexNormalAccessor = modelRoot.LogicalMeshes[0].Primitives[0].GetVertexAccessor("NORMAL");
            var vertexNormals = (vertexNormalAccessor == null) ? null : new Vector3[vertexNormalAccessor.Count];
            if (vertexNormalAccessor != null)
                vertexNormalAccessor.AsVector3Array().CopyTo(vertexNormals, 0);

            // Fetch indicies, converting triangles back into quads.
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

            // Build our model.
            return new SGL_Model(modelCollectionId ?? 0, modelId ?? 0, levelOfDetail ?? 0,
                vertices.Select(x => x.ToVECTOR().ToSwappedYZ()).ToArray(),
                quadIndices.Select(x => new SGL_ModelFace(x, new VECTOR(0, -1, 0), new ATTR())).ToArray(),
                vertexNormals.Select(x => x.ToVECTOR().ToSwappedYZ()).ToArray()
            );
        }
    }
}
