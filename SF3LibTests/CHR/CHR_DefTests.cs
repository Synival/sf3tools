using SF3.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.CHR {
    [TestClass]
    public class CHR_DefTests {
        private static readonly string c_emptyCHR_Text = @"
            { 'Sprites': [] }
        ";

        private static readonly string c_twoEmptySpriteCHR_Text = @"
            { 'Sprites': [
                {
                    'SpriteName': 'Synbios (U)',
                    'SpriteID': 0,
                    'Width': 40,
                    'Height': 40,
                    'Directions': 4,
                    'VerticalOffset': 0,
                    'Unknown0x08': 20,
                    'CollisionSize': 40,
                    'Scale': 1.0,
                },
                {
                    'SpriteName': 'Synbios (P1)',
                    'SpriteID': 0,
                    'Width': 40,
                    'Height': 40,
                    'Directions': 4,
                    'VerticalOffset': 0,
                    'Unknown0x08': 20,
                    'CollisionSize': 40,
                    'PromotionLevel': 1,
                    'Scale': 1.0,
                }
            ]}
        ";

        private static readonly string c_minimalCHR_Text = @"
            { 'Sprites': [
                {
                    'SpriteName': 'Synbios (U)',
                    'SpriteID': 0,
                    'Width': 40,
                    'Height': 40,
                    'Directions': 4,
                    'VerticalOffset': 0,
                    'Unknown0x08': 20,
                    'CollisionSize': 40,
                    'Scale': 1.0,
                    'SpriteFrames': [{ 'FrameGroups': [
                        { 'Name': 'Idle (Battle) 1' },
                        { 'Name': 'Idle (Battle) 2' },
                        { 'Name': 'Idle (Battle) 3' }
                    ]}],
                    'SpriteAnimations': [{ 'AnimationGroups': [{ 'Animations': [
                        'StillFrame (Battle)',
                        'Idle (Battle)'
                    ]}]}]
                }
            ]}
        ";

        [TestMethod]
        public void ToCHR_File_ToStream_WithEmptyCHR_ExportsExpectedData() {
            var emptyCHR = CHR_Def.FromJSON(c_emptyCHR_Text);

            // TODO: get real data!
            var expectedData = new byte[] {
                0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            };

            byte[]? resultData = null;
            int rval;
            using (var outputStream = new MemoryStream()) {
                rval = emptyCHR.ToCHR_File(outputStream);
                resultData = outputStream.ToArray();
            }

            Assert.IsTrue(Enumerable.SequenceEqual(expectedData, resultData));
            Assert.AreEqual(expectedData.Length, rval);
        }

        [TestMethod]
        public void ToCHR_File_ToFile_WithEmptyCHR_ExportsExpectedData() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var emptyCHR = CHR_Def.FromJSON(c_emptyCHR_Text);
            var chrFile = emptyCHR.ToCHR_File(nameGetterContext, nameGetterContext.Scenario);
            Assert.AreEqual(0, chrFile.SpriteTable.Length);
        }

        [TestMethod]
        public void ToCHR_File_ToFile_WithMinimalCHR_ExportsExpectedData() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var minimalCHR = CHR_Def.FromJSON(c_minimalCHR_Text);
            var chrFile = minimalCHR.ToCHR_File(nameGetterContext, nameGetterContext.Scenario);

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
        public void ToCHR_File_ToFile_WithTwoEmptySpriteCHR_ExportsSuccessfully() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var twoEmptySpriteCHR = CHR_Def.FromJSON(c_twoEmptySpriteCHR_Text);
            var chrFile = twoEmptySpriteCHR.ToCHR_File(nameGetterContext, nameGetterContext.Scenario);

            Assert.AreEqual(2, chrFile.SpriteTable.Length);
            Assert.AreEqual(0, chrFile.SpriteTable[0].FrameTable.Length);
            Assert.AreEqual(0, chrFile.SpriteTable[0].AnimationTable.Length);
            Assert.AreEqual(0, chrFile.SpriteTable[1].FrameTable.Length);
            Assert.AreEqual(0, chrFile.SpriteTable[1].AnimationTable.Length);
        }
    }
}
