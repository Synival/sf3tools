using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.Types;

namespace SF3.Imaging {
    public class TagKey {
        public TagKey(byte bitFlags) {
            BitFlags = bitFlags;
        }

        public byte BitFlags { get; }
    };

    public class TagValue {
        public TagValue(string name) {
            Name = name;
        }

        public string Name { get; }
    }

    /// <summary>
    /// Interface for any object that contains texture data.
    /// </summary>
    public interface IMPD_Texture : ITextureData {
        /// <summary>
        /// Collection to which this texture belongs.
        /// </summary>
        MPD_CollectionType Collection { get; }

        /// <summary>
        /// ID for texture.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Tags for identifying textures with the same Hash.
        /// </summary>
        Dictionary<TagKey, TagValue> Tags { get; }
    }
}
