using System;
using System.Linq;

namespace SF3 {
    public class TextureAnimation {
        public TextureAnimation(int id, ITexture[] frames) {
            if (frames == null)
                throw new ArgumentNullException(nameof(frames));
            if (!frames.All(x => x.ID == id) || frames.Length == 0)
                throw new ArgumentException(nameof(id));

            ID = id;
            Frames = frames.OrderBy(x => x.Frame).ToArray();

            _frameByTimeFrame = new ITexture[Frames.Sum(x => Math.Max(0, x.Duration))];
            int pos = 0;
            foreach (var frame in Frames) {
                for (int i = 0; i < frame.Duration; i++)
                    _frameByTimeFrame[pos++] = frame;
            }
        }

        public ITexture GetFrame(int timeFrame)
            => _frameByTimeFrame.Length == 0 ? Frames[0] : _frameByTimeFrame[timeFrame % _frameByTimeFrame.Length];

        public int ID { get; set; }
        public ITexture[] Frames { get; }
        public readonly ITexture[] _frameByTimeFrame;
    }
}
