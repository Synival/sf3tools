using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class SpritesheetDef {
        public SpritesheetDef() { }

        public SpritesheetDef(UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroups = frames
                .GroupBy(x => x.FrameName)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            AnimationByDirections = GetAnimationGroupsByDirections(animations);
        }

        public SpritesheetDef(StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            AnimationByDirections = GetAnimationGroupsByDirections(animations);
        }

        public SpritesheetDef(StandaloneFrameDef[] frames, Dictionary<int, AnimationGroupDef> variants) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            AnimationByDirections = variants;
        }

        private Dictionary<int, AnimationGroupDef> GetAnimationGroupsByDirections(UniqueAnimationDef[] animations) {
            return animations
                .Where(y => y.AnimationFrames != null && y.AnimationFrames.Length > 0)
                .OrderBy(y => y.Width)
                .ThenBy(y => y.Height)
                .ThenBy(y => y.Directions)
                .GroupBy(y => ((y.Width & 0xFFFF) << 24) + ((y.Height & 0xFFFF) << 8) + (y.Directions & 0xFF))
                .ToDictionary(y => y.First().Directions, y => new AnimationGroupDef(y.ToArray()));
        }

        public static string DimensionsToKey(int width, int height)
            => $"{width}x{height}";

        public static (int Width, int Height) KeyToDimensions(string key) {
            var xPos = key.IndexOf('x');
            return (Width: int.Parse(key.Substring(0, xPos)), Height: int.Parse(key.Substring(xPos + 1)));
        }

        public override string ToString() => string.Join(", ", FrameGroups.Keys);

        public int VerticalOffset = 0;
        public int Unknown0x08    = 0;
        public int CollisionSize  = 0;
        public float Scale        = 0;

        public Dictionary<string, FrameGroupDef> FrameGroups;
        public Dictionary<int, AnimationGroupDef> AnimationByDirections;
    }
}
