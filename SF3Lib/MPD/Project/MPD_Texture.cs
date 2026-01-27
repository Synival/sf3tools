using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Texture : TextureData, IMPD_Texture {
        public MPD_Texture(IMPD_Texture original) : base(original) {
            ID         = original.ID;
            Collection = original.Collection;
            IsIgnored  = original.IsIgnored;

            if (original.Tags != null) {
                Tags = original.Tags
                    .Select(x => new KeyValuePair<TagKey, TagValue>(new TagKey(x.Key), new TagValue(x.Value)))
                    .ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public int ID { get; }
        public MPD_CollectionType Collection { get; }
        public bool IsIgnored { get; set; }

        public Dictionary<TagKey, TagValue> Tags { get; }
    }
}
