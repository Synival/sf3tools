using CommonLib.Imaging;
using SF3.Types;

namespace SF3.Images {
    /// <summary>
    /// Abstract representation of image data alone.
    /// </summary>
    public interface IImageData {
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
        /// Sets the 8-bit indexed image data along with the palette. Data or palette can be left unchanged by supplying 'null'.
        /// </summary>
        /// <param name="data">Data to set. If 'null', no data will be set.</param>
        /// <param name="palette">Palette to set. If 'null', no palette will be set.</param>
        void SetImageData8Bit(byte[,] data, Palette palette);

        /// <summary>
        /// Raw 16-bit image data in [x, y] order. Only usable when BytesPerPixel is 2.
        /// </summary>
        ushort[,] ImageData16Bit { get; set; }

        /// <summary>
        /// Image data for 16-bit ARGB1555 format. Cannot be set; it is based on the image data and PixelFormat.
        /// </summary>
        byte[] BitmapDataARGB1555 { get; }

        /// <summary>
        /// Gets the image data for 16-bit ARGB1555 format.
        /// </summary>
        /// <param name="highlightEndcodes">When set, 'endcode' pixels will be highlighted so they are visible.</param>
        /// <returns></returns>
        byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false);

        /// <summary>
        /// Image data for 32-bit ARGB8888 format. Cannot be set; it is based on the image data and PixelFormat.
        /// </summary>
        byte[] BitmapDataARGB8888 { get; }

        /// <summary>
        /// Gets the image data for 32-bit ARGB8888 format.
        /// </summary>
        /// <param name="highlightEndcodes">When set, 'endcode' pixels will be highlighted so they are visible.</param>
        /// <returns></returns>
        byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false);

        /// <summary>
        /// Hash for identifying this as a unique texture.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// The palette used for indexed images. Should be 'null' for non-indexed images.
        /// </summary>
        Palette Palette { get; }

        /// <summary>
        /// When 'true', the variable 'ImageData8Bit' can be set.
        /// </summary>
        bool CanSetImageData8Bit { get; }

        /// <summary>
        /// When 'true', the variable 'ImageData16Bit' can be set.
        /// </summary>
        bool CanSetImageData16Bit { get; }

        /// <summary>
        /// Checks 'data' and 'palette' to see if they're valid to be set for 'ImageData8Bit' / 'Palette'.
        /// Returns a non-null value if 'data' or 'palette' is invalid.
        /// </summary>
        /// <param name="data">8-bit data to check for validity.</param>
        /// <param name="palette">Palette for 8-bit data to set.</param>
        /// <returns>Returns 'null' if 'data' and 'palette' is valid, or a string with an error if not.</returns>
        string Validate8BitImageData(byte[,] data, Palette palette);

        /// <summary>
        /// Checks 'data' to see if it's valid to be set for 'ImageData16Bit'. Returns a non-null value if 'data' is invalid.
        /// </summary>
        /// <param name="data">16-bit data to check for validity.</param>
        /// <returns>Returns 'null' if the data is valid, or a string with an error if not.</returns>
        string Validate16BitImageData(ushort[,] data);
    }
}
