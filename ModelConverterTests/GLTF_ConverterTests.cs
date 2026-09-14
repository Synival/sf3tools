using CommonLib.SGL;

namespace GLTF_Converter.Tests.Utils {
    [TestClass]
    public class GLTF_ConverterTests {
        public static readonly VECTOR[] c_cubeVertices = [
            new VECTOR(-1, -1,  1), // 0: Top-back-left
            new VECTOR( 1, -1,  1), // 1: Top-back-right
            new VECTOR( 1, -1, -1), // 2: Top-front-right
            new VECTOR(-1, -1, -1), // 3: Top-front-left

            new VECTOR(-1,  1,  1), // 4: Bottom-back-left
            new VECTOR( 1,  1,  1), // 5: Bottom-back-right
            new VECTOR( 1,  1, -1), // 6: Bottom-front-right
            new VECTOR(-1,  1, -1), // 7: Bottom-front-left
        ];

        public static readonly SGL_ModelFace[] c_cubePolys = [
            new SGL_ModelFace([0, 1, 2, 3], new VECTOR( 0, -1,  0), new ATTR()),
            new SGL_ModelFace([7, 6, 5, 4], new VECTOR( 0,  1,  0), new ATTR()),

            new SGL_ModelFace([0, 3, 7, 4], new VECTOR(-1,  0,  0), new ATTR()),
            new SGL_ModelFace([2, 1, 5, 6], new VECTOR( 1,  0,  0), new ATTR()),

            new SGL_ModelFace([3, 2, 6, 7], new VECTOR( 0,  0, -1), new ATTR()),
            new SGL_ModelFace([1, 0, 4, 5], new VECTOR( 0,  0,  1), new ATTR()),
        ];

        public static readonly VECTOR[] c_cubeVertexNormals = [
            new VECTOR(-0.577f, -0.577f,  0.577f),
            new VECTOR( 0.577f, -0.577f,  0.577f),
            new VECTOR( 0.577f, -0.577f, -0.577f),
            new VECTOR(-0.577f, -0.577f, -0.577f),

            new VECTOR(-0.577f,  0.577f,  0.577f),
            new VECTOR( 0.577f,  0.577f,  0.577f),
            new VECTOR( 0.577f,  0.577f, -0.577f),
            new VECTOR(-0.577f,  0.577f, -0.577f),
        ];

        [TestMethod]
        public void CompressThenDecompress_WithCube_ProducesOriginal() {
            var originalModel = new SGL_Model(0, 0, 0, c_cubeVertices, c_cubePolys, c_cubeVertexNormals);

            var converter = new GLTF_Converter();
            var gltf = converter.ModelToGLTF(originalModel);
            var convertedModel = converter.GLTF_ToModel(gltf, originalModel.ModelCollectionID, originalModel.ModelID, originalModel.LevelOfDetail);

            Assert.IsNotNull(convertedModel.Vertices, $"Not null: Vertices");
            Assert.AreEqual(originalModel.Vertices.Count, convertedModel.Vertices.Count, $"Not equal: Vertices.Count");
            for (int i = 0; i < originalModel.Vertices.Count; i++) {
                Assert.IsNotNull(convertedModel.Vertices[i], $"Not null: Vertices[{i}]");
                Assert.AreEqual(originalModel.Vertices[i].X.Float, convertedModel.Vertices[i].X.Float, 0.001f, $"Not equal: Vertices[{i}].X");
                Assert.AreEqual(originalModel.Vertices[i].Y.Float, convertedModel.Vertices[i].Y.Float, 0.001f, $"Not equal: Vertices[{i}].Y");
                Assert.AreEqual(originalModel.Vertices[i].Z.Float, convertedModel.Vertices[i].Z.Float, 0.001f, $"Not equal: Vertices[{i}].Z");
            }

            Assert.IsNotNull(convertedModel.Faces, $"Not null: Faces");
            Assert.AreEqual(originalModel.Faces.Count, convertedModel.Faces.Count, $"Not equal: Faces.Count");
            for (int i = 0; i < originalModel.Faces.Count; i++) {
                Assert.IsNotNull(convertedModel.Faces[i], $"Not null: Faces[{i}]");
                Assert.IsNotNull(convertedModel.Faces[i].VertexIndices, $"Not null: Faces[{i}].VertexIndices");
                Assert.AreEqual(4, convertedModel.Faces[i].VertexIndices.Count, $"Not equal: Faces[{i}].VertexIndices.Count");
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(originalModel.Faces[i].VertexIndices[j], convertedModel.Faces[i].VertexIndices[j], $"Not equal: Faces[{i}].VertexIndices[{j}]");

                // TODO: Check ATTRs
            }

            Assert.IsNotNull(convertedModel.VertexNormals, $"Not null: VertexNormals");
            Assert.AreEqual(originalModel.VertexNormals.Count, convertedModel.VertexNormals.Count, $"Not equal: Vertices.Count");
            for (int i = 0; i < originalModel.VertexNormals.Count; i++) {
                Assert.IsNotNull(convertedModel.VertexNormals[i], $"Not null: VertexNormals[{i}]");
                Assert.AreEqual(originalModel.VertexNormals[i].X.Float, convertedModel.VertexNormals[i].X.Float, 0.001f, $"Not equal: VertexNormals[{i}].X");
                Assert.AreEqual(originalModel.VertexNormals[i].Y.Float, convertedModel.VertexNormals[i].Y.Float, 0.001f, $"Not equal: VertexNormals[{i}].Y");
                Assert.AreEqual(originalModel.VertexNormals[i].Z.Float, convertedModel.VertexNormals[i].Z.Float, 0.001f, $"Not equal: VertexNormals[{i}].Z");
            }

        }
    }
}