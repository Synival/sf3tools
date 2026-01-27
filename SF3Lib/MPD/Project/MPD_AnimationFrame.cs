using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_AnimationFrame : MPD_Texture, IMPD_AnimationFrame {
        public MPD_AnimationFrame(IMPD_AnimationFrame original) : base(original) {
            Frame    = original.Frame;
            Duration = original.Duration;
        }

        public int Frame { get; set; }
        public int Duration { get; set; }
    }
}
