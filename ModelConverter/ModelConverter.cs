using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.SGL;
using CommonLib.ThirdParty.TexturePacker;
using SharpGLTF.Materials;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;

namespace ModelConverter {
    public class ModelConverter {
        private readonly Vector4 c_black = new Vector4(0, 0, 0, 0);

        private struct ConvertedVertex {
            public ConvertedVertex(int primIndex, int originalIndex, int quadIndex, int indexInQuad, Vector3 position, Vector3? normal, Vector2? texCoord0, Vector4 color0) {
                PrimitiveIndex = primIndex;
                OriginalIndex  = originalIndex;
                QuadIndex      = quadIndex;
                IndexInQuad    = indexInQuad;
                Position       = position;
                Normal         = normal;
                TexCoord0      = texCoord0;
                Color0         = color0;
            }

            public override string ToString() => $"{{ Idx: {PrimitiveIndex}, OrigIdx: {OriginalIndex}: QuadIdx: {QuadIndex}.{IndexInQuad}, Pos: {Position}, Normal: {Normal}, TexCoord: {TexCoord0}, Color: {Color0} }}";

            public readonly int PrimitiveIndex;
            public readonly int OriginalIndex;
            public readonly int QuadIndex;
            public readonly int IndexInQuad;
            public readonly Vector3 Position;
            public readonly Vector3? Normal;
            public readonly Vector2? TexCoord0;
            public readonly Vector4 Color0;
        }

        private struct Quad {
            public Quad(int originalIndex, ConvertedVertex[] vertices, IATTR attr) {
                OriginalIndex = originalIndex;
                Vertices      = vertices;
                ATTR          = attr;
            }

            public override string ToString() => $"{{ OrigIdx: {OriginalIndex}, Vertices: [{Vertices[0].OriginalIndex}, {Vertices[1].OriginalIndex}, {Vertices[2].OriginalIndex}, {Vertices[3].OriginalIndex}] }}";

            public readonly int OriginalIndex;
            public readonly ConvertedVertex[] Vertices;
            public readonly IATTR ATTR;
        }

        private struct AttrKey {
            public AttrKey(IATTR attr) {
                HasTextures = attr.UseTexture;
                IsTwoSided  = attr.IsTwoSided;
                UseLight    = attr.UseLight;
                HFlip       = attr.HFlip;
                VFlip       = attr.VFlip;

                Key = (HasTextures ? 0x01 : 0)
                    | (IsTwoSided  ? 0x02 : 0)
                    | (UseLight    ? 0x04 : 0)
                    | (HFlip       ? 0x08 : 0)
                    | (VFlip       ? 0x10 : 0);
            }

            public override int GetHashCode() => Key;

            public override bool Equals(object obj)
                => (obj is AttrKey other) ? Key == other.Key : base.Equals(obj);

            public readonly bool HasTextures;
            public readonly bool IsTwoSided;
            public readonly bool UseLight;
            public readonly bool HFlip;
            public readonly bool VFlip;

            public readonly int Key;
        }

