namespace SF3.Sprites {
    public class UniqueAnimationInfo {
        public UniqueAnimationInfo(
            string animationHash,
            string spriteName,
            string animationName,
            int width,
            int height,
            int directions,
            int frameCount,
            int duration,
            int missingFrames,
            UniqueSpriteAnimationCollectionDef.Variant.Animation.Frame[] frames
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
            Frames               = frames;
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
        public UniqueSpriteAnimationCollectionDef.Variant.Animation.Frame[] Frames;
    }
}
