using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using Newtonsoft.Json.Linq;
using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_Animation : IMPD_Animation {
        public MPD_Animation(IMPD_AnimatableTexture texture, IMPD_Animation original) {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            Texture = texture;
            FrameTimerStart = original.FrameTimerStart;

            if (original.Frames != null)
                Frames = original.Frames.Select(x => new MPD_AnimationFrame(x)).ToArray();
        }

        public static IMPD_Animation FromJToken(JToken token, IMPD_AnimatableTexture texture, Palette indexedPalette)
            => new MPD_Animation(texture, indexedPalette, token);
        private MPD_Animation(IMPD_AnimatableTexture texture, Palette indexedPalette, JToken token) {
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
            // TODO: animate!!
            return null;
        }

        public IMPD_AnimatableTexture Texture { get; }

        public int FrameTimerStart { get; }
        public IMPD_AnimationFrame[] Frames { get; }
        public bool IsIgnored => Texture.IsIgnored;
    }
}
