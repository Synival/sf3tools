namespace SF3.Tests.CHR {
    public static class TestCHRs {
        public static readonly string EmptyCHR = @"
            { 'Sprites': [] }
        ";

        public static readonly string TwoEmptySpriteCHR = @"
            {
              'Sprites': [
                {
                    'Name': 'Synbios (U)',
                    'Width': 40,
                    'Height': 40,
                },
                {
                    'Name': 'Synbios (P1)',
                    'Width': 40,
                    'Height': 40,
                    'PromotionLevel': 1
                }
            ]}
        ";

        public static readonly string MinimalCHR = @"
            {
              'Sprites': [
                {
                  'Name': 'Synbios (U)',
                  'Width': 40,
                  'Height': 40,
                  'Frames': [
                    'Idle (Battle) 1',
                    'Idle (Battle) 2',
                    'Idle (Battle) 3'
                  ],
                  'Animations': [
                    'StillFrame (Battle)',
                    'Idle (Battle)'
                  ]
                }
              ]
            }
        ";

        public static readonly string LookoverChurchCHR = @"
             {
              'Sprites': [
                {
                  'Name': 'Church Priest',
                  'Frames': [
                    'Walking 1',
                    'Walking 2',
                    'Idle 1',
                    'Walking 3',
                    'Walking 4'
                  ],
                  'Animations': [
                    'StillFrame (Walking 1)',
                    'Walking (Render 1)',
                    'Walking (Render 1)'
                  ]
                },
                {
                  'Name': 'Woman w/ Blue Dress and Apron',
                  'Frames': [
                    'Walking 1',
                    'Walking 2',
                    'Walking 3',
                    'Walking 4',
                    'Walking 5'
                  ],
                  'Animations': [
                    'StillFrame',
                    'Walking (Slower)',
                    'Walking (Faster)'
                  ]
                },
                {
                  'Name': 'Old Woman',
                  'Frames': [
                    'Walking 1',
                    'Walking 2',
                    'Walking 3',
                    'Walking 4',
                    'Walking 5'
                  ],
                  'Animations': [
                    'StillFrame (Walking)',
                    'Walking',
                    'Walking'
                  ]
                }
              ]
            }
        ";

        public static readonly string LookoverChurchCHR_WithoutFrames = @"
             {
              'Sprites': [
                {
                  'Name': 'Church Priest',
                  'Animations': [
                    'StillFrame (Walking 1)',
                    'Walking (Render 1)',
                    'Walking (Render 1)'
                  ]
                },
                {
                  'Name': 'Woman w/ Blue Dress and Apron',
                  'Animations': [
                    'StillFrame',
                    'Walking (Slower)',
                    'Walking (Faster)'
                  ]
                },
                {
                  'Name': 'Old Woman',
                  'Animations': [
                    'StillFrame (Walking)',
                    'Walking',
                    'Walking'
                  ]
                }
              ]
            }
        ";
    }
}
