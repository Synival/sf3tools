// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SF3.Win.ThirdParty.TexturePacker.Extensions {
    public static class BitmapExtensions {
        /// <summary>
        /// Returns a new Bitmap with trimmed content, or 'null' if no non-zero data is present.
        /// </summary>
        /// <param name="source">The Bitmap to trim.</param>
        /// <returns>A new Bitmap with trimmed content, or 'null' if no non-zero data is present.</returns>
        public static Bitmap Trim(this Bitmap source, bool clampToPow2 = false) {
            Rectangle srcRect = default;
            BitmapData data = null;

            try {
                data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

                int xMin = int.MaxValue;
                int xMax = 0;
                int yMin = int.MaxValue;
                int yMax = 0;
                for (int y = 0; y < data.Height; y++) {
                    for (int x = 0; x < data.Width; x++) {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0) {
                            if (x < xMin)
                                xMin = x;
                            if (x > xMax)
                                xMax = x;
                            if (y < yMin)
                                yMin = y;
                            if (y > yMax)
                                yMax = y;
                        }
                    }
                }

                if (xMax < xMin || yMax < yMin) {
                    // Image is empty...
                    return null;
                }

                srcRect = Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
            }
            finally {
                if (data != null)
                    source.UnlockBits(data);
            }

            int width = clampToPow2 ? 1 : srcRect.Width;
            while (width < srcRect.Width)
                width *= 2;

            int height = clampToPow2 ? 1 : srcRect.Height;
            while (height < srcRect.Height)
                height *= 2;

            Bitmap dest = new Bitmap(width, height);
            Rectangle destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (Graphics graphics = Graphics.FromImage(dest))
                graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);

            return dest;
        }
    }
}
