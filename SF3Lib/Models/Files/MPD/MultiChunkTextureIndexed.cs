using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MultiChunkTextureIndexed : TextureData {
        public const int c_width = 512;

        public MultiChunkTextureIndexed(IByteData[] datas, TexturePixelFormat format, Func<Palette> paletteGetter, bool isTiled = false)
        : base(FetchTextureData(datas, isTiled), format, paletteGetter(), zeroIsTransparent: false, canSetImage: true) {
            Datas = datas;
            PaletteGetter = paletteGetter;
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

        public override string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) {
            var error = base.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize);
            if (error != null)
                return error;

            return (data.GetLength(0) != Width || data.GetLength(1) != Height)
                ? $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {Width}x{Height}"
                : null;
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            // TODO: do it!
            throw new NotImplementedException();

            base.SetImageData8Bit(data, palette);
            // TODO: set the data
            // TODO: set the palette
        }

        public override bool CanSetImageData8Bit => !IsTiled;
        public override bool CanSetImageData16Bit => false;

        public IByteData[] Datas { get; }
        public Func<Palette> PaletteGetter { get; }
        public bool IsTiled { get; }
    }
}
