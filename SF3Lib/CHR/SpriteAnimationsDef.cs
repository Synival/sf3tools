using System.Linq;

namespace SF3.CHR {
    public class SpriteAnimationsDef {
        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((AnimationGroups != null) ? string.Join(", ", AnimationGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public AnimationGroupDef[] AnimationGroups;
    }
}
