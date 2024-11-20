using System.Drawing;
using SF3.Models.MPD.TextureGroup;

namespace SF3.Win.Extensions {
    public static class FrameExtensions {
        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB1555.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmap(this FrameModel frame) {
            // TODO: find out how to get the actual image!!
            return null;
        }
    }
}
