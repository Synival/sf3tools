using System.Collections.Generic;
using SF3.Types;

namespace SF3.Images {
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
    public interface ITexture : ITextureData {
        /// <summary>
        /// Collection to which this texture belongs.
        /// </summary>
        CollectionType Collection { get; }

        /// <summary>
        /// ID for texture.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Frame index of this texture.
        /// </summary>
        int Frame { get; }

        /// <summary>
        /// Length of time in 1/30 seconds that this frame is active.
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// Tags for identifying textures with the same Hash.
        /// </summary>
        Dictionary<TagKey, TagValue> Tags { get; }
    }
}
