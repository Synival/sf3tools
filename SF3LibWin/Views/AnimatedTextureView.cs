using System;
using System.Drawing;
using System.Windows.Forms;
using SF3.Images;

namespace SF3.Win.Views {
    public abstract class AnimatedTextureView : TextureView {
        private const int c_framesPerSecond = 30;
        private const int c_msPerFrame = 1000 / c_framesPerSecond;

        public AnimatedTextureView(string name, float? imageScale = null) : base(name, imageScale) {}
        public AnimatedTextureView(string name, ITextureData firstTexture, float? imageScale = null) : base(name, firstTexture, imageScale) {}

        private void OnTick(object sender, EventArgs e) {
            var currentTimeMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (!Animating || Control == null || !Control.Visible) {
                _lastTickMs = currentTimeMs;
                return;
            }

            var timeElapsed = (int) (currentTimeMs - _lastTickMs);
            _lastTickMs = currentTimeMs;

            _nextFrameInMs -= timeElapsed;
            while (_nextFrameInMs <= 0) {
                _nextFrameInMs += c_msPerFrame;
                FrameCounter--;
                if (FrameCounter <= 0)
                    OnFrameCompleted();
            }
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
                _timer = new Timer() { Interval = 1000 / 60 };
                _timer.Tick += OnTick;
                _timer.Start();
                _lastTickMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                _nextFrameInMs = c_msPerFrame;
            }
            if (Paused)
                ResumeAnimation();

            Texture      = texture;
            FrameIndex   = index;
            FrameCounter = duration;

            if (FrameCounter <= 0)
                OnFrameCompleted();
        }

        public void PauseAnimation() {
            _timer?.Stop();
        }

        public void ResumeAnimation() {
            _timer?.Start();
            _lastTickMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _nextFrameInMs = c_msPerFrame;
        }

        protected abstract void OnFrameCompleted();

        public override void Destroy() {
            ClearAnimation();
            base.Destroy();
        }

        protected override Action GetImportImageAction() => null;
        protected override void OnImportImage(Image image, string filename) => throw new NotImplementedException();
        protected override Action GetExportImageAction() => null;

        public bool Animating => _timer != null;
        public bool Paused => _timer?.Enabled == false;
        public int FrameIndex { get; private set; } = 0;
        public int FrameCounter { get; private set; } = 0;

        private Timer _timer = null;
        private int _nextFrameInMs = 0;
        private long _lastTickMs = 0;
    }
}