using System.Collections.Generic;

namespace CommonLib.Rigging {
    public interface IModelAnimation {
        string AnimationName { get; }
        int StartFrame { get; }
        int FrameCount { get; }
        IReadOnlyList<IBoneKeyframe> BoneKeyframes { get; }
    }
}
