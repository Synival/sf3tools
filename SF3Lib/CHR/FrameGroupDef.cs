using System.Linq;

namespace SF3.CHR {
    public class FrameGroupDef {
        public override string ToString()
            => (Name != null ? Name + ": " : "") + ((Directions != null) ? string.Join(", ", Directions.Select(x => x.ToString())) : "[]");

        public string Name;
        public string[] Directions;
    }
}
