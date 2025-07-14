using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class SpritesheetDef {
        public SpritesheetDef() { }

        public SpritesheetDef(UniqueFrameDef[] frames) {
            FrameGroups = frames
                .GroupBy(x => x.FrameName)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));
        }

        public SpritesheetDef(StandaloneFrameDef[] frames) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));
        }

        public static string DimensionsToKey(int width, int height)
            => $"{width}x{height}";

        public static (int Width, int Height) KeyToDimensions(string key) {
            var xPos = key.IndexOf('x');
            return (Width: int.Parse(key.Substring(0, xPos)), Height: int.Parse(key.Substring(xPos + 1)));
        }

        public override string ToString() => string.Join(", ", FrameGroups.Keys);
        public Dictionary<string, FrameGroupDef> FrameGroups;
    }
}
