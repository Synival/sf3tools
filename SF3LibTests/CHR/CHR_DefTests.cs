using Newtonsoft.Json;
using SF3.CHR;

namespace SF3.Tests.CHR {
    [TestClass]
    public class CHR_DefTests {
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

        private static CHR_Def c_minimalCHR = JsonConvert.DeserializeObject<CHR_Def>(c_minimalCHR_Text)!;

        [TestMethod]
        public void ToCHR_File_WithMinimalCHR_ExportsExpectedData() {
            // TODO: get real data!
            var expectedData = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            };

            byte[]? resultData = null;
            int rval;
            using (var outputStream = new MemoryStream()) {
                rval = c_minimalCHR.ToCHR_File(outputStream);
                resultData = outputStream.ToArray();
            }

            Assert.IsTrue(Enumerable.SequenceEqual(expectedData, resultData));
            Assert.AreEqual(expectedData.Length, rval);
        }
    }
}
