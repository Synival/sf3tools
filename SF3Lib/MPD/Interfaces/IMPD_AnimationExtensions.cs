using System.Linq;
using Newtonsoft.Json.Linq;
using SF3.Imaging;

namespace SF3.MPD.Interfaces {
    public static class IMPD_AnimationExtensions {
        public static JObject ToJObject(this IMPD_Animation animation) {
            return new JObject {
                { "FrameTimerStart", animation.FrameTimerStart },
                { "Frames", JArray.FromObject(animation.Frames?.Select(x => x.ToJObject()).ToArray()) },
            };
        }
    }
}
