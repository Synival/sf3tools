using System.Collections.Generic;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationTextureView : AnimatedTextureView {
        public SpriteAnimationTextureView(string name, AnimationFrameTable[] animationFrames, float textureScale = 0)
        : base(name, textureScale) {
            AnimationFrames = animationFrames;
        }

        public void StartAnimation(int index) {
            if (index < 0 || index >= AnimationFrames.Length)
                ClearAnimation();

            _frames = AnimationFrames[index];
            if (_frames == null || _frames.Length == 0 || _frames[0].IsEndingFrame)
                ClearAnimation();
            else
                GotoFrame(null, 0);
        }

        protected override void OnFrameCompleted()
            => GotoFrame(FrameIndex, FrameIndex + 1);

        private void GotoFrame(int? lastFrameIndex, int nextFrameIndex) {
            var framesSeen = new List<AnimationFrame>();
            if (lastFrameIndex.HasValue && lastFrameIndex >= 0 && lastFrameIndex < _frames.Length)
                framesSeen.Add(_frames[lastFrameIndex.Value]);

            var frameCount = _frames.Length;
            var nextImage  = Image;

            while (true) {
                if (nextFrameIndex >= frameCount) {
                    PauseAnimation();
                    return;
                }

                // If we've gone in a loop, abort.
                var nextFrame = _frames[nextFrameIndex];
                if (framesSeen.Contains(nextFrame)) {
                    PauseAnimation();
                    return;
                }
                framesSeen.Add(nextFrame);

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
                        // Set number of directions
                        case 0xF1:
                            // TODO: this should effect the texture
                            nextFrameIndex++;
                            break;

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
                            if (duration < 0 || duration >= AnimationFrames.Length) {
                                PauseAnimation();
                                return;
                            }

                            _frames = AnimationFrames[duration];
                            nextFrameIndex = 0;
                            break;

                        // All other commands are skipped.
                        default:
                            nextFrameIndex++;
                            break;
                    }
                }
            }
        }

        public AnimationFrameTable[] AnimationFrames { get; }

        private string _lastTextureHash = null;
        public ITexture _lastTexture = null;
        private AnimationFrameTable _frames = null;
    }
}