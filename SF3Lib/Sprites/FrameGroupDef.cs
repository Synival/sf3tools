using System.Collections.Generic;
using System.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class FrameGroupDef {
        public FrameGroupDef() { }

        public FrameGroupDef(UniqueFrameDef[] frames) {
            Name   = frames[0].FrameName;
            Frames = frames.ToDictionary(x => x.Direction.ToString(), x => new FrameDef(x));
        }

        public FrameGroupDef(StandaloneFrameDef[] frames) {
            Name   = frames[0].Name;
            Frames = frames.ToDictionary(x => x.Direction.ToString(), x => new FrameDef(x));
        }

        public override string ToString() => Name;

        public string Name;
        public Dictionary<string, FrameDef> Frames;
    }
}
