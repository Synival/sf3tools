using CommonLib.Extensions;
using CommonLib.Imaging;
using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_AnimatableTexture : MPD_Texture, IMPD_AnimatableTexture {
        public MPD_AnimatableTexture(IMPD_AnimatableTexture original) : base(original) {
            if (original.Animation != null)
                Animation = new MPD_Animation(this, original.Animation);
        }

        public IMPD_Animation Animation { get; }

        public static MPD_AnimatableTexture FromJToken(JToken token, MPD_CollectionType collection, Palette palette)
            => new MPD_AnimatableTexture(token, collection, palette);
        private MPD_AnimatableTexture(JToken token, MPD_CollectionType collection, Palette palette)
        : base((JObject) token, collection, palette) {
            var jObject = (JObject) token;
            if (collection == MPD_CollectionType.Primary)
                Animation = jObject.GetValueIfExists("Animation", t => MPD_Animation.FromJToken(t, this, palette));
        }
    }
}
