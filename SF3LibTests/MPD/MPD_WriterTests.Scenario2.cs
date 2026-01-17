using SF3.Types;
using static SF3.Tests.Utils.DataUtils;

namespace SF3.Tests.MPD {
    public partial class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario2_VOID_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "VOID", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);
        }
    }
}
