using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_TextureExtensions {
        public static JToken ToJToken(this IMPD_Texture texture) => texture.ToJObject();
        public static JObject ToJObject(this IMPD_Texture texture) {
            var jObject = ITextureDataExtensions.ToJObject(texture, includePalette: false);

            jObject.AddFirst(new JProperty("ID", texture.TextureID));
            if (texture.Collection == MPD_CollectionType.Primary)
                jObject.Add("IsIgnored", texture.IsIgnored);

            return jObject;
        }
    }
}
