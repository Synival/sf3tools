using System.Collections.Generic;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Types;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationTextureViewContext {
        public SpriteAnimationTextureViewContext(SpriteDirectionCountType spriteDirections, Dictionary<int, AnimationCommandTable> aniCommandTablesByIndex, FrameTable frameTable) {
            SpriteDirections              = spriteDirections;
            AnimationCommandTablesByIndex = aniCommandTablesByIndex;
            FrameTable                    = frameTable;
        }

        public readonly SpriteDirectionCountType SpriteDirections;
        public readonly Dictionary<int, AnimationCommandTable> AnimationCommandTablesByIndex;
        public readonly FrameTable FrameTable;
    }

    public class SpriteAnimationTextureView : AnimatedTextureView {
        public SpriteAnimationTextureView(string name, SpriteAnimationTextureViewContext context, float imageScale = 0)
        : base(name, imageScale) {
            _context = context;
        }

        public void StartAnimation(int index) {
            if (Context == null || Context.FrameTable == null || Context?.AnimationCommandTablesByIndex?.ContainsKey(index) != true) {
                ClearAnimation();
                _commands = null;
                return;
            }

            _commands = Context.AnimationCommandTablesByIndex[index];
            if (_commands == null || _commands.Length == 0 || _commands[0].IsEndingCommand) {
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
            var nextTexture  = Texture;

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

                var cmd     = nextCommand.Command;
                var param   = nextCommand.Parameter;
                var cmdType = nextCommand.CommandType;

                switch (cmdType) {
                    case SpriteAnimationCommandType.Frame:
                        nextTexture = nextCommand.GetTexture(_currentDirections);
                        if (param > 0) {
                            SetFrame(nextTexture, nextCommandIndex, param);
                            return;
                        }
                        else
                            nextCommandIndex++;
                        break;

                    case SpriteAnimationCommandType.SetDirectionCount:
                        _currentDirections = (SpriteDirectionCountType) param;
                        nextCommandIndex++;
                        break;

                    case SpriteAnimationCommandType.Stop:
                        PauseAnimation();
                        return;

                    case SpriteAnimationCommandType.SetDuration:
                        if (param > 0) {
                            SetFrame(nextTexture, nextCommandIndex, param);
                            return;
                        }
                        nextCommandIndex++;
                        break;

                    case SpriteAnimationCommandType.GotoCommandOffset:
                        var isValidParameter = (nextCommand.Parameter % 2 == 0 && (nextCommand.Parameter / 2) < _commands.Length);
                        if (!isValidParameter) {
                            PauseAnimation();
                            return;
                        }
                        nextCommandIndex = nextCommand.Parameter / 2;
                        break;

                    case SpriteAnimationCommandType.GotoAnimation:
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
        private SpriteDirectionCountType _currentDirections = 0;
    }
}