using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MultiChunkImage {
        public const int c_width = 512;

        public MultiChunkImage(IByteData[] datas, TexturePixelFormat format, Palette palette, bool isTiled = false) {
            IsTiled = isTiled;

            for (int i = 0; i < datas.Length; i++)
                if (datas[i].Length % c_width != 0)
                    throw new ArgumentException($"{nameof(datas)}[{i}] height is not divisible by 512");

            var dataBytes = datas.Select(x => x.GetDataCopy()).ToArray();
            var fullDataHeight = dataBytes.Select(x => x.Length / c_width).Sum();
            var fullDataBytes = new byte[c_width * fullDataHeight];

            int pos = 0;
            foreach (var data in dataBytes) {
                data.CopyTo(fullDataBytes, pos);
                pos += data.Length;
            }

            FullTexture  = new TextureIndexed(2, 0, 0, ToTextureData(fullDataBytes,  fullDataHeight),  format, palette, false);
        }

        private byte[,] ToTextureData(byte[] data, int height)
            => IsTiled ? data.ToTiles(c_width, height, 8, 8) : data.To2DArrayColumnMajor(c_width, height);

        public TextureIndexed FullTexture { get; }
        public bool IsTiled { get; }
    }
}
