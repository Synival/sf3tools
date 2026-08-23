using System;
using CommonLib.Imaging;

namespace SF3.Imaging {
    public class MPD_MockAnimation : MockAnimatedTexture, IMPD_Animation, IDisposable {
        public MPD_MockAnimation(IMPD_Texture texture)
        : base(new IMPD_AnimationFrame[] { new MPD_MockAnimationFrame(texture) }) {
        }

        public new IMPD_AnimationFrame GetFrame(int timeFrame) => Frames[0];
        public new IMPD_AnimationFrame[] Frames { get; }

        public bool IsIgnored => Frames[0].IsIgnored;
    }
}
