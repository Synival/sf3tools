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
        public static byte[] CreateImageData(this ITexture texture) {
            // Determine what format to use and what data to copy in.
            if (texture.BytesPerPixel == 2 && texture.BitmapDataARGB1555 != null)
                return texture.BitmapDataARGB1555;
            else if (texture.BytesPerPixel == 1 && texture.BitmapDataIndexed != null)
                return texture.BitmapDataIndexed;
            else
                throw new InvalidOperationException("Unhandled bitmap type");
        }

        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB1555.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmap(this ITexture texture) {
            var imageData = texture.CreateImageData();

            // Determine what format to use and what data to copy in.
            Bitmap image = null;
            if (texture.BytesPerPixel == 2 && texture.BitmapDataARGB1555 != null)
                image = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555);
            else if (texture.BytesPerPixel == 1 && texture.BitmapDataIndexed != null)
                image = new Bitmap(texture.Width, texture.Height, PixelFormat.Format8bppIndexed);
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
