using SF3.Types;
using static SF3.Utils.FileUtils;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests.Utils {
    [TestClass]
    public class FileUtilsTests {
        private static readonly Dictionary<string, SF3FileType> c_supportedFileTypes = new() {
            { "X1FOO.BIN",   SF3FileType.X1 },
            { "X1BTL99.BIN", SF3FileType.X1BTL99 },
            { "X002.BIN",    SF3FileType.X002 },
            { "X005.BIN",    SF3FileType.X005 },
            { "X012.BIN",    SF3FileType.X012 },
            { "X013.BIN",    SF3FileType.X013 },
            { "X014.BIN",    SF3FileType.X014 },
            { "X019.BIN",    SF3FileType.X019 },
            { "X031.BIN",    SF3FileType.X031 },
            { "X033.BIN",    SF3FileType.X033 },
            { "X044.BIN",    SF3FileType.X044 },
            { "FOO.MPD",     SF3FileType.MPD },
        };

        private static readonly Dictionary<string, SF3FileType> c_supportedFileTypesByFilter = new() {
            { "*X1*.BIN",      SF3FileType.X1 },
            { "*X1BTL99*.BIN", SF3FileType.X1BTL99 },
            { "*X002*.BIN",    SF3FileType.X002 },
            { "*X005*.BIN",    SF3FileType.X005 },
            { "*X012*.BIN",    SF3FileType.X012 },
            { "*X013*.BIN",    SF3FileType.X013 },
            { "*X014*.BIN",    SF3FileType.X014 },
            { "*.MPD",         SF3FileType.MPD },
        };

        [TestMethod]
        public void DetermineFileType_WithoutFilter_ReturnsExpectedTypes() {
            foreach (var supportedTypeKv in c_supportedFileTypes) {
                var determinedType = DetermineFileType(supportedTypeKv.Key);
                Assert.AreEqual(supportedTypeKv.Value, determinedType);
            }
        }

        [TestMethod]
        public void DetermineFileType_WithFilter_ReturnsExpectedTypes() {
            foreach (var supportedTypeKv in c_supportedFileTypesByFilter) {
                var determinedType = DetermineFileType("FOO.BAR", supportedTypeKv.Key);
                Assert.AreEqual(supportedTypeKv.Value, determinedType);
            }
        }

        [TestMethod]
        public void DetermineFileType_WithoutFilter_WithPrependedText_ReturnsExpectedTypes() {
            foreach (var supportedTypeKv in c_supportedFileTypes) {
                var determinedType = DetermineFileType("BAK_" + supportedTypeKv.Key);
                Assert.AreEqual(supportedTypeKv.Value, determinedType);
            }
        }

        [TestMethod]
        public void DetermineFileType_WithoutFilter_WithAppendedText_ReturnsExpectedTypes() {
            foreach (var supportedTypeKv in c_supportedFileTypes) {
                var filename = Path.GetFileNameWithoutExtension(supportedTypeKv.Key) + "_BAK" + Path.GetExtension(supportedTypeKv.Key);
                var determinedType = DetermineFileType(filename);
                Assert.AreEqual(supportedTypeKv.Value, determinedType);
            }
        }

        [TestMethod]
        public void DetermineFileType_WithoutFilter_WithAppendedExtension_ReturnsExpectedTypes() {
            foreach (var supportedTypeKv in c_supportedFileTypes) {
                var determinedType = DetermineFileType(supportedTypeKv.Key + ".BAK");
                Assert.AreEqual(supportedTypeKv.Value, determinedType);
            }
        }

        [TestMethod]
        public void DetermineScenario_WithModPaths_ReturnsExpectedScenario() {
            Assert.AreEqual(ScenarioType.Scenario1,   DetermineScenario("C:\\GS-9175\\FOO.BIN"));
            Assert.AreEqual(ScenarioType.Scenario1,   DetermineScenario("C:\\Foo\\MK-81383\\FOO.BIN"));
            Assert.AreEqual(ScenarioType.Scenario2,   DetermineScenario("C:\\Foo\\GS-9188\\FOO.BIN"));
            Assert.AreEqual(ScenarioType.Scenario3,   DetermineScenario("C:\\Foo\\GS-9203\\FOO.BIN"));
            Assert.AreEqual(ScenarioType.PremiumDisk, DetermineScenario("C:\\Foo\\6106979\\FOO.BIN"));
        }

        [TestMethod]
        public void DetermineScenario_WithMountedDrives_ReturnsExpectedScenario() {
            Assert.AreEqual(ScenarioType.Scenario1,   DetermineScenario(ScenarioDataPaths[ScenarioType.Scenario1] + "FOO.BIN"));
            Assert.AreEqual(ScenarioType.Scenario2,   DetermineScenario(ScenarioDataPaths[ScenarioType.Scenario2] + "FOO.BIN"));
            Assert.AreEqual(ScenarioType.Scenario3,   DetermineScenario(ScenarioDataPaths[ScenarioType.Scenario3] + "FOO.BIN"));
            Assert.AreEqual(ScenarioType.PremiumDisk, DetermineScenario(ScenarioDataPaths[ScenarioType.PremiumDisk] + "FOO.BIN"));
        }
    }
}
