using SF3.Sprites;

namespace SF3.Tests.Sprites {
    [TestClass]
    public class FormatterTests {
        private static string c_bombJsonStandardFormat =
@"{
  'Name': 'Bomb',
  'Width': 16,
  'Height': 16,
  'Spritesheets': {
    '16x16': {
      'SpriteID': 394,
      'VerticalOffset': 0,
      'Unknown0x08': 8,
      'CollisionSize': 16,
      'Scale': 1.0,
      'FrameGroups': {
        'Bomb 1': {
          'Directions': 1,
          'SpritesheetX': 0,
          'SpritesheetY': 0
        }
      },
      'AnimationByDirections': {
        'OneNoFlip': {
          'StillFrame (Bomb)': [
            {
              'Frame': 'Bomb 1',
              'Duration': 1
            },
            {
              'Command': 'Stop'
            }
          ]
        }
      }
    }
  }
}".Replace('\'', '"').Replace("\r\n", "\n");

        private static string c_bombJsonFormatted =
@"{
  'Name': 'Bomb',
  'Width': 16,
  'Height': 16,
  'Spritesheets': {
    '16x16': {
      'SpriteID': 394,
      'VerticalOffset': 0,
      'Unknown0x08': 8,
      'CollisionSize': 16,
      'Scale': 1.0,
      'FrameGroups': {
        'Bomb 1': { 'Directions': 1, 'SpritesheetX': 0, 'SpritesheetY': 0 }
      },
      'AnimationByDirections': {
        'OneNoFlip': {
          'StillFrame (Bomb)': [
            { 'Frame': 'Bomb 1', 'Duration': 1 },
            { 'Command': 'Stop' }
          ]
        }
      }
    }
  }
}".Replace('\'', '"').Replace("\r\n", "\n");

        [TestMethod]
        public void Format_WithBombJsonStandardFormat_ProducesFormattedTextt () {
            var formatter = new Formatter();
            var formattedText = formatter.Format(c_bombJsonStandardFormat);
            var expectedText = c_bombJsonFormatted;
            Assert.AreEqual(expectedText, formattedText);
        }
    }
}