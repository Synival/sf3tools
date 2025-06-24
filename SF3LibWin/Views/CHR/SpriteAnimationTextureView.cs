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
                SetFrame(frame.Texture, 0, frame.Duration);
            }
        }

        protected override void OnFrameCompleted() {
            var startingFrame  = FrameIndex;
            var nextFrameIndex = startingFrame + 1;
            var frameCount     = _frames.Length;
            var nextImage      = Image;

            while (true) {
                if (nextFrameIndex == startingFrame || nextFrameIndex >= frameCount) {
                    PauseAnimation();
                    return;
                }

                var nextFrame = _frames[nextFrameIndex];
                var frameID  = nextFrame.FrameID;
                var duration = nextFrame.Duration;

                // Normal frame.
                if (frameID < 0xF1) {
                    nextImage = nextFrame.Texture;
                    if (duration > 0) {
                        SetFrame(nextImage, nextFrameIndex, duration);
                        return;
                    }
                    else
                        nextFrameIndex++;
                }
                else {
                    switch (frameID) {
                        // Pause
                        case 0xF2:
                            PauseAnimation();
                            return;

                        // Set duration, and unknown that also sets duration:
                        case 0xF6:
                        case 0xFC:
                            if (duration > 0) {
                                SetFrame(nextImage, nextFrameIndex, duration);
                                return;
                            }
                            nextFrameIndex++;
                            break;

                        // Jump to frame
                        case 0xFE:
                            var isValidFrame = (nextFrame.Duration % 2 == 0 && (nextFrame.Duration / 2) < _frames.Length);
                            if (!isValidFrame) {
                                PauseAnimation();
                                return;
                            }
                            nextFrameIndex = nextFrame.Duration / 2;
                            break;

                        // Jump to animation
                        case 0xFF:
                            // TODO: actually jump to the animation!
                            PauseAnimation();
                            return;

                        // All other commands are skipped.
                        default:
                            nextFrameIndex++;
                            break;
                    }
                }
            }
        }

        private string _lastTextureHash = null;
        public ITexture _lastTexture = null;
        private AnimationFrameTable _frames = null;
    }
}