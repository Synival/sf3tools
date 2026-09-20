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

        private class FaceWithIndex {
            public ISGL_ModelFace Face;
            public int Index;
        }

        private class MaterialWithFaces {
            public Material Material;
            public Dictionary<ISGL_Model, FaceWithIndex[]> FacesByModel;
        }

        private class ModelCollectionWithAtlas {
            public TextureAtlas TextureAtlas;
            public Rectangle TextureAtlasDimensions;
            internal MemoryImage TextureAtlasMemoryImage;
            public Dictionary<AttrKey, MaterialWithFaces> ModelsByAttr;
        }

        public ModelRoot ModelToGLTF_ModelRoot(ISGL_Model[] sglModels, ITextureMetaCollection texMetaCollection) {
            // TODO: four-way split for crazy quads
            // TODO: name stuff better (e.g, "Material001" instead of "material")

            var modelRoot = ModelRoot.CreateModel();

            // Default scene.
            var scene = modelRoot.UseScene("scene");

            var mcIds = sglModels.Select(x => x.ModelCollectionID).Distinct().OrderBy(x => x).ToArray();
            var allFaces = sglModels.SelectMany(x => x.Faces.Select((y, i) => (Model: x, Face: new FaceWithIndex() { Face = y, Index = i }))).ToArray();
            var facesByMcIdThenAttrKeyThenModel = allFaces
                .OrderBy(x => x.Model.ModelCollectionID)
                .GroupBy(x => x.Model.ModelCollectionID)
                .ToDictionary(x => x.Key, x => new ModelCollectionWithAtlas() { 
                    ModelsByAttr = x
                        .Select(y => (AttrKey: new AttrKey(y.Face.Face.Attributes), Model: y.Model, Face: y.Face))
                        .OrderBy(y => y.AttrKey.Key)
                        .GroupBy(y => y.AttrKey)
                        .ToDictionary(y => y.Key, y => new MaterialWithFaces() {
                            Material = null,
                            FacesByModel = y
                                .OrderBy(z => z.Model.ModelID)
                                .GroupBy(z => z.Model)
                                .ToDictionary(z => z.Key, z => z.Select(a => a.Face).ToArray())
                        })
                    }
                );

            // Generate the texture atlas and materials for each model collection.
            foreach (var mcId in mcIds) {
                var mc = facesByMcIdThenAttrKeyThenModel[mcId];

                // Start by generating the atlas.
                var texturesById = (texMetaCollection != null)
                    ? texMetaCollection.GetAnimatableTexturesByModelCollectionID(mcId)
                    : new Dictionary<int, IAnimatableTexture>();
                var mcTextures = texturesById.Values.ToArray();

                mc.TextureAtlas = new TextureAtlas(mcTextures, tryRotate: false);
                mc.TextureAtlasDimensions = mc.TextureAtlas.GetDimensions(onlyTextures: true, forceEvenWidth: true);

                // If the atlas has images, create a shared 'MemoryImage' for it.
                if (mcTextures.Length > 0) {
                    byte[] textureAtlasBitmapContent;
                    using (var textureAtlasBitmap = mc.TextureAtlas.CreateBitmap(onlyTextures: true, forceEvenWidth: true)) {
                        using (var bitmapStream = new MemoryStream()) {
                            textureAtlasBitmap.Save(bitmapStream, ImageFormat.Png);
                            textureAtlasBitmapContent = bitmapStream.ToArray();
                        }
                    }

                    // Add the texture.
                    mc.TextureAtlasMemoryImage = new MemoryImage(textureAtlasBitmapContent);
                }

                // Create unique materials.
                foreach (var attrFaces in facesByMcIdThenAttrKeyThenModel[mcId].ModelsByAttr) {
                    AttrKey attrKey = attrFaces.Key;
                    var faces = attrFaces.Value.FacesByModel.SelectMany(x => x.Value).ToArray();

                    var materialBuilder = new MaterialBuilder("material");

                    // If this AttrKey has textures, apply them.
                    if (attrKey.HasTextures) {
                        var textureIds = faces.Select(x => x.Face.Attributes).Where(x => x.UseTexture).Select(x => x.TextureNo).Distinct().OrderBy(x => x).ToArray();
                        var textures = textureIds.Where(x => texturesById.ContainsKey(x)).Select(x => texturesById[x]).ToArray();

                        if (textures.Length > 0) {
                            // Add the texture.
                            var textureAtlasImage = ImageBuilder.From(mc.TextureAtlasMemoryImage);
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

                    attrFaces.Value.Material = modelRoot.CreateMaterial(materialBuilder);
                }
            }

            // Build all the meshes.
            foreach (var sglModel in sglModels) {
                var mesh = modelRoot.CreateMesh();

                // All the faces to create for this model are already nested in the 'facesByMcIdThenAttrKeyThenModel' dictionary.
                foreach (var attrFaces in facesByMcIdThenAttrKeyThenModel[sglModel.ModelCollectionID].ModelsByAttr) {
                    if (!attrFaces.Value.FacesByModel.ContainsKey(sglModel))
                        continue;

                    var attrKey                = attrFaces.Key;
                    var mc                     = facesByMcIdThenAttrKeyThenModel[sglModel.ModelCollectionID];
                    var textureAtlas           = mc.TextureAtlas;
                    var textureAtlasDimensions = mc.TextureAtlasDimensions;
                    var faces                  = attrFaces.Value.FacesByModel[sglModel];

                    var primitive = mesh.CreatePrimitive();
                    primitive.Material = attrFaces.Value.Material;

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
                        else {
                            // Dummy UV coordinates.
                            return new Vector2(0, 0);
                        }
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
