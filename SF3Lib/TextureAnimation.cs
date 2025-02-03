using System;
using System.Linq;

namespace SF3 {
    public class TextureAnimation {
        public TextureAnimation(int id, ITexture[] frames, int frameTimerStart) {
            if (frames == null)
                throw new ArgumentNullException(nameof(frames));
            if (!frames.Where(x => x != null).All(x => x.ID == id) || frames.Length == 0)
                throw new ArgumentException(nameof(id));

            ID = id;
            FrameTimerStart = frameTimerStart;
            Frames = frames.Where(x => x != null).OrderBy(x => x.Frame).ToArray();

            _frameByTimeFrame = new ITexture[Frames.Sum(x => Math.Max(0, x.Duration))];
            int pos = 0;
            foreach (var frame in Frames) {
                for (int i = 0; i < frame.Duration; i++)
                    _frameByTimeFrame[pos++] = frame;
            }
        }

        /// <summary>
        /// Why do I have to keep re-writing this code???
        /// </summary>
        private int YetAnotherModImplementation(int num, int divisor) {
            int remainder = num % divisor;
            return remainder < 0 ? remainder + divisor : remainder;
        }

        public ITexture GetFrame(int timeFrame)
            => _frameByTimeFrame.Length == 0 ? Frames[0] : _frameByTimeFrame[YetAnotherModImplementation(timeFrame + FrameTimerStart, _frameByTimeFrame.Length)];

        public int ID { get; set; }
        public int FrameTimerStart { get; set; }
        public ITexture[] Frames { get; }
        public readonly ITexture[] _frameByTimeFrame;
    }
}
