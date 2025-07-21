using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class FrameGroupDef {
        public FrameGroupDef() { }

        public FrameGroupDef(UniqueFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction.ToString(), x => new FrameDef(x));
        }

        public FrameGroupDef(StandaloneFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction.ToString(), x => new FrameDef(x));
        }

        public override string ToString() => string.Join(", ", Frames.Keys);

        public Dictionary<string, FrameDef> Frames;
    }
}
