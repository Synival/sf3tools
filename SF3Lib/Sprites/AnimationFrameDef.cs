namespace SF3.Sprites {
    public class AnimationFrameDef {
        public override string ToString() => $"{Command}_{Parameter}";

        public string[] FrameHashes;
        public int Command;
        public int Parameter;
    };
}
