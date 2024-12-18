using System.Collections.Generic;
using SF3.Types;

namespace SF3 {
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
    public interface ITexture {
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
        /// The number of bytes per pixel, in an inclusive range from (1 - 4).
        /// </summary>
        int BytesPerPixel { get; }

        /// <summary>
        /// Pixel format that determines what kinds of data are available.
        /// </summary>
        TexturePixelFormat PixelFormat { get; }

        /// <summary>
        /// Width of the image.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the image.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Raw 8-bit image data in [x, y] order. Only usable when BytesPerPixel is 1.
        /// </summary>
        byte[,] ImageData8Bit { get; }

        /// <summary>
        /// Raw 16-bit image data in [x, y] order. Only usable when BytesPerPixel is 2.
        /// </summary>
        ushort[,] ImageData16Bit { get; }

        /// <summary>
        /// Image data for 8-bit palette format. Cannot be set; it is based on data from ImageData8Bit. Only usable when BytesPerPixel is 1.
        /// TODO: actually have a palette!!
        /// </summary>
        byte[] BitmapDataIndexed { get; }

        /// <summary>
        /// Image data for 16-bit ARGB1555 format. Cannot be set; it is based on data from ImageData16Bit. Only usable when BytesPerPixel is 2.
        /// </summary>
        byte[] BitmapDataARGB1555 { get; }

        /// <summary>
        /// Hash for identifying this as a unique texture.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// Tags for identifying textures with the same Hash.
        /// </summary>
        Dictionary<TagKey, TagValue> Tags { get; }
    }
}
