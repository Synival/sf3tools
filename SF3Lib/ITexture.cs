using SF3.Types;

namespace SF3 {
    /// <summary>
    /// Interface for any object that contains texture data.
    /// </summary>
    public interface ITexture {
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
    }
}