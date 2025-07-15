using System.Linq;

namespace SF3.Sprites {
    public class AnimationDef {
        public AnimationDef() { }

        public AnimationDef(UniqueAnimationDef aniInfo) : this() {
            Hash = aniInfo.AnimationHash;
            AnimationFrames = aniInfo.AnimationFrames;
        }

        public override string ToString() => string.Join(", ", AnimationFrames.Select(x => x.ToString()));

        public string Hash;
        public AnimationFrameDef[] AnimationFrames;
    };
}
