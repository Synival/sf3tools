using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MultiChunkTextureIndexed : TextureIndexed {
        public const int c_width = 512;

        public MultiChunkTextureIndexed(IByteData[] datas, TexturePixelFormat format, Palette palette, bool isTiled = false)
        : base(0, 0, 0, 0, FetchTextureData(datas, isTiled), format, palette, false)
        {
            IsTiled = isTiled;
        }

        private static byte[,] FetchTextureData(IByteData[] datas, bool isTiled) {
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

            return isTiled ? fullDataBytes.ToTiles(c_width, fullDataHeight, 8, 8) : fullDataBytes.To2DArrayColumnMajor(c_width, fullDataHeight);
        }

        public bool IsTiled { get; }
    }
}
