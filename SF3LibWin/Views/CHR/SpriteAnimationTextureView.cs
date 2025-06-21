using System;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationTextureView : AnimatedTextureView {
        public SpriteAnimationTextureView(string name, float textureScale = 0) : base(name, textureScale) {}
        public SpriteAnimationTextureView(string name, ITexture firstTexture, float textureScale = 0) : base(name, firstTexture, textureScale) {}
        protected override void OnAdvanceFrame(object sender, EventArgs e) {}
    }
}