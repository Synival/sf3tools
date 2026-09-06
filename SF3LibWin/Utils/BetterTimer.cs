using System;
using System.Windows.Forms;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Utils {
    public class BetterTimer : Timer {
        public BetterTimer(int fps) {
            Interval = 1000 / (fps * 2);
            Tick += (s, a) => IncrementFrame();
        }

        private long _startTimeInMs = 0;
        private long _lastTimeInMs = 0;
        private float _lastDeltaInMs = 0.0f;

        private long GetNow() {
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds() - _startTimeInMs;
            if (_startTimeInMs == 0) {
                _startTimeInMs = now;
                _lastTimeInMs = 0;
                now = 0;
                _lastDeltaInMs = now - _lastDeltaInMs;
            }

            return now;
        }

        private void IncrementFrame() {
            // Get the currrent timestamp.
            var now = GetNow();

            // Frame delta is an average of this frame and the last to reduce some jittering.
            var deltaInMs = now - _lastTimeInMs;

            FrameTick?.Invoke(this, (_lastDeltaInMs + deltaInMs) / 2);

            _lastTimeInMs = now;
            _lastDeltaInMs = deltaInMs;
        }

        public event FrameTickEventHandler FrameTick;
    }
}
