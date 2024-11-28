using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SF3.Structs.MPD.TextureChunk;

namespace SF3.Win.Extensions {
    public static class TextureExtensions {
        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB1555.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmap(this Texture texture) {
            Bitmap image = null;
            byte[] imageData = null;

            // Determine what format to use and what data to copy in.
            if (texture.CachedBitmapDataARGB1555 != null) {
                image = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555);
                imageData = texture.CachedBitmapDataARGB1555;
            }
            else if (texture.CachedBitmapDataPalette != null) {
                image = new Bitmap(texture.Width, texture.Height, PixelFormat.Format8bppIndexed);
                imageData = texture.CachedBitmapDataPalette;
            }
            else
                throw new InvalidOperationException("Unhandled bitmap type");

            // Update bitmap data.
            if (image != null && imageData != null) {
                BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
                Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);
                image.UnlockBits(bmpData);
            }
            return image;
        }
    }
}
