using SF3.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.CHR
{
    [TestClass]
    public class CHR_CompilerTests {

        [TestMethod]
        public void Compile_ToStream_WithEmptyCHR_ExportsExpectedData() {
            var emptyCHR = CHR_Def.FromJSON(TestCHRs.EmptyCHR);

            var expectedData = new byte[] {
                // Blank (terminating) header entry.
                0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,

                // Manditory 4 btyes at the end.
                0x00, 0x00, 0x00, 0x00
            };

            byte[]? resultData = null;
            int bytesWritten;
            using (var outputStream = new MemoryStream()) {
                var compiler = new CHR_Compiler();
                bytesWritten = compiler.Compile(emptyCHR, outputStream);

                resultData = outputStream.ToArray();
            }

            Assert.IsTrue(Enumerable.SequenceEqual(expectedData, resultData));
            Assert.AreEqual(expectedData.Length, bytesWritten);
        }

        [TestMethod]
        public void Compile_ToFile_WithEmptyCHR_ExportsExpectedData() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var emptyCHR = CHR_Def.FromJSON(TestCHRs.EmptyCHR);

            var compiler = new CHR_Compiler();
            var chrFile  = compiler.Compile(emptyCHR, nameGetterContext, nameGetterContext.Scenario);

            Assert.AreEqual(0, chrFile.SpriteTable.Length);
        }

        [TestMethod]
        public void Compile_ToFile_WithMinimalCHR_ExportsExpectedData() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var minimalCHR = CHR_Def.FromJSON(TestCHRs.MinimalCHR);

            var compiler = new CHR_Compiler();
            var chrFile  = compiler.Compile(minimalCHR, nameGetterContext, nameGetterContext.Scenario);

            Assert.AreEqual(1, chrFile.SpriteTable.Length);

            var sprite = chrFile.SpriteTable[0];
            Assert.AreEqual(0, sprite.Header.SpriteID);
            Assert.AreEqual(40, sprite.Header.Width);
            Assert.AreEqual(40, sprite.Header.Height);
            Assert.AreEqual(4, sprite.Header.Directions);
            Assert.AreEqual(0, sprite.Header.VerticalOffset);
            Assert.AreEqual(20, sprite.Header.Unknown0x08);
            Assert.AreEqual(40, sprite.Header.CollisionShadowDiameter);
            Assert.AreEqual(0, sprite.Header.PromotionLevel);
            Assert.AreEqual(65536u, sprite.Header.Scale);
        }

        [TestMethod]
        public void Compile_ToFile_WithTwoEmptySpriteCHR_ExportsSuccessfully() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var twoEmptySpriteCHR = CHR_Def.FromJSON(TestCHRs.TwoEmptySpriteCHR);

            var compiler = new CHR_Compiler();
            var chrFile  = compiler.Compile(twoEmptySpriteCHR, nameGetterContext, nameGetterContext.Scenario);

            Assert.AreEqual(2, chrFile.SpriteTable.Length);

            Assert.AreEqual(0, chrFile.SpriteTable[0].FrameTable.Length);
            Assert.AreEqual(16, chrFile.SpriteTable[0].AnimationOffsetTable.Length);
            Assert.AreEqual(0, chrFile.SpriteTable[0].AnimationTable.Length);
            for (int i = 0; i < 16; i++)
                Assert.AreEqual(0u, chrFile.SpriteTable[0].AnimationOffsetTable[i].Offset);

            Assert.AreEqual(0, chrFile.SpriteTable[1].FrameTable.Length);
            Assert.AreEqual(16, chrFile.SpriteTable[1].AnimationOffsetTable.Length);
            Assert.AreEqual(0, chrFile.SpriteTable[1].AnimationTable.Length);
            for (int i = 0; i < 16; i++)
                Assert.AreEqual(0u, chrFile.SpriteTable[1].AnimationOffsetTable[i].Offset);
        }

        [TestMethod]
        public void Compile_LookoverChurchCHRs_CompiledCHRsHaveSameFramesAndAnimations() {
            var chrDefWithFrames    = CHR_Def.FromJSON(TestCHRs.LookoverChurchCHR);
            var chrDefWithoutFrames = CHR_Def.FromJSON(TestCHRs.LookoverChurchCHR_WithoutFrames);

            // TODO: finish this thing!
            throw new NotImplementedException();
        }
    }
}
