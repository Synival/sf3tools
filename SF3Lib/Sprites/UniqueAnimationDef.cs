using SF3.Types;

namespace SF3.Sprites {
    public class UniqueAnimationDef {
        public UniqueAnimationDef(
            string animationHash,
            string spriteName,
            int width,
            int height,
            SpriteDirectionCountType directions,
            string animationName
        ) {
            AnimationHash        = animationHash;
            SpriteName           = spriteName;
            Width                = width;
            Height               = height;
            Directions           = directions;
            AnimationName        = animationName;
        }

        public override string ToString() => $"{SpriteName} ({Width}x{Height}x{Directions}).{AnimationName}";

        public string AnimationHash;
        public string SpriteName;
        public int Width;
        public int Height;
        public SpriteDirectionCountType Directions;
        public string AnimationName;
    }
}
