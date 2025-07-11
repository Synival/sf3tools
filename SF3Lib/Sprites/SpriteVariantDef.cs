using System.Linq;

namespace SF3.Sprites {
    public class SpriteVariantDef {
        public SpriteVariantDef() { }

        public SpriteVariantDef(string name, int width, int height, int directions, UniqueAnimationDef[] animations) {
            Name       = name;
            Width      = width;
            Height     = height;
            Directions = directions;
            Animations = animations
                .OrderBy(x => x.AnimationName)
                .ThenBy(x => x.AnimationHash)
                .Select(x => new AnimationDef(x))
                .ToArray();
        }

        public override string ToString() => Name;

        public string Name;
        public int Width;
        public int Height;
        public int Directions;

        public AnimationDef[] Animations;
    }
}
