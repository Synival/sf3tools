using System.Linq;

namespace SF3.Sprites {
    public class SpriteVariantDef {
        public SpriteVariantDef() { }

        public SpriteVariantDef(UniqueAnimationDef[] animations) {
            Animations = animations
                .OrderBy(x => x.AnimationName)
                .ThenBy(x => x.AnimationHash)
                .Select(x => new AnimationDef(x))
                .ToArray();
        }

        public override string ToString() => string.Join(", ", Animations.Select(x => x.ToString()));

        public AnimationDef[] Animations;
    }
}
