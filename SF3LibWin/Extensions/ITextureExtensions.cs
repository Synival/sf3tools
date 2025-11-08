using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Extensions;

namespace SF3.Win.Extensions {
    public static class ITextureExtensions {
        /// <summary>
        /// Creates a bitmap image with the same pixel format as specified.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <param name="highlightEndcodes">When set, 'endcode' pixels will be highlighted so they are visible. Not relevant for 8-bit textures.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmap(this ITexture texture, bool highlightEndcodes = false) {
            if (texture.BytesPerPixel == 1)
                return texture.CreateBitmapIndexed();
            else if (texture.BytesPerPixel == 2)
                return texture.CreateBitmapARGB1555(highlightEndcodes);
            else if (texture.BytesPerPixel == 4)
                return texture.CreateBitmapARGB8888(highlightEndcodes);
            else
                throw new InvalidOperationException($"Unhandled {nameof(texture.BytesPerPixel)} value '{texture.BytesPerPixel}'");
        }

        /// <summary>
        /// Creates a bitmap image using an a texture's indexed data.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmapIndexed(this ITexture texture) {
            var texBitmapData = texture.ImageData8Bit?.To1DArrayTransposed();
            if (texBitmapData == null)
                return null;

            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format8bppIndexed);
            bitmap.UsePalette(texture.Palette);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(texBitmapData, 0, bitmapData.Scan0, texBitmapData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB1555.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <param name="highlightEndcodes">When set, 'endcode' pixels will be highlighted so they are visible.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmapARGB1555(this ITexture texture, bool highlightEndcodes = false) {
            var texBitmapData = texture.GetBitmapDataARGB1555(highlightEndcodes);
            if (texBitmapData == null)
                return null;

            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(texBitmapData, 0, bitmapData.Scan0, texBitmapData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB8888.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <param name="highlightEndcodes">When set, 'endcode' pixels will be highlighted so they are visible.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmapARGB8888(this ITexture texture, bool highlightEndcodes = false) {
            var texBitmapData = texture.GetBitmapDataARGB8888(highlightEndcodes);
            if (texBitmapData == null)
                return null;

            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format32bppArgb);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(texBitmapData, 0, bitmapData.Scan0, texBitmapData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
