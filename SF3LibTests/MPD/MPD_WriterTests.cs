using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.MPD {
    [TestClass]
    public class MPD_WriterTests {
        private static MPD_File MakeFile(ScenarioType scenario, string filename) {
            var filePath = TestDataPaths.ResourcePath(scenario, filename)!;
            var fileData = File.ReadAllBytes(filePath);
            return MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(fileData)), new NameGetterContext(scenario), scenario);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1TestMap_CanBeLoaded() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "TESMAP.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(originalFile);
                outputData = memoryStream.ToArray();
            }

            var newFile = MPD_File.Create(
                new SF3.ByteData.ByteData(new ByteArray(outputData)),
                originalFile.NameGetterContext,
                originalFile.Scenario
            );
        }

        [TestMethod]
        public void WriteMPD_WithScenario1TestMap_ProducesSameData() {
            var file = MakeFile(ScenarioType.Scenario1, "TESMAP.MPD");
            var fileData = file.Data.GetDataCopyOrReference();

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("TESMAP_Test.MPD", outputData);

            AssertByteComparison(fileData, outputData);
        }

        private void AssertByteComparison(byte[] fileData, byte[] outputData) {
            var errors = ByteComparisonErrors(fileData, outputData) ?? [];
            if (errors.Count > 0)
                Assert.Fail(string.Join("\r\n", errors));
        }

        private List<string> ByteComparisonErrors(byte[] expected, byte[] actual) {
            var errors = new List<string>();

            if (expected.Length != actual.Length)
                errors.Add($"Length is wrong: should be {expected.Length} (0x{expected.Length:X5}), is {actual.Length} (0x{actual.Length:X5})");
            uint? firstWrongByte = null;
            int wrongBytes = 0;
            int bytesToCompare = Math.Min(expected.Length, actual.Length);

            for (uint i = 0; i < bytesToCompare; i++) {
                if (expected[i] != actual[i]) {
                    if (!firstWrongByte.HasValue)
                        firstWrongByte = i;
                    wrongBytes++;
                }
            }

            if (wrongBytes > 0) {
                var percent = (float) (bytesToCompare - wrongBytes) / bytesToCompare * 100.0f;
                errors.Add($"Comparable data is wrong: {percent:0.00}% accurate");

                var rightByte = expected[firstWrongByte!.Value];
                var wrongByte = actual[firstWrongByte!.Value];
                errors.Add($"First wrong byte is at {firstWrongByte!.Value} (0x{firstWrongByte:X4}):");
                errors.Add($"  Should be {rightByte} (0x{rightByte:X2}), is {wrongByte} (0x{wrongByte:X2})");
            }

            return errors;
        }
    }
}
