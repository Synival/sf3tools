namespace SF3.Sprites {
    public class UniqueAnimationDef {
        public UniqueAnimationDef(
            string animationHash,
            string spriteName,
            string animationName,
            int width,
            int height,
            int directions,
            int frameCount,
            int duration,
            int missingFrames,
            AnimationFrameDef[] animationFrames
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
            AnimationFrames      = animationFrames;
        }

        public string AnimationHash;
        public string SpriteName;
        public string AnimationName;
        public int Width;
        public int Height;
        public int Directions;
        public int FrameCommandCount;
        public int Duration;
        public int FrameTexturesMissing;
        public AnimationFrameDef[] AnimationFrames;
    }
}
