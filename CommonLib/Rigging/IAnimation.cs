namespace CommonLib.Rigging {
    public interface IAnimation {
        string AnimationName { get; }
        int StartFrame { get; }
        int FrameCount { get; }
    }
}
