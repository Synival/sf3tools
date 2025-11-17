using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.MPD {
    [TestClass]
    public class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario1VoidFile_ProducesSameData() {
            var filePath = TestDataPaths.ResourcePath(ScenarioType.Scenario1, "VOID.MPD")!;
            var fileData = File.ReadAllBytes(filePath);
            var file = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(fileData)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);

            byte[] outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.Write(file);
                outputData = memoryStream.ToArray();
            }

            Assert.AreEqual(fileData.Length, outputData.Length);
        }
    }
}
