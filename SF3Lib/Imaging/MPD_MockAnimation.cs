using System;

namespace SF3.Imaging {
    public class MPD_MockAnimation : IMPD_Animation {
        public MPD_MockAnimation(IMPD_Texture texture) {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));
            Frames = new IMPD_AnimationFrame[] { new MPD_MockAnimationFrame(texture) };
        }

        public IMPD_AnimationFrame GetFrame(int timeFrame) => Frames[0];

        public int ID => Frames[0].ID;
        public int FrameTimerStart => 0;

        public IMPD_AnimationFrame[] Frames { get; }
        public bool IsIgnored => Frames[0].IsIgnored;
    }
}
