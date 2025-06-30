using System.Collections.Generic;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationTextureViewContext {
        public SpriteAnimationTextureViewContext(int spriteDirections, Dictionary<int, AnimationFrameTable> animationFramesByIndex, FrameTable frameTable) {
            SpriteDirections       = spriteDirections;
            AnimationFramesByIndex = animationFramesByIndex;
            FrameTable             = frameTable;
        }

        public readonly int SpriteDirections;
        public readonly Dictionary<int, AnimationFrameTable> AnimationFramesByIndex;
        public readonly FrameTable FrameTable;
    }

    public class SpriteAnimationTextureView : AnimatedTextureView {
        public SpriteAnimationTextureView(string name, SpriteAnimationTextureViewContext context, float textureScale = 0)
        : base(name, textureScale) {
            _context = context;
        }

        public void StartAnimation(int index) {
            if (Context == null || Context.FrameTable == null || Context?.AnimationFramesByIndex?.ContainsKey(index) != true) {
                ClearAnimation();
                _frames = null;
                return;
            }

            _frames = Context.AnimationFramesByIndex[index];
            if (_frames == null || _frames.Length == 0 || _frames[0].IsFinalFrame) {
                ClearAnimation();
                _frames = null;
                return;
            }

            _currentDirections = Context.SpriteDirections;
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

                // Normal frame with texture.
                if (nextFrame.HasTexture) {
                    nextImage = nextFrame.GetTexture(_currentDirections);
                    var duration = nextFrame.Duration;
                    if (duration > 0) {
                        SetFrame(nextImage, nextFrameIndex, duration);
                        return;
                    }
                    else
                        nextFrameIndex++;
                }
                // Special commands.
                else {
                    var cmd   = nextFrame.FrameID;
                    var param = nextFrame.Duration;

                    switch (cmd) {
                        // Set number of directions
                        case 0xF1:
                            _currentDirections = param;
                            nextFrameIndex++;
                            break;

                        // Pause
                        case 0xF2:
                            PauseAnimation();
                            return;

                        // Set duration
                        case 0xF6:
                            if (param > 0) {
                                SetFrame(nextImage, nextFrameIndex, param);
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
                            if (!Context.AnimationFramesByIndex.ContainsKey(param)) {
                                PauseAnimation();
                                return;
                            }

                            _frames = Context.AnimationFramesByIndex[param];
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

        SpriteAnimationTextureViewContext _context;
        public SpriteAnimationTextureViewContext Context {
            get => _context;
            set {
                if (_context != value) {
                    _context = value;
                    ClearAnimation();
                    _frames = null;
                }
            }
        }

        private AnimationFrameTable _frames = null;
        private int _currentDirections = 0;
    }
}