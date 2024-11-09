using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SF3.Models.MPD.TextureChunk;

namespace SF3.MPDEditor.Extensions {
    public static class TextureExtensions {
        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB1555.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap GetImage(this Texture texture) {
            var image = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555);

            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            Marshal.Copy(texture.BitmapDataARGB1555, 0, bmpData.Scan0, texture.BitmapDataARGB1555.Length);
            image.UnlockBits(bmpData);

            return image;
        }
    }
}
