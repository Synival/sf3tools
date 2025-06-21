using System;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public abstract class AnimatedTextureView : TextureView {
        public AnimatedTextureView(string name, float textureScale = 0) : base(name, textureScale) {}
        public AnimatedTextureView(string name, ITexture firstTexture, float textureScale = 0) : base(name, firstTexture, textureScale) {}

        private void OnTick(object sender, EventArgs e) {
            if (!Animating)
                return;

            FrameCounter--;
            if (FrameCounter <= 0)
                OnFrameCompleted();
        }

        public void ClearAnimation() {
            if (!Animating)
                return;

            _timer.Stop();
            _timer.Dispose();
            _timer = null;

            Image        = null;
            FrameCounter = 0;
            FrameIndex   = 0;
        }

        public void SetFrame(ITexture texture, int index, int duration) {
            if (!Animating) {
                _timer = new Timer() { Interval = 1000 / 30 };
                _timer.Tick += OnTick;
                _timer.Start();
            }

            Image        = texture;
            FrameIndex   = index;
            FrameCounter = duration;

            if (FrameCounter <= 0)
                OnFrameCompleted();
        }

        protected abstract void OnFrameCompleted();

        public override void Destroy() {
            ClearAnimation();
            base.Destroy();
        }

        public bool Animating => _timer != null;
        public int FrameIndex { get; private set; } = 0;
        public int FrameCounter { get; private set; } = 0;
        private Timer _timer = null;
    }
}