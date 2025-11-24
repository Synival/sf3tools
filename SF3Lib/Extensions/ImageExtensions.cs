using System.Drawing;
using CommonLib.Extensions;
using SF3.Types;

namespace SF3.Extensions {
    public static class ImageExtensions {
        public static TextureABGR1555 CreateTextureABGR1555(this Image image, TextureCollectionType collection, int id, int frame, int duration) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.CreateTextureABGR1555(bitmap, collection, id, frame, duration);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.CreateTextureABGR1555(bitmap, collection, id, frame, duration);
            }
        }
    }
}
