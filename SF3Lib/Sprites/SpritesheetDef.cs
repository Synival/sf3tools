using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class SpritesheetDef {
        public SpritesheetDef() { }

        public SpritesheetDef(UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroups = frames
                .GroupBy(x => x.FrameName)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            Variants = GetVariants(animations);
        }

        public SpritesheetDef(StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            Variants = GetVariants(animations);
        }

        public SpritesheetDef(StandaloneFrameDef[] frames, SpriteVariantDef[] variants) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            Variants = variants;
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

        public static string DimensionsToKey(int width, int height)
            => $"{width}x{height}";

        public static (int Width, int Height) KeyToDimensions(string key) {
            var xPos = key.IndexOf('x');
            return (Width: int.Parse(key.Substring(0, xPos)), Height: int.Parse(key.Substring(xPos + 1)));
        }

        public override string ToString() => string.Join(", ", FrameGroups.Keys);
        public Dictionary<string, FrameGroupDef> FrameGroups;
        public SpriteVariantDef[] Variants;
    }
}
