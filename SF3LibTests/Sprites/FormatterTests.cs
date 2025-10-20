using SF3.Sprites;

namespace SF3.Tests.Sprites {
    [TestClass]
    public class FormatterTests {
        private static string c_bombUnformatted =
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

        private static string c_bombFormatted =
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

        private static string c_multiFrameAnimationUnformatted =
@"{
  'Name': 'Multi-Frame Animation',
  'Spritesheets': {
    '16x16': {
      'FrameGroups': {
        'First Frame': {
          'Directions': 1,
          'SpritesheetX': 0,
          'SpritesheetY': 0
        },
        'Second Frame': {
          'Directions': 1,
          'SpritesheetX': 16,
          'SpritesheetY': 0
        }
      },
      'AnimationByDirections': {
        'OneNoFlip': {
          'Animation': [
            {
              'Frame': 'First Frame',
              'Duration': 1
            },
            {
              'Frame': 'Second Frame',
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

        private static string c_multiFrameAnimationFormatted =
@"{
  'Name': 'Multi-Frame Animation',
  'Spritesheets': {
    '16x16': {
      'FrameGroups': {
        'First Frame':  { 'Directions': 1, 'SpritesheetX': 0,  'SpritesheetY': 0 },
        'Second Frame': { 'Directions': 1, 'SpritesheetX': 16, 'SpritesheetY': 0 }
      },
      'AnimationByDirections': {
        'OneNoFlip': {
          'Animation': [
            { 'Frame': 'First Frame',  'Duration': 1 },
            { 'Frame': 'Second Frame', 'Duration': 1 },
            { 'Command': 'Stop' }
          ]
        }
      }
    }
  }
}".Replace('\'', '"').Replace("\r\n", "\n");

        [TestMethod]
        public void Format_WithBomb_ProducesFormattedText () {
            var formatter = new Formatter();
            var formattedText = formatter.Format(c_bombUnformatted);
            var expectedText = c_bombFormatted;
            Assert.AreEqual(expectedText, formattedText);
        }

        [TestMethod]
        public void Format_WithMultiFrameAnimation_ProducesFormattedText () {
            var formatter = new Formatter();
            var formattedText = formatter.Format(c_multiFrameAnimationUnformatted);
            var expectedText = c_multiFrameAnimationFormatted;
            Assert.AreEqual(expectedText, formattedText);
        }
    }
}