        public byte[] ModelToGLB_Data(ISGL_Model[] sglModels, ITextureMetaCollection texMetaCollection) {
            var modelRoot = ModelToGLTF_ModelRoot(sglModels, texMetaCollection);

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

        public ModelRoot ModelToGLTF_ModelRoot(ISGL_Model[] sglModels, ITextureMetaCollection texMetaCollection) {
            // TODO: four-way split for crazy quads

            var modelRoot = ModelRoot.CreateModel();

            // Default scene.
            var scene = modelRoot.UseScene("scene");

            // Get all textures for applicable ModelCollectionID's.
            var mcIds = sglModels.Select(x => x.ModelCollectionID).Distinct().OrderBy(x => x).ToArray();
            var texturesByMcId = (texMetaCollection != null)
                ? mcIds.ToDictionary(x => x, x => texMetaCollection.GetAnimatableTexturesByModelCollectionID(x))
                : new Dictionary<int, Dictionary<int, IAnimatableTexture>>();

            foreach (var sglModel in sglModels) {
                var mesh = modelRoot.CreateMesh();

                var facesByAttr = sglModel.Faces
                    .Select((x, i) => (Face: x, Index: i, AttrKey: new AttrKey(x.Attributes)))
                    .GroupBy(x => x.AttrKey)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                foreach (var attrFaces in facesByAttr) {
                    var attrKey = attrFaces.Key;
                    var faces = attrFaces.Value;

                    var primitive = mesh.CreatePrimitive();

                    // Build a texture atlas for this model.
                    TextureAtlas textureAtlas = null;
                    Rectangle textureAtlasDimensions;

                    var materialBuilder = new MaterialBuilder("material");
                    if (attrKey.HasTextures) {
                        var textureIds = faces.Select(x => x.Face.Attributes).Where(x => x.UseTexture).Select(x => x.TextureNo).Distinct().OrderBy(x => x).ToArray();
                        var texturesForMcId = texturesByMcId[sglModel.ModelCollectionID];
                        var textures = textureIds.Where(x => texturesForMcId.ContainsKey(x)).Select(x => texturesForMcId[x]).ToArray();
                        textureAtlas = new TextureAtlas(textures, tryRotate: false);
                        textureAtlasDimensions = textureAtlas.GetDimensions(onlyTextures: true, forceEvenWidth: true);

                        if (textures.Length > 0) {
                            byte[] textureAtlasBitmapContent;
                            using (var textureAtlasBitmap = textureAtlas.CreateBitmap(onlyTextures: true, forceEvenWidth: true)) {
                                using (var bitmapStream = new MemoryStream()) {
                                    textureAtlasBitmap.Save(bitmapStream, ImageFormat.Png);
                                    textureAtlasBitmapContent = bitmapStream.ToArray();
                                }
                            }

                            // Add the texture.
                            var textureAtlasImageContent = new MemoryImage(textureAtlasBitmapContent);
                            var textureAtlasImage = ImageBuilder.From(textureAtlasImageContent);

                            materialBuilder.UseChannel(KnownChannel.BaseColor)
                                .UseTexture()
                                .WithPrimaryImage(textureAtlasImage)
                                .WithSampler(TextureWrapMode.CLAMP_TO_EDGE, TextureWrapMode.CLAMP_TO_EDGE, mag: TextureInterpolationFilter.NEAREST);
                        }
                    }

                    // Build additional properties.
                    materialBuilder = materialBuilder.WithDoubleSide(attrKey.IsTwoSided);
                    if (!attrKey.UseLight)
                        materialBuilder = materialBuilder.WithUnlitShader();
                    materialBuilder.Extras = new JsonObject() {
                        ["hFlip"] = attrKey.HFlip,
                        ["vFlip"] = attrKey.VFlip,
                    };

                    var material = modelRoot.CreateMaterial(materialBuilder);
                    primitive.Material = material;

                    // Build quads, each with its own vertices. We're not going to have *ANY* shared vertices because we
                    // *must* store unique ATTR data per-polygon. (We can at least share them between triangles)
                    // Swap Y/Z coordinates to match the standard coordinate system.
                    var quadList = new List<Quad>();

                    Vector3? GetVertexNormal(int idx)
                        => (sglModel.VertexNormals != null) ? sglModel.VertexNormals[idx].ToNumericsVector3().ToSwappedYZ() : (Vector3?) null;

                    Vector2 GetTexCoord0(ISGL_ModelFace face, int idx) {
                        var attr = face.Attributes;
                        var idxX = ((idx + 1) / 2) % 2;
                        var idxY = ((idx + 0) / 2) % 2;

                        if (attrKey.HasTextures) {
                            var node = textureAtlas.GetNodeByTextureIDFrame(attr.TextureNo, 0);
                            var nodeRect = node.Rect;

                            // Also flip UV coordinates so the face appears correct when importing.
                            // (They'll need to be flipped back on re-import.)
                            if (attr.HFlip)
                                idxX = 1 - idxX;
                            if (attr.VFlip)
                                idxY = 1 - idxY;

                            return new Vector2(
                                (nodeRect.Left + idxX * nodeRect.Width)  / (float) textureAtlasDimensions.Width,
                                (nodeRect.Top  + idxY * nodeRect.Height) / (float) textureAtlasDimensions.Height
                            );
                        }
                        else
                            return new Vector2(idxX, idxY);
                    }

                    Vector4 GetColor0(ISGL_ModelFace face) {
                        var attr = face.Attributes;
                        if (attr.UseTexture)
                            return new Vector4(1, 1, 1, 1);
                        else {
                            var channels = PixelConversion.ABGR1555toChannels(attr.ColorNo);
                            return new Vector4(channels.R / 255.0f, channels.G / 255.0f, channels.B / 255.0f, channels.A / 255.0f);
                        }
                    }

                    // Build quads for this primitive.
                    int quadFirstVertIdx = 0;
                    foreach (var face in faces) {
                        var color = GetColor0(face.Face);
                        var faceVertices = face.Face.VertexIndices
                            .Select((modelVertIdx, faceVertIdx) => {
                                return new ConvertedVertex(
                                    quadFirstVertIdx + faceVertIdx,
                                    modelVertIdx,
                                    face.Index,
                                    faceVertIdx,
                                    sglModel.Vertices[modelVertIdx].ToNumericsVector3().ToSwappedYZ(),
                                    GetVertexNormal(modelVertIdx),
                                    GetTexCoord0(face.Face, faceVertIdx),
                                    color
                                );
                            })
                            .ToArray();

                        quadFirstVertIdx += 4;
                        quadList.Add(new Quad(face.Index, faceVertices, face.Face.Attributes));
                    }

                    // Create a big buffer for the entire model.
                    var vertexCount = quadList.Count * 4;
                    var stride = (12 /*pos*/ + 12 /*normal*/ + 8/*quadIdx*/ + 2/*normalIdx*/ + 2/*padding*/ + 8/*texcoord_0*/ + 16/*color_0*/);
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
                    var vertexAccessor = modelRoot.CreateVector3Accessor("vertices", bufferView, 0, vertexCount);
                    primitive.SetVertexAccessor("POSITION", vertexAccessor);

                    // Vertex attribute for normals, if available.
                    if (sglModel.VertexNormals != null) {
                        var vertexNormalData = quadList.SelectMany(x => x.Vertices.Select(y => y.Normal.Value)).ToArray();
                        for (int i = 0; i < vertexCount; i++) {
                            var dataFloats = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride + 12, 12));
                            dataFloats[0] = vertexNormalData[i].X;
                            dataFloats[1] = vertexNormalData[i].Y;
                            dataFloats[2] = vertexNormalData[i].Z;
                        }
                        var vertexNormalAccessor = modelRoot.CreateVector3Accessor("vertexNormals", bufferView, 12, vertexCount);
                        primitive.SetVertexAccessor("NORMAL", vertexNormalAccessor);
                    }

                    // Vertex attribute that associates each vertex with a particular quad.
                    var vertexQuadData = quadList.SelectMany(x => x.Vertices.Select(y => new Vector2(y.QuadIndex, y.IndexInQuad))).ToArray();
                    for (int i = 0; i < vertexCount; i++) {
                        var dataVec2 = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride + 24, 8));
                        dataVec2[0] = vertexQuadData[i].X;
                        dataVec2[1] = vertexQuadData[i].Y;
                    }
                    var vertexQuadAccessor = modelRoot.CreateVector2Accessor("quadIndices", bufferView, 24, vertexCount);
                    primitive.SetVertexAccessor("_QUAD_INDEX", vertexQuadAccessor);

