using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SF3.Win.Extensions {
    public static class ITextureExtensions {
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
