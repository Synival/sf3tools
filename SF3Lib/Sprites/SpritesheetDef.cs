using System.Linq;

namespace SF3.Sprites {
    public class SpritesheetDef {
        public SpritesheetDef() { }

        public SpritesheetDef(UniqueFrameDef[] frames) {
            FrameWidth  = frames[0].Width;
            FrameHeight = frames[0].Height;
            FrameGroups = frames
                .GroupBy(x => x.FrameName)
                .Select(x => new FrameGroupDef(x.ToArray()))
                .ToArray();
        }

        public SpritesheetDef(StandaloneFrameDef[] frames) {
            FrameWidth  = frames[0].Width;
            FrameHeight = frames[0].Height;
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .Select(x => new FrameGroupDef(x.ToArray()))
                .ToArray();
        }

        public override string ToString() => $"{FrameWidth}x{FrameHeight}";

        public int FrameWidth;
        public int FrameHeight;
        public FrameGroupDef[] FrameGroups;
    }
}
