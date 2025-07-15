using System.Linq;

namespace SF3.Sprites {
    public class AnimationDef {
        public AnimationDef() { }

        public AnimationDef(UniqueAnimationDef aniInfo) : this() {
            AnimationFrames = aniInfo.AnimationFrames;
        }

        public override string ToString() => string.Join(", ", AnimationFrames.Select(x => x.ToString()));

        public AnimationFrameDef[] AnimationFrames;
    };
}
