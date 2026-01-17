using SF3.Types;
using static SF3.Tests.Utils.DataUtils;

namespace SF3.Tests.MPD {
    public partial class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario3_VOID3_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "VOID3", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);
        }
    }
}
