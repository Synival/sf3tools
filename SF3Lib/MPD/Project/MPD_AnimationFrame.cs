using Newtonsoft.Json.Linq;
using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_AnimationFrame : MPD_Texture, IMPD_AnimationFrame {
        public MPD_AnimationFrame(IMPD_AnimationFrame original) : base(original) {
            Frame    = original.Frame;
            Duration = original.Duration;
        }

        public static MPD_AnimationFrame FromJToken(JToken token, IMPD_Texture texture, int frameIndex)
            => new MPD_AnimationFrame(token, texture, frameIndex);
        private MPD_AnimationFrame(JToken token, IMPD_Texture texture, int frameIndex) : base(token["ImageData"], texture) {
            var jObject = (JObject) token;

            Frame = frameIndex;
            Duration = (int) jObject["Duration"];
        }

        public int Frame { get; set; }
        public int Duration { get; set; }
    }
}
