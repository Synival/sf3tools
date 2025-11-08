using System.Drawing;
using System.Drawing.Imaging;

namespace CommonLib.Extensions {
    public static class ImageExtensions {
        public static Bitmap CreateARGB8888Bitmap(this Image image) {
            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap)) {
                graphics.DrawImage(image, 0, 0);
                graphics.Flush();
            }
            return bitmap;
        }

        public static byte[] GetBitmapDataBGRA8888(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetBitmapDataBGRA8888(bitmap);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.GetBitmapDataBGRA8888(bitmap);
            }
        }

        public static ushort[] GetDataABGR1555(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetDataABGR1555(bitmap);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.GetDataABGR1555(bitmap);
            }
        }

        public static ushort[,] Get2DDataABGR1555(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get2DDataABGR1555(bitmap);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.Get2DDataABGR1555(bitmap);
            }
        }
    }
}
