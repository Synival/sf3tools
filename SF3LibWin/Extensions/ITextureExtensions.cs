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
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmap(this ITexture texture) {
            var texBitmapData = texture.BitmapDataARGB1555;
            if (texBitmapData == null)
                return null;

            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(texBitmapData, 0, bitmapData.Scan0, texBitmapData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
