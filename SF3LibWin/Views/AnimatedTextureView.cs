using System;
using System.Windows.Forms;
using SF3.Images;

namespace SF3.Win.Views {
    public abstract class AnimatedTextureView : TextureView {
        public AnimatedTextureView(string name, float imageScale = 0) : base(name, imageScale) {}
        public AnimatedTextureView(string name, ITextureData firstTexture, float imageScale = 0) : base(name, firstTexture, imageScale) {}

        private void OnTick(object sender, EventArgs e) {
            if (!Animating || Control == null || !Control.Visible)
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

        public void SetFrame(ITextureData texture, int index, int duration) {
            if (!Animating) {
                _timer = new Timer() { Interval = 1000 / 30 };
                _timer.Tick += OnTick;
                _timer.Start();
            }
            if (Paused)
                ResumeAnimation();

            Texture      = texture;
            FrameIndex   = index;
            FrameCounter = duration;

            if (FrameCounter <= 0)
                OnFrameCompleted();
        }

        public void PauseAnimation() => _timer?.Stop();
        public void ResumeAnimation() => _timer?.Start();

        protected abstract void OnFrameCompleted();

        public override void Destroy() {
            ClearAnimation();
            base.Destroy();
        }

        protected override Action GetExportImageAction() => null;

        public bool Animating => _timer != null;
        public bool Paused => _timer?.Enabled == false;
        public int FrameIndex { get; private set; } = 0;
        public int FrameCounter { get; private set; } = 0;
        private Timer _timer = null;
    }
}