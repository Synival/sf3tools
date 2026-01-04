using System;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.Imaging;
using SF3.Types;

namespace SF3.Utils {
    public static class TextureUtils {
        public static ITextureData StackTextures(ITextureData[] textures, bool canSetImage) {
            if (textures.Length == 0)
                return null;

            var expectedFormat = textures[0].PixelFormat;
            if (!textures.All(x => x.PixelFormat == expectedFormat))
                throw new ArgumentException($"{textures}: not all the same pixel format");

            switch (expectedFormat) {
                case TexturePixelFormat.ABGR1555:
                    return StackTexturesABGR1555(textures, canSetImage: canSetImage);
                default:
                    throw new ArgumentException($"TexturePixelFormat '{expectedFormat}' not supported");
            }
        }

        private static TextureData StackTexturesABGR1555(ITextureData[] textures, bool canSetImage) {
            var frameDatas = textures.Select(x => x.ImageData16Bit).ToArray();
            var allData = new ushort[frameDatas.Max(x => x.GetLength(0)), frameDatas.Sum(x => x.GetLength(1))];

            // TODO: memcpy(), row by row!
            int row = 0;
            foreach (var data in frameDatas) {
                for (int y = 0; y < data.GetLength(1); y++, row++)
                    for (int x = 0; x < data.GetLength(0); x++)
                        allData[x, row] = data[x, y];
            }

            return new TextureData(allData, null, zeroIsTransparent: false, canSetImage: canSetImage);
        }
    }
}
