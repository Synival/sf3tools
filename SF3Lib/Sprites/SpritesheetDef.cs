using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class SpritesheetDef {
        public SpritesheetDef() { }

        public SpritesheetDef(UniqueFrameDef[] frames) {
            FrameWidth  = frames[0].Width;
            FrameHeight = frames[0].Height;
            FrameGroups = frames
                .GroupBy(x => x.FrameName)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));
        }

        public SpritesheetDef(StandaloneFrameDef[] frames) {
            FrameWidth  = frames[0].Width;
            FrameHeight = frames[0].Height;
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));
        }

        public override string ToString() => $"{FrameWidth}x{FrameHeight}";

        public int FrameWidth;
        public int FrameHeight;
        public Dictionary<string, FrameGroupDef> FrameGroups;
    }
}
