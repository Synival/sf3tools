namespace SF3.Models.Structs.CHR {
    public class UniqueAnimationInfo {
        public UniqueAnimationInfo(string animationHash, string spriteName, string animationName, int width, int height, int directions, int frames, int duration, int missingFrames) {
            AnimationHash = animationHash;
            SpriteName    = spriteName;
            AnimationName = animationName;
            Width         = width;
            Height        = height;
            Directions    = directions;
            Frames        = frames;
            Duration      = duration;
            MissingFrames = missingFrames;
        }

        public string AnimationHash { get; }
        public string SpriteName { get; set; }
        public string AnimationName { get; set; }
        public int Width { get; }
        public int Height { get; }
        public int Directions { get; }
        public int Frames { get; }
        public int Duration { get; set; }
        public int MissingFrames { get; }
    }
}
