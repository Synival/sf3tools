using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_AnimatableTexture : TextureData, IMPD_AnimatableTexture {
        public MPD_AnimatableTexture(IMPD_AnimatableTexture original) : base(original) {
        }

        public IMPD_Animation Animation => null;
        public MPD_CollectionType Collection { get; }
        public int ID { get; }
        public Dictionary<TagKey, TagValue> Tags => null;
        public bool IsIgnored { get; set; }
    }
}
