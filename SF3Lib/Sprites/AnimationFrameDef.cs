namespace SF3.Sprites {
    public class AnimationFrameDef {
        public override string ToString() => $"{Command}_{ParameterOrDuration}";

        public string[] FrameHashes;
        public int Command;
        public int ParameterOrDuration;
    };
}
