using System;

namespace SF3.Imaging {
    public class MPD_MockAnimation : IMPD_Animation, IDisposable {
        public MPD_MockAnimation(IMPD_Texture texture) {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));
            Frames = new IMPD_AnimationFrame[] { new MPD_MockAnimationFrame(texture) };
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    (Frames[0] as IDisposable)?.Dispose();
                _disposedValue = true;
            }
        }

        public IMPD_AnimationFrame GetFrame(int timeFrame) => Frames[0];

        public int ID => Frames[0].ID;
        public int FrameTimerStart => 0;

        public IMPD_AnimationFrame[] Frames { get; }
        public bool IsIgnored => Frames[0].IsIgnored;

        private bool _disposedValue;
    }
}
