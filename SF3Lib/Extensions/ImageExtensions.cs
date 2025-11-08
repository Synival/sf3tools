using System.Drawing;
using CommonLib.Extensions;

namespace SF3.Extensions {
    public static class ImageExtensions {
        public static TextureABGR1555 CreateTextureABGR1555(this Image image, int id, int frame, int duration) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.CreateTextureABGR1555(bitmap, id, frame, duration);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.CreateTextureABGR1555(bitmap, id, frame, duration);
            }
        }
    }
}
