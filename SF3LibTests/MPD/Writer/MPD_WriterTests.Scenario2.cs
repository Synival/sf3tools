using SF3.Analysis;
using SF3.Types;

namespace SF3.Tests.MPD.Writer {
    public partial class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario2_VOID_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "VOID", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario2_BLACK_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "BLACK", performByteComparison: true, [
                // PDATA's have the wrong addresses in the original file!! Just skip it!!
                new ByteComparisonSkipRegion { Offset = 0x2100, Size = 0x798 },

                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },

                // Whole buncha other random LZSS inconsistencies that are totally fine.
                new ByteComparisonSkipRegion { Offset = 0x0B267, Size = 1 },
                new ByteComparisonSkipRegion { Offset = 0x0F1FC, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x1577A, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x18576, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x18DDD, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x18DF2, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario2_SNIOKI_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "SNIOKI", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_BTL43_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "BTL43", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_AIRO_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "AIRO", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_SARA22_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "SARA22", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_SARA23_ProducesVerySpecificDifferences() {
            try {
                // This file actually has a bug in the animation table that we're not going to reproduce.
                // They made a big dumb mistake and added (or left in) '0xFFFE 0xFFFE 0xFFFE' before the
                // terminating '0xFFFF' in the table. The game thinks there are 3 extra animations because
                // of this, and each one is loaded with a bogus frame timer of '0xFFFF' (~36.4 minutes).
                // When those timers reach zero, the game crashes.
                // Let's not keep that bug :)
                ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "SARA23", performByteComparison: false);
            }
            catch (Exception ex) {
                Assert.AreEqual(
                    "Assert.Fail failed. \r\n" +
                    "=================================================\r\n" +
                    "Main/Header Region:\r\n" +
                    "  Comparable data is wrong: 70.71% accurate (2399 wrong bytes)\r\n" +
                    "  First wrong byte is at 3 (0x0003):\r\n" +
                    "    Should be 56 (0x38), is 48 (0x30)",
                    ex.Message);
            }
        }

        [TestMethod]
        public void WriteMPD_WithScenario2_MUBAR2_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "MUBAR2", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_BTL42_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "BTL42", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_FUNE_T_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "FUNE_T", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_STAMP2_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "STAMP2", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_STAMP3_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "STAMP3", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_ELINB_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "ELINB", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_ATBTL2_ProducesSameLoadableData() {
            try {
                // This map is funny -- it's the only time the LoD feature is actually put into use, but not for what
                // it was likely intended for. There are floor polys that only appear when very distant, at a distance
                // when the surface model is not visible. The floor polys match the surface model, so they're used to
                // make sure the long road is always visible.
                ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "ATBTL2", performByteComparison: false);
            }
            catch (Exception ex) {
                Assert.AreEqual(
                    "Assert.Fail failed. \r\n" +
                    "=================================================\r\n" +
                    "Chunk[20] (uncompressed data -- actual offset is 0x39894:\r\n" +
                    "  Length is wrong: should be 45310 (0x0B0FE), is 45426 (0x0B172)\r\n" +
                    "  Comparable data is wrong: 91.31% accurate (3942 wrong bytes)\r\n" +
                    "  First wrong byte is at 235671 (0x39897):\r\n" +
                    "    Should be 112 (0x70), is 184 (0xB8)",
                    ex.Message);

            }
        }
    }
}
