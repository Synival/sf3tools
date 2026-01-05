using System;
using System.Linq;
using CommonLib.Utils;

namespace SF3.Imaging {
    public class MPD_Animation : IMPD_Animation {
        public MPD_Animation(int id, IMPD_AnimationFrame[] frames, int frameTimerStart) {
            if (frames == null)
                throw new ArgumentNullException(nameof(frames));
            if (!frames.Where(x => x != null).All(x => x.ID == id) || frames.Length == 0)
                throw new ArgumentException(nameof(id));

            ID = id;
            FrameTimerStart = frameTimerStart;
            Frames = frames.Where(x => x != null).OrderBy(x => x.Frame).ToArray();

            _frameByTimeFrame = new IMPD_AnimationFrame[Frames.Sum(x => Math.Max(0, x.Duration))];
            var pos = 0;
            foreach (var frame in Frames) {
                for (var i = 0; i < frame.Duration; i++)
                    _frameByTimeFrame[pos++] = frame;
            }
        }

        public IMPD_AnimationFrame GetFrame(int timeFrame)
            => _frameByTimeFrame.Length == 0 ? Frames[0] : _frameByTimeFrame[MathHelpers.ActualMod(timeFrame + FrameTimerStart, _frameByTimeFrame.Length)];

        public int ID { get; }
        public int FrameTimerStart { get; }
        public IMPD_AnimationFrame[] Frames { get; }

        private readonly IMPD_AnimationFrame[] _frameByTimeFrame;
    }
}
