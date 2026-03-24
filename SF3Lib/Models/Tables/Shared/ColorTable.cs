using System;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class ColorTable : FixedSizeTable<Color>, ITextureData {
        protected ColorTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
            (Width, Height) = ImageUtils.GetPaletteImageDimensions(size);
            ImageDataSize = Width * Height;

            // 8-bit data is just a sequence.
            var data8Bit = new byte[Width, Height];
            int index = 0;
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    data8Bit[x, y] = (byte) index;
                    if (index < 0xFF)
                        index++;
                }
            }
            ImageData8Bit = data8Bit;

            // Invalidate the image whenever the colors have been modified.
            data.Data.RangeModified += (s, eventData) => {
                if (eventData.IntersectsWithRange(Address, Address + SizeInBytes))
                    Invalidate();
            };

            // Store one single palette that will be updated upon invalidation.
            _palette = new Palette(size);
        }

        public void Invalidate(bool sendEvent = true) {
            _textureDataCache.Invalidate();
            _updatePalette = true;
            if (sendEvent)
                Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public static ColorTable Create(IByteData data, string name, int address, int size)
            => Create(() => new ColorTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new Color(Data, id, "Color" + id.ToString("D3"), address));

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => _textureDataCache.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, zeroIsTransparent: false));
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => _textureDataCache.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, zeroIsTransparent: false));

        public string Validate16BitImageData(ushort[,] data, int? oldStoredSize, int? newStoredSize)
            => TextureDataValidators.IsSameDimensions(data, Width, Height);

        public string Validate8BitImageData(byte[,] data, Palette palette, int? oldStoredSize, int? newStoredSize)
            => TextureDataValidators.IsSameDimensions(data, Width, Height);

        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.Indexed8Bit;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ImageDataSize { get; private set; }

        public byte[,] ImageData8Bit { get; private set; }
        public void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette, null, null);
            if (error != null)
                throw new ArgumentException(error);

            var dataWidth  = data.GetLength(0);
            var dataHeight = data.GetLength(1);

            var newColors = new ushort[Size];
            int pos = 0;
            for (int y = 0; y < dataHeight && pos < newColors.Length; y++)
                for (int x = 0; x < dataWidth && pos < newColors.Length; x++)
                    newColors[pos++] = (ushort) (palette.Colors[data[x, y]].ToABGR1555() & 0x7FFF);

            Data.Data.SetDataAtTo(Address, newColors.Length * 2, newColors.ToBytes());
        }

        public ushort[,] ImageData16Bit {
            get => throw new NotSupportedException();
            set {
                var error = Validate16BitImageData(value, null, null);
                if (error != null)
                    throw new ArgumentException(error);

                var dataWidth  = value.GetLength(0);
                var dataHeight = value.GetLength(1);

                var newColors = new ushort[Size];
                int pos = 0;
                for (int y = 0; y < dataHeight && pos < newColors.Length; y++)
                    for (int x = 0; x < dataWidth && pos < newColors.Length; x++)
                        newColors[pos++] = (ushort) (value[x, y] & 0x7FFF);

                Data.Data.SetDataAtTo(Address, newColors.Length * 2, newColors.ToBytes());
            }
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);
        public string Hash => BitmapDataARGB1555.CreateTextureHash();

        private bool _updatePalette = true;
        private readonly Palette _palette;
        public Palette Palette {
            get {
                if (_updatePalette) {
                    for (int i = 0; i < Size; ++i)
                        _palette.Colors[i] = PixelConversion.ABGR1555toChannels(Rows[i].ColorABGR1555);
                    _updatePalette = false;
                }
                return _palette;
            }
            set {
                if (value == null)
                    return;
                var newColors = new ushort[Size];
                for (int i = 0; i < Size && i < value.Colors.Length; i++)
                    newColors[i] = (ushort) (value.Colors[i].ToABGR1555() & 0x7FFF);

                Data.Data.SetDataAtTo(Address, newColors.Length * 2, newColors.ToBytes());
            }
        }

        public bool ZeroIsTransparent => false;
        public bool CanSetImageData8Bit => true;
        public bool CanSetImageData16Bit => true;


        public event EventHandler Invalidated;

        private TextureDataCache _textureDataCache = new TextureDataCache();
    }
}
