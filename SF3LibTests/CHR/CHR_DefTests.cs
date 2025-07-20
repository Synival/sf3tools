using Newtonsoft.Json;
using SF3.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.CHR {
    [TestClass]
    public class CHR_DefTests {
        private static readonly string c_emptyCHR_Text = @"
            { 'Sprites': [] }
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

        private static CHR_Def c_emptyCHR = JsonConvert.DeserializeObject<CHR_Def>(c_emptyCHR_Text)!;
        private static CHR_Def c_minimalCHR = JsonConvert.DeserializeObject<CHR_Def>(c_minimalCHR_Text)!;

        [TestMethod]
        public void ToCHR_File_ToStream_WithEmptyCHR_ExportsExpectedData() {
            // TODO: get real data!
            var expectedData = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            };

            byte[]? resultData = null;
            int rval;
            using (var outputStream = new MemoryStream()) {
                rval = c_emptyCHR.ToCHR_File(outputStream);
                resultData = outputStream.ToArray();
            }

            Assert.IsTrue(Enumerable.SequenceEqual(expectedData, resultData));
            Assert.AreEqual(expectedData.Length, rval);
        }

        [TestMethod]
        public void ToCHR_File_ToFile_WithEmptyCHR_ExportsExpectedData() {
            var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
            var chrFile = c_emptyCHR.ToCHR_File(nameGetterContext, nameGetterContext.Scenario);
            Assert.AreEqual(0, chrFile.SpriteTable.Length);
        }
    }
}
