using System.Linq;

namespace SF3.Sprites {
    /// <summary>
    /// Serializable defintion of animations by sprite family and variant
    /// </summary>
    public class UniqueSpriteAnimationCollectionDTO {
        public UniqueSpriteAnimationCollectionDTO() { }

        public UniqueSpriteAnimationCollectionDTO(string spriteName, UniqueAnimationInfo[] allAnimationsForVariants) {
            Name = spriteName;
            Variants = allAnimationsForVariants
                .OrderBy(y => y.Width)
                .ThenBy(y => y.Height)
                .ThenBy(y => y.Directions)
                .GroupBy(y => ((y.Width & 0xFFFF) << 24) + ((y.Height & 0xFFFF) << 8) + (y.Directions & 0xFF))
                .Select(y => new Variant(
                    (y.Key >> 24) & 0xFFFF,
                    (y.Key >>  8) & 0xFFFF,
                    (y.Key >>  0) & 0xFF,
                    y.ToArray()
                ))
                .ToArray();
        }

        public string Name;

        public class Variant {
            public Variant() { }

            public Variant(int width, int height, int directions, UniqueAnimationInfo[] animations) {
                Width      = width;
                Height     = height;
                Directions = directions;
                Animations = animations
                    .OrderBy(x => x.AnimationName)
                    .ThenBy(x => x.AnimationHash)
                    .Select(x => new Animation(x))
                    .ToArray();
            }

            public int Width;
            public int Height;
            public int Directions;

            public class Animation {
                public Animation() { }

                public Animation(UniqueAnimationInfo aniInfo) : this() {
                    Name   = aniInfo.AnimationName;
                    Hash   = aniInfo.AnimationHash;
                    Frames = aniInfo.Frames;
                }

                public string Name;
                public string Hash;

                public class Frame {
                    public string[] FrameHashes;
                    public int Command;
                    public int ParameterOrDuration;
                };

                public Frame[] Frames;
            };

            public Animation[] Animations;
        }

        public Variant[] Variants;
    }
}
