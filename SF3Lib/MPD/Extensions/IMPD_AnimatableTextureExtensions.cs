using Newtonsoft.Json.Linq;
using SF3.Imaging;

namespace SF3.MPD.Extensions {
    public static class IMPD_AnimatableTextureExtensions {
        public static JToken ToJToken(this IMPD_AnimatableTexture texture) => texture.ToJObject();
        public static JObject ToJObject(this IMPD_AnimatableTexture texture) {
            return new JObject {
                // TODO: content!
            };
        }
    }
}
