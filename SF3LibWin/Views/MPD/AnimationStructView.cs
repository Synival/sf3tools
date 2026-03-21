using SF3.Models.Structs.MPD.Animation;

namespace SF3.Win.Views.MPD {
    public class AnimationStructView : AnimatedTextureView {
        public AnimationStructView(string name, AnimationStruct animation, float? imageScale = null) : base(name, imageScale) {
            Animation = animation;
        }

        private AnimationStruct _animation = null;
        public AnimationStruct Animation {
            get => _animation;
            set {
                if (_animation != value) {
                    _animation = value;
                    var aniFrame = (_animation?.AnimationFrameTable?.Count > 0) ? _animation.AnimationFrameTable[0] : null;
                    if (aniFrame == null) {
                        ClearAnimation();
                        Texture = null;
                    }
                    else
                        SetFrame(aniFrame, 0, aniFrame.Duration);
                }
            }
        }

        protected override void OnFrameCompleted() {
            var frameIndex = (FrameIndex + 1) % _animation.AnimationFrameTable.Count;
            var aniFrame = _animation.AnimationFrameTable[frameIndex];
            SetFrame(aniFrame, frameIndex, aniFrame.Duration);
        }
    }
}
