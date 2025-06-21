using System;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public abstract class AnimatedTextureView : TextureView {
        public AnimatedTextureView(string name, float textureScale = 0) : base(name, textureScale) {}
        public AnimatedTextureView(string name, ITexture firstTexture, float textureScale = 0) : base(name, firstTexture, textureScale) {}

        public override Control Create() {
            if (base.Create() == null)
                return null;

            _timer = new Timer() { Interval = 250 };
            _timer.Tick += OnAdvanceFrame;

            return Control;
        }

        protected abstract void OnAdvanceFrame(object sender, EventArgs e);

        public override void Destroy() {
            if (_timer != null) {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            base.Destroy();
        }

        private Timer _timer = null;
    }
}