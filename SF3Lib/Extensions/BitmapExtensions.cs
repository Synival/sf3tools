using System.Drawing;
using CommonLib.Extensions;
using SF3.Types;

namespace SF3.Extensions {
    public static class BitmapExtensions {
        public static TextureABGR1555 CreateTextureABGR1555(this Bitmap bitmap, TextureCollectionType collection, int id, int frame, int duration)
            => new TextureABGR1555(collection, id, frame, duration, bitmap.Get2DDataABGR1555());
    }
}
