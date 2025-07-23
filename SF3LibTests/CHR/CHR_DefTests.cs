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

            var expectedData = new byte[] {
                // Blank (terminating) header entry.
                0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,

                // Manditory 4 btyes at the end.
                0x00, 0x00, 0x00, 0x00
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
        public void Deserialize_WithEmptyCHR_HasExpectedData() {
            var chrDef = CHR_Def.FromJSON(c_emptyCHR_Text);

            Assert.IsNull(chrDef.JunkAfterFrameTables);
            Assert.AreEqual(0, chrDef.Sprites.Length);
        }

        [TestMethod]
        public void Deserialize_WithTwoEmptySpriteCHR_HasExpectedData() {
            var chrDef = CHR_Def.FromJSON(c_twoEmptySpriteCHR_Text);

            Assert.IsNull(chrDef.JunkAfterFrameTables);
            Assert.AreEqual(2, chrDef.Sprites.Length);

            var sprite0 = chrDef.Sprites[0];
            Assert.AreEqual(sprite0.SpriteName, "Synbios (U)");
            Assert.AreEqual(sprite0.SpriteID, 0);
            Assert.AreEqual(sprite0.Width, 40);
            Assert.AreEqual(sprite0.Height, 40);
            Assert.AreEqual(sprite0.Directions, 4);
            Assert.AreEqual(sprite0.VerticalOffset, 0);
            Assert.AreEqual(sprite0.Unknown0x08, 20);
            Assert.AreEqual(sprite0.CollisionSize, 40);
            Assert.AreEqual(sprite0.Scale, 1.00f);
            Assert.AreEqual(sprite0.PromotionLevel, null);
            Assert.AreEqual(sprite0.SpriteFrames, null);
            Assert.AreEqual(sprite0.SpriteAnimations, null);

            var sprite1 = chrDef.Sprites[1];
            Assert.AreEqual(sprite1.SpriteName, "Synbios (P1)");
            Assert.AreEqual(sprite1.SpriteID, 0);
            Assert.AreEqual(sprite1.Width, 40);
            Assert.AreEqual(sprite1.Height, 40);
            Assert.AreEqual(sprite1.Directions, 4);
            Assert.AreEqual(sprite1.VerticalOffset, 0);
            Assert.AreEqual(sprite1.Unknown0x08, 20);
            Assert.AreEqual(sprite1.CollisionSize, 40);
            Assert.AreEqual(sprite1.Scale, 1.00f);
            Assert.AreEqual(sprite1.PromotionLevel, 1);
            Assert.AreEqual(sprite1.SpriteFrames, null);
            Assert.AreEqual(sprite1.SpriteAnimations, null);
        }
    }
}
