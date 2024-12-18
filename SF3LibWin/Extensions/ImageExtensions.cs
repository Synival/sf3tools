using System.Drawing;
using System.Drawing.Imaging;
using static CommonLib.Utils.PixelConversion;

namespace SF3.Win.Extensions {
    public static class ImageExtensions {
        public static Bitmap CreateBitmap(this Image image) {
            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap)) {
                graphics.DrawImage(image, 0, 0);
                graphics.Flush();
            }
            return bitmap;
        }

        public static TextureABGR1555 CreateTextureABGR1555(this Image image, int id, int frame, int duration) {
            using (var bitmap = image.CreateBitmap())
                return BitmapExtensions.CreateTextureABGR1555(bitmap, id, frame, duration);
        }
    }
}
