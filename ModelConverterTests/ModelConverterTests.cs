using CommonLib.Arrays;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Files.X8PC;
using SF3.NamedValues;
using SF3.Types;

namespace ModelConverter.Tests.Utils {
    [TestClass]
    public class ModelConverterTests {
        public static string c_synbiosPath = "C:/SF3/Scenario1/X8PC00A.BIN";
        public static string c_barrelPath  = "C:/SF3/Scenario1/X8PC795.BIN";

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

        private void TestModelConversion(ISGL_Model originalModel, ITextureMetaCollection? texMetaCollection)
            => TestModelConversion([originalModel], texMetaCollection);

        private void TestModelConversion(ISGL_Model[] originalModels, ITextureMetaCollection? texMetaCollection) {
            var converter = new ModelConverter();
            var glb = converter.ModelToGLB_Data(originalModels, texMetaCollection);
            var convertedModels = converter.GLB_DataToModels(glb, null, null, null);

            Assert.AreEqual(originalModels.Length, convertedModels.Length, "Not equal: convertedModels.Length");
            for (int modelIdx = 0; modelIdx < originalModels.Length; modelIdx++) {
                var convertedModel = convertedModels[modelIdx];
                var originalModel = originalModels[modelIdx];

                Assert.IsNotNull(convertedModel.Vertices, $"Not null: Vertices");
                Assert.AreEqual(originalModel.Vertices.Count, convertedModel.Vertices.Count, $"Not equal: Vertices.Count");
                for (int i = 0; i < originalModel.Vertices.Count; i++) {
                    Assert.IsNotNull(convertedModel.Vertices[i], $"Not null: Vertices[{i}]");

                    var originalVertex  = originalModel.Vertices[i];
                    var convertedVertex = convertedModel.Vertices[i];
                    Assert.AreEqual(originalVertex.X.Float, convertedVertex.X.Float, 0.001f, $"Not equal: Vertices[{i}].X");
                    Assert.AreEqual(originalVertex.Y.Float, convertedVertex.Y.Float, 0.001f, $"Not equal: Vertices[{i}].Y");
                    Assert.AreEqual(originalVertex.Z.Float, convertedVertex.Z.Float, 0.001f, $"Not equal: Vertices[{i}].Z");
                }

                Assert.IsNotNull(convertedModel.Faces, $"Not null: Faces");
                Assert.AreEqual(originalModel.Faces.Count, convertedModel.Faces.Count, $"Not equal: Faces.Count");
                for (int i = 0; i < originalModel.Faces.Count; i++) {
                    Assert.IsNotNull(convertedModel.Faces[i], $"Not null: Faces[{i}]");

                    var originalFace  = originalModel.Faces[i];
                    var convertedFace = convertedModel.Faces[i];
                    Assert.IsNotNull(convertedFace.VertexIndices, $"Not null: Faces[{i}].VertexIndices");
                    Assert.AreEqual(4, convertedFace.VertexIndices.Count, $"Not equal: Faces[{i}].VertexIndices.Count");
                    for (int j = 0; j < 4; j++)
                        Assert.AreEqual(originalFace.VertexIndices[j], convertedFace.VertexIndices[j], $"Not equal: Faces[{i}].VertexIndices[{j}]");

                    // TODO: Check remaining ATTR properties
                    var originalAttr  = originalFace.Attributes;
                    var convertedAttr = convertedFace.Attributes;
                    Assert.AreEqual(originalAttr.ColorNo,    convertedAttr.ColorNo,    $"Not equal: Faces[{i}].Attributes.ColorNo");
                    Assert.AreEqual(originalAttr.IsTwoSided, convertedAttr.IsTwoSided, $"Not equal: Faces[{i}].Attributes.IsTwoSided");
                    Assert.AreEqual(originalAttr.UseTexture, convertedAttr.UseTexture, $"Not equal: Faces[{i}].Attributes.UseTexture");
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

        [TestMethod]
        public void ExportThenImport_WithCube_ProducesOriginal() {
            var originalModel = new SGL_Model(0, 0, 0, c_cubeVertices, c_cubePolys, c_cubeVertexNormals);
            TestModelConversion(originalModel, null);
        }

        [TestMethod]
        public void ExportThenImport_WithBarrelFromX8PC_ProducesOriginal() {
            var barrelData = File.ReadAllBytes(c_barrelPath);
            var x8pcFile = X8PC_File.Create(new ByteData(new ByteArray(barrelData)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);
            var polyChar = x8pcFile.PolyCharTable[0];
            TestModelConversion(polyChar.GetModel(0, 0), polyChar);
        }

        [TestMethod]
        public void ExportThenImport_WithSynbiosPart1FromX8PC_ProducesOriginal() {
            var barrelData = File.ReadAllBytes(c_synbiosPath);
            var x8pcFile = X8PC_File.Create(new ByteData(new ByteArray(barrelData)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);
            var polyChar = x8pcFile.PolyCharTable[0];
            TestModelConversion(polyChar.GetModel(0, 0), polyChar);
        }

        [TestMethod]
        public void ExportThenImport_WithSynbiosPolyChar_ProducesOriginal() {
            var barrelData = File.ReadAllBytes(c_synbiosPath);
            var x8pcFile = X8PC_File.Create(new ByteData(new ByteArray(barrelData)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);
            var polyChar = x8pcFile.PolyCharTable[0];
            TestModelConversion(polyChar.ToArray(), polyChar);
        }
    }
}