                    // Vertex attribute that associates each vertex with a particular quad.
                    var vertexIndexData = quadList.SelectMany(x => x.Vertices.Select(y => (ushort) y.OriginalIndex)).ToArray();
                    for (int i = 0; i < vertexCount; i++) {
                        var dataUShorts = MemoryMarshal.Cast<byte, ushort>(bufferViewData.AsSpan().Slice(i * stride + 32, 2));
                        dataUShorts[0] = vertexIndexData[i];
                    }
                    var vertexIndexAccessor = modelRoot.CreateUShortAccessor("originalIndices", bufferView, 32, vertexCount);
                    primitive.SetVertexAccessor("_ORIGINAL_INDEX", vertexIndexAccessor);

                    // Vertex attribute for texture coordinates.
                    var texCoord0Data = quadList.SelectMany(x => x.Vertices.Select(y => y.TexCoord0)).ToArray();
                    for (int i = 0; i < vertexCount; i++) {
                        var dataVec2 = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride + 36, 8));
                        dataVec2[0] = texCoord0Data[i].Value.X;
                        dataVec2[1] = texCoord0Data[i].Value.Y;
                    }
                    var texCoord0Accessor = modelRoot.CreateVector2Accessor("texCoords0", bufferView, 36, vertexCount);
                    primitive.SetVertexAccessor("TEXCOORD_0", texCoord0Accessor);

                    // Vertex attribute for polygon colors.
                    var texColor0Data = quadList.SelectMany(x => x.Vertices.Select(y => y.Color0)).ToArray();
                    for (int i = 0; i < vertexCount; i++) {
                        var dataVec4 = MemoryMarshal.Cast<byte, float>(bufferViewData.AsSpan().Slice(i * stride + 44, 16));
                        dataVec4[0] = texColor0Data[i].X;
                        dataVec4[1] = texColor0Data[i].Y;
                        dataVec4[2] = texColor0Data[i].Z;
                        dataVec4[3] = texColor0Data[i].W;
                    }
                    var texColor0Accessor = modelRoot.CreateVector4Accessor("color0", bufferView, 44, vertexCount);
                    primitive.SetVertexAccessor("COLOR_0", texColor0Accessor);

                    // Build faces, breaking down quads into triangles.
                    var faceIndexData = quadList
                        .SelectMany(x => {
                            var firstIndex = x.Vertices[0].PrimitiveIndex;
                            return new ushort[] {
                                (ushort) (firstIndex + 0), (ushort) (firstIndex + 1), (ushort) (firstIndex + 2),
                                (ushort) (firstIndex + 2), (ushort) (firstIndex + 3), (ushort) (firstIndex + 0),
                            };
                        })
                        .ToArray()
                        .To2DArray(faces.Length * 2, 3);

                    var indexAccessor = modelRoot.CreateTriangeIndiciesAccessor("indices", faceIndexData);
                    primitive.IndexAccessor = indexAccessor;
                }

                // Make our model visible.
                scene.CreateNode("node").WithMesh(mesh);
            }

            return modelRoot;
        }

        public SGL_Model[] GLB_DataToModels(byte[] glbFile, int? modelCollectionId, int? modelId, int? levelOfDetail) {
            var modelRoot = ModelRoot.ParseGLB(new ArraySegment<byte>(glbFile));
            return GLTF_ModelRootToModels(modelRoot, modelCollectionId, modelId, levelOfDetail);
        }

        public SGL_Model[] GLTF_ModelRootToModels(ModelRoot modelRoot, int? modelCollectionId, int? modelId, int? levelOfDetail) {
            // TODO: (maybe not?) somehow integrate triangle indices back into quad generation.
            // TODO: swap UV coordinates based on HFlip and VFlip

            var sglModels = new List<SGL_Model>();
            foreach (var mesh in modelRoot.LogicalMeshes) {
                var meshVertices = new List<ConvertedVertex>();
                var meshQuads    = new List<Quad>();

                foreach (var primitive in mesh.Primitives) {
                    var material = primitive.Material;
                    var materialTexture = material.GetDiffuseTexture();

                    // Fetch vertices.
                    var vertexAccessor = primitive.GetVertexAccessor("POSITION");
                    var vertexPositions = new Vector3[vertexAccessor.Count];
                    vertexAccessor.AsVector3Array().CopyTo(vertexPositions, 0);

                    // Fetch vertex normals, if available.
                    var vertexNormalAccessor = primitive.GetVertexAccessor("NORMAL");
                    var vertexNormals = (vertexNormalAccessor == null) ? null : new Vector3[vertexNormalAccessor.Count];
                    if (vertexNormalAccessor != null)
                        vertexNormalAccessor.AsVector3Array().CopyTo(vertexNormals, 0);

                    // Fetch original vertex IDs for each vertex. This is part of mesh reconstruction.
                    var vertexOrigIndicesAccessor = primitive.GetVertexAccessor("_ORIGINAL_INDEX");
                    var vertexOrigIndices = vertexOrigIndicesAccessor.AsScalarArray();

                    // Fetch the quads that each vertex belong to. This is part of quad reconstruction.
                    var vertexQuadIndicesAccessor = primitive.GetVertexAccessor("_QUAD_INDEX");
                    var vertexQuadIndices = vertexQuadIndicesAccessor.AsVector2Array();
                    var quadVertexMap = vertexQuadIndices
                        .Select((x, i) => (QuadID: (ushort) x.X, IndexInQuad: (ushort) x.Y, ExportedVertexID: (ushort) i))
                        .OrderBy(x => x.IndexInQuad)
                        .GroupBy(x => x.QuadID)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.ExportedVertexID).ToArray());

                    // Reconstruct the transitionary 'ConvertedVertex' classes
                    var primVertices = Enumerable
                        .Range(0, vertexAccessor.Count)
                        .Select((x, i) => new ConvertedVertex(
                            i,
                            (int) Math.Round(vertexOrigIndices[x]),
                            (int) Math.Round(vertexQuadIndices[x].X),
                            (int) Math.Round(vertexQuadIndices[x].Y),
                            vertexPositions[x],
                            vertexNormals[x],
                            null, // TODO: texCoord0
                            new Vector4(1, 1, 1, 1)
                        ))
                        .ToArray();

                    // Keep track of vertices for the entire mesh.
                    meshVertices.AddRange(primVertices);

                    // Fetch colors.
                    var color0Accessor = primitive.GetVertexAccessor("COLOR_0");
                    var vertexColors = new Vector4[color0Accessor.Count];
                    color0Accessor.AsVector4Array().CopyTo(vertexColors, 0);

                    var primQuads = quadVertexMap
                        .Select(x => {
                            var colorVec4 = (materialTexture == null) ? vertexColors[x.Value[0]] : c_black;
                            var colorChannels = new PixelChannels() {
                                R = (byte) (colorVec4.X * 255),
                                G = (byte) (colorVec4.Y * 255),
                                B = (byte) (colorVec4.Z * 255),
                                A = (byte) (colorVec4.W * 255),
                            };
                            return new Quad(
                                x.Key,
                                x.Value.Select(y => primVertices[y]).ToArray(),
                                new ATTR() {
                                    ColorNo    = colorChannels.ToABGR1555(),
                                    IsTwoSided = material.DoubleSided,
                                    UseTexture = (materialTexture != null),
                                    UseLight   = !material.Unlit,
                                    HFlip      = (bool) material.Extras["hFlip"],
                                    VFlip      = (bool) material.Extras["vFlip"],
                                }
                            );
                        })
                        .ToArray();

                    // Keep track of quads for the entire mesh.
                    meshQuads.AddRange(primQuads);
                }

                // Sort vertices and quads by their original ID.
                meshVertices = meshVertices.OrderBy(x => x.OriginalIndex).GroupBy(x => x.OriginalIndex).Select(x => x.First()).ToList();
                meshQuads = meshQuads.OrderBy(x => x.OriginalIndex).GroupBy(x => x.OriginalIndex).Select(x => x.First()).ToList();

                // Build our SGL_Model.
                var newSglModel = new SGL_Model(modelCollectionId ?? 0, modelId ?? 0, levelOfDetail ?? 0,
                    meshVertices.Select(x => x.Position.ToVECTOR().ToSwappedYZ()).ToArray(),
                    meshQuads.Select(x => new SGL_ModelFace(x.Vertices.Select(y => y.OriginalIndex).ToArray(), new VECTOR(0, -1, 0), new ATTR() {
                        ColorNo    = x.ATTR.ColorNo,
                        IsTwoSided = x.ATTR.IsTwoSided,
                        UseTexture = x.ATTR.UseTexture,
                        UseLight   = x.ATTR.UseLight,
                        HFlip      = x.ATTR.HFlip,
                        VFlip      = x.ATTR.VFlip,
                    })).ToArray(),
                    meshVertices.Select(x => x.Normal.Value.ToVECTOR().ToSwappedYZ()).ToArray()
                );

                sglModels.Add(newSglModel);
            }

            return sglModels.ToArray();
        }
    }
}
