using SF3.Types;

namespace SF3.Sprites {
    public class UniqueAnimationDef {
        public UniqueAnimationDef(
            string animationHash,
            string spriteName,
            string animationName,
            int width,
            int height,
            SpriteDirectionCountType directions,
            int frameCount,
            int duration,
            int missingFrames,
            AnimationCommand[] animationCommands
        ) {
            AnimationHash        = animationHash;
            SpriteName           = spriteName;
            AnimationName        = animationName;
            Width                = width;
            Height               = height;
            Directions           = directions;
            FrameCommandCount    = frameCount;
            Duration             = duration;
            FrameTexturesMissing = missingFrames;
            AnimationCommands    = animationCommands;
        }

        public override string ToString() => $"{SpriteName} ({Width}x{Height}x{Directions}).{AnimationName}";

        public string AnimationHash;
        public string SpriteName;
        public string AnimationName;
        public int Width;
        public int Height;
        public SpriteDirectionCountType Directions;
        public int FrameCommandCount;
        public int Duration;
        public int FrameTexturesMissing;
        public AnimationCommand[] AnimationCommands;
        public int RefCount;
    }
}
