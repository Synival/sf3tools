using System.Collections.Generic;
using System.Linq;

namespace SF3.Sprites {
    public class SpriteVariantDef {
        public SpriteVariantDef() { }

        public SpriteVariantDef(UniqueAnimationDef[] animations) {
            Animations = animations
                .OrderBy(x => x.AnimationName)
                .ToDictionary(x => x.AnimationName, x => new AnimationDef(x));
        }

        public override string ToString() => string.Join(", ", Animations.Keys.Select(x => x.ToString()));

        public Dictionary<string, AnimationDef> Animations;
    }
}
