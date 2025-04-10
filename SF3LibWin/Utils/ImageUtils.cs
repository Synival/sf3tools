using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Utils;

namespace SF3.Win.Utils {
    public static class ImageUtils {
        public static void SaveBitmapToFile(string filename, ushort[,] imageData, ImageFormat imageFormat) {
            var width  = imageData.GetLength(0);
            var height = imageData.GetLength(1);
            var imageByteData = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(imageData);

            using (var bitmap = new Bitmap(width, height, PixelFormat.Format16bppArgb1555)) {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                Marshal.Copy(imageByteData, 0, bitmapData.Scan0, imageByteData.Length);
                bitmap.UnlockBits(bitmapData);
                bitmap.Save(filename, imageFormat);
            }
        }
    }
}
