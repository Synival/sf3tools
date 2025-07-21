using System.Linq;

namespace SF3.CHR {
    public class SpriteFramesDef {
        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((FrameGroups != null) ? string.Join(", ", FrameGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public FrameGroupDef[] FrameGroups;
    }
}
