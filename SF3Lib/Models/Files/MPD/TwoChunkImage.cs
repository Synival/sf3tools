using System;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class TwoChunkImage {
        public TwoChunkImage(IByteData upperData, IByteData lowerData, TexturePixelFormat format, Palette palette) {
            const int c_width = 512;

            if (upperData.Length % c_width != 0)
                throw new ArgumentException(nameof(upperData) + " height is not divisible by 512");
            if (lowerData.Length % c_width != 0)
                throw new ArgumentException(nameof(lowerData) + " height is not divisible by 512");

            var upperDataBytes = upperData.GetDataCopy();
            var upperDataHeight = upperDataBytes.Length / c_width;

            var lowerDataBytes = lowerData.GetDataCopy();
            var lowerDataHeight = lowerDataBytes.Length / c_width;

            var fullDataHeight = upperDataHeight + lowerDataHeight;
            var fullDataBytes = new byte[c_width * fullDataHeight];
            upperDataBytes.CopyTo(fullDataBytes, 0);
            lowerDataBytes.CopyTo(fullDataBytes, c_width * upperDataHeight);

            UpperTexture = new TextureIndexed(0, 0, 0, upperDataBytes.To2DArrayColumnMajor(c_width, upperDataHeight), format, palette, false);
            LowerTexture = new TextureIndexed(1, 0, 0, lowerDataBytes.To2DArrayColumnMajor(c_width, lowerDataHeight), format, palette, false);
            FullTexture  = new TextureIndexed(2, 0, 0, fullDataBytes .To2DArrayColumnMajor(c_width, fullDataHeight),  format, palette, false);
        }

        public TextureIndexed FullTexture { get; }
        public TextureIndexed UpperTexture { get; }
        public TextureIndexed LowerTexture { get; }
    }
}
