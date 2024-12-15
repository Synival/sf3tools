using System;
using System.Drawing;

namespace SF3.Win.Extensions {
    public static class BitmapExtensions {
        /// <summary>
        /// Draws a bitmap onto another bitmap without using Graphics.FromImage(), which can throw 'OutOfMemoryException'
        /// due to GDI+ implementation bugs.
        /// </summary>
        /// <param name="to">The bitmap to copy an image to.</param>
        /// <param name="from">The bitmap to copy to an image.</param>
        /// <param name="x">X coordinate of 'to' to copy to.</param>
        /// <param name="y">Y coordinate of 'to' to copy to.</param>
        public static void SafeDrawImage(this Bitmap to, Bitmap from, int x, int y) {
            for (var iy = Math.Max(-y, 0); iy < from.Height && iy + y < to.Height; iy++)
                for (var ix = Math.Max(-x, 0); ix < from.Width && ix + x < to.Width; ix++)
                    to.SetPixel(ix + x, iy + y, from.GetPixel(ix, iy));
        }
    }
}
