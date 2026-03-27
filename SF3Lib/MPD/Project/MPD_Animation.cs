using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using Newtonsoft.Json.Linq;
using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_Animation : IMPD_Animation, IDisposable {
        public MPD_Animation(IMPD_AnimatableTexture texture, IMPD_Animation original) {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            Texture = texture;
            FrameTimerStart = original.FrameTimerStart;

            if (original.Frames != null)
                Frames = original.Frames.Select(x => new MPD_AnimationFrame(x)).ToArray();
        }

        public static IMPD_Animation FromJToken(JToken token, IMPD_AnimatableTexture texture, IPalette indexedPalette)
            => new MPD_Animation(texture, indexedPalette, token);
        private MPD_Animation(IMPD_AnimatableTexture texture, IPalette indexedPalette, JToken token) {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            var jObject = (JObject) token;

            Texture = texture;
            FrameTimerStart = (int) jObject["FrameTimerStart"];
            Frames = jObject.GetValueIfExists("Frames", t => ((JArray) t)
                .Select((x, i) => (IMPD_AnimationFrame) MPD_AnimationFrame.FromJToken(x, texture, i + 1))
                .ToArray()
            );
        }

        public IMPD_AnimationFrame GetFrame(int frameCounter) {
            if (Frames.Length == 0)
                return null;
            if (Frames.Length == 1)
                return Frames[0];

            var totalFrameTime = 0;
            foreach (var frame in Frames)
                totalFrameTime += frame.Duration;
            var framePos = MathHelpers.ActualMod(frameCounter + FrameTimerStart, totalFrameTime);

            foreach (var frame in Frames) {
                framePos -= frame.Duration;
                if (framePos < 0)
                    return frame;
            }

            return Frames[0];
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    foreach (var frame in Frames)
                        (frame as IDisposable)?.Dispose();

                _disposedValue = true;
            }
        }

        public IMPD_AnimatableTexture Texture { get; }

        public int FrameTimerStart { get; }
        public IMPD_AnimationFrame[] Frames { get; }
        public bool IsIgnored => Texture.IsIgnored;

        private bool _disposedValue;
    }
}
