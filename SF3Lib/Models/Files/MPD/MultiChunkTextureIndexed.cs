using System;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Files.MPD {
    public class MultiChunkTextureIndexed : TextureDataBase {
        public const int c_width = 512;

        public MultiChunkTextureIndexed(
            IByteData[] datas, TexturePixelFormat format, Func<Palette> paletteGetter, Action<Palette> paletteSetter,
            bool zeroIsTransparent, bool isTiled
        ) {
            if (datas != null) {
                for (int i = 0; i < datas.Length; i++)
                    if (datas[i] != null && datas[i].Length % c_width != 0)
                        throw new ArgumentException($"{nameof(datas)}[{i}] height is not divisible by 512");
            }

            Datas         = datas;
            _height       = (datas == null) ? 0 : datas.Select(x => (x?.Length ?? 0) / c_width).Sum();
            _pixelFormat  = format;
            PaletteGetter = paletteGetter;
            PaletteSetter = paletteSetter;
            _zeroIsTransparent = zeroIsTransparent;
            IsTiled       = isTiled;
        }

        protected override byte[,] FetchImageData8Bit() {
            if (Datas == null)
                return null;

            var dataBytes = Datas.Where(x => x != null).Select(x => x.GetDataCopyOrReference()).ToArray();
            var fullDataHeight = dataBytes.Select(x => x.Length / c_width).Sum();
            var fullDataBytes = new byte[c_width * fullDataHeight];

            int pos = 0;
            foreach (var data in dataBytes) {
                data.CopyTo(fullDataBytes, pos);
                pos += data.Length;
            }

            return IsTiled ? fullDataBytes.ToTiles(c_width, fullDataHeight, 8, 8) : fullDataBytes.To2DArrayColumnMajor(c_width, fullDataHeight);
        }

        public override string Validate8BitImageData(byte[,] data, Palette palette, int? oldStoredSize, int? newStoredSize) {
            return
                base.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize) ??
                TextureDataValidators.IsSameDimensions(data, Width, Height);
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette, null, null);
            if (error != null)
                throw new ArgumentException(error);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                var toDatas = Datas?.Where(x => x != null)?.ToArray() ?? new IByteData[0];
                var fromData = IsTiled ? data.FromTiles(8, 8) : data.To1DArrayTransposed();
                int fromDataPos = 0;

                foreach (var toData in toDatas) {
                    var newData = new byte[toData.Length];
                    for (int i = 0; i < toData.Length; i++)
                        newData[i] = fromData[fromDataPos++];
                    toData.Data.SetDataTo(newData);
                }

                _textureDataBuffer.SetImageData8Bit(data);
                PaletteSetter?.Invoke(palette);
            }
            InvokeInvalidatedEvent();
        }

        protected override ushort[,] FetchImageData16Bit() => throw new NotSupportedException();
        protected override void SetImageData16Bit(ushort[,] data) => throw new NotSupportedException();

        private TexturePixelFormat _pixelFormat;
        public override TexturePixelFormat PixelFormat { get => _pixelFormat; set {} }

        public override int Width { get => c_width; set {} }

        private int _height;
        public override int Height { get => _height; set {} }

        public override Palette Palette {
            get => PaletteGetter?.Invoke();
            set => PaletteSetter?.Invoke(value);
        }

        private bool _zeroIsTransparent;
        public override bool ZeroIsTransparent { get => _zeroIsTransparent; set {} }

        public override bool CanSetImageData8Bit => true;
        public override bool CanSetImageData16Bit => false;

        public IByteData[] Datas { get; }
        public Func<Palette> PaletteGetter { get; }
        public Action<Palette> PaletteSetter { get; }
        public bool IsTiled { get; }
    }
}
