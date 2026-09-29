namespace CommonLib.Rigging {
    public struct BoneKeyframeComponentIndices {
        public int IndexA, IndexB;
        public float? FramesLeft;
        public float Mix;

        public override string ToString() => $"{{ A={IndexA}, B={IndexB}) Frames={FramesLeft}, Mix={Mix} }}";
    }
}
