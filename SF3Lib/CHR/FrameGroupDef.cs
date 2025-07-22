using System.Linq;

namespace SF3.CHR {
    public class FrameGroupDef {
        public override string ToString()
            => (Name != null ? Name + ": " : "") + ((Frames != null) ? string.Join(", ", Frames.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string Name;
        public FrameDef[] Frames;
    }
}
