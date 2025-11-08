using System.Drawing;
using CommonLib.Extensions;

namespace SF3.Extensions {
    public static class BitmapExtensions {
        public static TextureABGR1555 CreateTextureABGR1555(this Bitmap bitmap, int id, int frame, int duration)
            => new TextureABGR1555(id, frame, duration, bitmap.Get2DDataABGR1555());
    }
}
