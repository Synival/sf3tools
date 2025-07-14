using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class SpriteDef {
        public SpriteDef() { }

        public SpriteDef(string name, UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            Name = name;
            Spritesheets = frames
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .OrderBy(x => x.FrameName)
                .ThenBy(x => x.Direction)
                .GroupBy(x => $"{x.Width}x{x.Height}")
                .ToDictionary(x => x.Key, x => new SpritesheetDef(x.ToArray()));

            Variants = GetVariants(animations);
        }

        public SpriteDef(string name, StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            Name = name;
            Spritesheets = frames
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Direction)
                .GroupBy(x => $"{x.Width}x{x.Height}")
                .ToDictionary(x => x.Key, x => new SpritesheetDef(x.ToArray()));

            Variants = GetVariants(animations);
        }

        private SpriteVariantDef[] GetVariants(UniqueAnimationDef[] animations) {
            return animations
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
        public Dictionary<string, SpritesheetDef> Spritesheets;
        public SpriteVariantDef[] Variants;
    }
}
