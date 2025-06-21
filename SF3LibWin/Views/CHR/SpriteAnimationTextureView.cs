using System;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationTextureView : AnimatedTextureView {
        public SpriteAnimationTextureView(string name, float textureScale = 0) : base(name, textureScale) {}
        public SpriteAnimationTextureView(string name, ITexture firstTexture, float textureScale = 0) : base(name, firstTexture, textureScale) {}

        public void StartAnimation(AnimationFrameTable frames) {
            _frames = frames;
            if (_frames == null || _frames.Length == 0 || _frames[0].IsEndingFrame)
                ClearAnimation();
            else {
                var frame = _frames[0];
                SetFrame(frame.Texture, 0, (frame.FrameID >= 0xF0) ? 0 : frame.Duration);
            }
        }

        protected override void OnFrameCompleted() {
            var nextFrameIndex = (FrameIndex + 1) % _frames.Length;
            var nextFrame = _frames[nextFrameIndex];

            // TODO: There are some special codes here. How do they work?
            if (nextFrame.IsEndingFrame)
                nextFrame = _frames[0];

            SetFrame(nextFrame.Texture, nextFrame.ID, (nextFrame.FrameID >= 0xF0) ? 0 : nextFrame.Duration);
        }

        private AnimationFrameTable _frames = null;

    }
}