using System;

namespace CommonLib.Rigging {
    public struct BoneKeyframeComponentIndices {
        public BoneKeyframeComponentIndices(int indexA, int indexB, int frameStart, int frameEnd, float currentFrame) {
            IndexA       = indexA;
            IndexB       = indexB;
            FrameStart   = frameStart;
            FrameEnd     = frameEnd;
            TotalFrames  = frameEnd - frameStart;

            CurrentFrame = currentFrame;
            FramesIn     = currentFrame - frameStart;
            FramesLeft   = TotalFrames - FramesIn;

            Mix          = FramesIn / TotalFrames;
        }

        public readonly int IndexA;
        public readonly int IndexB;
        public readonly int FrameStart;
        public readonly int FrameEnd;
        public readonly int TotalFrames;

        public readonly float CurrentFrame;
        public readonly float FramesIn;
        public readonly float FramesLeft;

        public readonly float Mix;

        public override string ToString() => $"{{ A={IndexA}, B={IndexB}, Start={FrameStart}, End={FrameEnd}, Total{TotalFrames}, Current={CurrentFrame}, Mix={Mix} }}";

        public bool SharesKeyframe(BoneKeyframeComponentIndices other)
            => FrameStart == other.FrameStart && FrameEnd == other.FrameEnd;
    }
}
