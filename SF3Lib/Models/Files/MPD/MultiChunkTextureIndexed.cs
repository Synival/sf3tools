using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MultiChunkTextureIndexed : TextureDataBase {
        public const int c_width = 512;

        public MultiChunkTextureIndexed(IByteData[] datas, TexturePixelFormat format, Func<Palette> paletteGetter, Action<Palette> paletteSetter, bool isTiled = false) {
            if (datas != null) {
                for (int i = 0; i < datas.Length; i++)
                    if (datas[i].Length % c_width != 0)
                        throw new ArgumentException($"{nameof(datas)}[{i}] height is not divisible by 512");
            }

            Datas         = datas;
            _height       = (datas == null) ? 0 : datas.Select(x => x.Length / c_width).Sum();
            _pixelFormat  = format;
            PaletteGetter = paletteGetter;
            PaletteSetter = paletteSetter;
            IsTiled       = isTiled;
        }

        protected override byte[,] FetchImageData8Bit() {
            if (Datas == null)
                return null;

            var dataBytes = Datas.Select(x => x.GetDataCopy()).ToArray();
            var fullDataHeight = dataBytes.Select(x => x.Length / c_width).Sum();
            var fullDataBytes = new byte[c_width * fullDataHeight];

            int pos = 0;
            foreach (var data in dataBytes) {
                data.CopyTo(fullDataBytes, pos);
                pos += data.Length;
            }

            return IsTiled ? fullDataBytes.ToTiles(c_width, fullDataHeight, 8, 8) : fullDataBytes.To2DArrayColumnMajor(c_width, fullDataHeight);
        }

        public override string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) {
            return
                base.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize) ??
                TextureDataValidators.IsSameDimensions(data, Width, Height);
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette, 0, 0);
            if (error != null)
                throw new ArgumentException(error);

            // TODO: set the data

            PaletteSetter?.Invoke(palette);
        }

        protected override ushort[,] FetchImageData16Bit() => throw new NotSupportedException();
        protected override void SetImageData16Bit(ushort[,] data) => throw new NotSupportedException();

        private TexturePixelFormat _pixelFormat;
        public override TexturePixelFormat PixelFormat { get => _pixelFormat; set {} }

        public override int Width { get => c_width; set {} }

        private int _height;
        public override int Height { get => _height; set {} }

        public override Palette Palette {
            get => PaletteGetter();
            set { /* TODO: PaletteSetter() */ }
        }

        public override bool ZeroIsTransparent { get => false; set {} }
        public override bool CanSetImageData8Bit => !IsTiled;
        public override bool CanSetImageData16Bit => false;

        public IByteData[] Datas { get; }
        public Func<Palette> PaletteGetter { get; }
        public Action<Palette> PaletteSetter { get; }
        public bool IsTiled { get; }
    }
}
