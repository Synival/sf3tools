using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.Types;

namespace SF3.Imaging {
    public class TagKey {
        public TagKey(byte bitFlags) {
            BitFlags = bitFlags;
        }

        public TagKey(TagKey original) {
            BitFlags = original.BitFlags;
        }

        public byte BitFlags { get; }
    };

    public class TagValue {
        public TagValue(string name) {
            Name = name;
        }

        public TagValue(TagValue original) {
            Name = original.Name;
        }

        public string Name { get; }
    }

    /// <summary>
    /// Interface for textures stored in an MPD.
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

        /// <summary>
        /// When 'true', this texture is not allocated in VRAM and should not be used.
        /// </summary>
        bool IsIgnored { get; }
    }
}
