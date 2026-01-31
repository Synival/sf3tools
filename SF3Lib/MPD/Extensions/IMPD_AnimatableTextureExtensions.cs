using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_AnimatableTextureExtensions {
        public static JToken ToJToken(this IMPD_AnimatableTexture texture) => texture.ToJObject();
        public static JObject ToJObject(this IMPD_AnimatableTexture texture) {
            var jObject = IMPD_TextureExtensions.ToJObject(texture);

            if (texture.Collection == MPD_CollectionType.Primary)
                jObject.Add("Animation", null); // TODO: animations

            return jObject;
        }
    }
}
