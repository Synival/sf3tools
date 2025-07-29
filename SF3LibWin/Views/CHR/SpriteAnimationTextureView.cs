using System.Collections.Generic;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationTextureViewContext {
        public SpriteAnimationTextureViewContext(int spriteDirections, Dictionary<int, AnimationCommandTable> aniCommandTablesByIndex, FrameTable frameTable) {
            SpriteDirections              = spriteDirections;
            AnimationCommandTablesByIndex = aniCommandTablesByIndex;
            FrameTable                    = frameTable;
        }

        public readonly int SpriteDirections;
        public readonly Dictionary<int, AnimationCommandTable> AnimationCommandTablesByIndex;
        public readonly FrameTable FrameTable;
    }

    public class SpriteAnimationTextureView : AnimatedTextureView {
        public SpriteAnimationTextureView(string name, SpriteAnimationTextureViewContext context, float textureScale = 0)
        : base(name, textureScale) {
            _context = context;
        }

        public void StartAnimation(int index) {
            if (Context == null || Context.FrameTable == null || Context?.AnimationCommandTablesByIndex?.ContainsKey(index) != true) {
                ClearAnimation();
                _commands = null;
                return;
            }

            _commands = Context.AnimationCommandTablesByIndex[index];
            if (_commands == null || _commands.Length == 0 || _commands[0].IsFinalCommand) {
                ClearAnimation();
                _commands = null;
                return;
            }

            _currentDirections = Context.SpriteDirections;
            GotoCommand(null, 0);
        }

        protected override void OnFrameCompleted()
            => GotoCommand(FrameIndex, FrameIndex + 1);

        private void GotoCommand(int? lastCommandIndex, int nextCommandIndex) {
            var commandsSeen = new List<AnimationCommand>();
            if (lastCommandIndex.HasValue && lastCommandIndex >= 0 && lastCommandIndex < _commands.Length)
                commandsSeen.Add(_commands[lastCommandIndex.Value]);

            var commandCount = _commands.Length;
            var nextImage    = Image;

            while (true) {
                if (nextCommandIndex >= commandCount) {
                    PauseAnimation();
                    return;
                }

                // If we've gone in a loop, abort.
                var nextCommand = _commands[nextCommandIndex];
                if (commandsSeen.Contains(nextCommand)) {
                    PauseAnimation();
                    return;
                }
                commandsSeen.Add(nextCommand);

                // Normal frame with texture.
                if (nextCommand.IsFrameCommand) {
                    nextImage = nextCommand.GetTexture(_currentDirections);
                    var duration = nextCommand.Duration;
                    if (duration > 0) {
                        SetFrame(nextImage, nextCommandIndex, duration);
                        return;
                    }
                    else
                        nextCommandIndex++;
                }
                // Special commands.
                else {
                    var cmd   = nextCommand.FrameID;
                    var param = nextCommand.Duration;

                    switch (cmd) {
                        // Set number of directions
                        case 0xF1:
                            _currentDirections = param;
                            nextCommandIndex++;
                            break;

                        // Pause
                        case 0xF2:
                            PauseAnimation();
                            return;

                        // Set duration
                        case 0xF6:
                            if (param > 0) {
                                SetFrame(nextImage, nextCommandIndex, param);
                                return;
                            }
                            nextCommandIndex++;
                            break;

                        // Jump to frame
                        case 0xFE:
                            var isValidParameter = (nextCommand.Duration % 2 == 0 && (nextCommand.Duration / 2) < _commands.Length);
                            if (!isValidParameter) {
                                PauseAnimation();
                                return;
                            }
                            nextCommandIndex = nextCommand.Duration / 2;
                            break;

                        // Jump to animation
                        case 0xFF:
                            if (!Context.AnimationCommandTablesByIndex.ContainsKey(param)) {
                                PauseAnimation();
                                return;
                            }

                            _commands = Context.AnimationCommandTablesByIndex[param];
                            nextCommandIndex = 0;
                            break;

                        // All other commands are skipped.
                        default:
                            nextCommandIndex++;
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
                    _commands = null;
                }
            }
        }

        private AnimationCommandTable _commands = null;
        private int _currentDirections = 0;
    }
}