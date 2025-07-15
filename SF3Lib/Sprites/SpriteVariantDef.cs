using System.Linq;

namespace SF3.Sprites {
    public class SpriteVariantDef {
        public SpriteVariantDef() { }

        public SpriteVariantDef(string name, UniqueAnimationDef[] animations) {
            Name       = name;
            Animations = animations
                .OrderBy(x => x.AnimationName)
                .ThenBy(x => x.AnimationHash)
                .Select(x => new AnimationDef(x))
                .ToArray();
        }

        public override string ToString() => Name;

        public string Name;
        public AnimationDef[] Animations;
    }
}
