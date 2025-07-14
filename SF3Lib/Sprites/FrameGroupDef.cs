using System.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class FrameGroupDef {
        public FrameGroupDef() { }

        public FrameGroupDef(UniqueFrameDef[] frames) {
            Name   = frames[0].FrameName;
            Frames = frames.Select(x => new FrameDef(x)).ToArray();
        }

        public FrameGroupDef(StandaloneFrameDef[] frames) {
            Name   = frames[0].Name;
            Frames = frames.Select(x => new FrameDef(x)).ToArray();
        }

        public override string ToString() => Name;

        public string Name;
        public FrameDef[] Frames;
    }
}
