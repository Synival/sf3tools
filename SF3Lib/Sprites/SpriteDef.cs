using System.Linq;

namespace SF3.Sprites {
    public class SpriteDef {
        public SpriteDef() { }

        public SpriteDef(string name, FrameDef[] frames, UniqueAnimationDef[] animations) {
            Name = name;
            Frames = frames;
            Variants = animations
                .Where(y => y.AnimationFrames != null && y.AnimationFrames.Length > 0)
                .OrderBy(y => y.Width)
                .ThenBy(y => y.Height)
                .ThenBy(y => y.Directions)
                .GroupBy(y => ((y.Width & 0xFFFF) << 24) + ((y.Height & 0xFFFF) << 8) + (y.Directions & 0xFF))
                .Select(y => {
                    var first = y.First();
                    return new SpriteVariantDef($"{first.Width}x{first.Height}x{first.Directions}", first.Width, first.Height, first.Directions, y.ToArray());
                })
                .ToArray();
        }

        public override string ToString() => Name;

        public string Name;
        public FrameDef[] Frames;
        public SpriteVariantDef[] Variants;
    }
}
