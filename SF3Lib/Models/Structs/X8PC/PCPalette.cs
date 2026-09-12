using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.Models.Structs.Shared.SGL;
using static CommonLib.Utils.ImageUtils;

namespace SF3.Models.Structs.X8PC {
    public class PCPalette : IPalette, ITextureData {
        public PCPalette(AttrStruct[][] attrLists) {
            _attrLists = attrLists;
            ColorCount = attrLists.Length;
            (Width, Height) = GetPaletteImageDimensions(ColorCount);
            ImageDataSize = Width * Height * BytesPerPixel;
        }

        public int ColorCount { get; }
        public int Width { get; }
        public int Height { get; }
        public int BytesPerPixel => 2;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.ABGR1555;
        public int ImageDataSize { get; }

        public PixelChannels[] Colors => _attrLists.Select(x => PixelConversion.ABGR1555toChannels(x[0].ColorNo)).ToArray();

        public PixelChannels this[int index] {
            get {
                if (index < 0 || index >= ColorCount)
                    throw new IndexOutOfRangeException();
                return PixelConversion.ABGR1555toChannels(_attrLists[index][0].ColorNo);
            }
            set {
                if (index < 0 || index >= ColorCount)
                    throw new IndexOutOfRangeException();
                var colorNo = value.ToABGR1555();
                foreach (var attr in _attrLists[index])
                    attr.ColorNo = colorNo;
                Invalidate();
            }
        }

        public void Replace(PixelChannels[] colors) {
            if (colors == null)
                throw new ArgumentNullException();
            if (colors.Length != ColorCount)
                throw new ArgumentException();

            for (int i = 0; i < colors.Length; i++) {
                var colorNo = colors[i].ToABGR1555();
                foreach (var attr in _attrLists[i])
                    attr.ColorNo = colorNo;
            }
            Invalidate();
        }

        // Not supported
        public void SetImageData8Bit(byte[,] data, IPalette palette) => throw new NotSupportedException();
        public byte[,] ImageData8Bit => throw new NotSupportedException();
        public IPalette Palette => null;
        public bool ZeroIsTransparent => false;

        // Supported
        public ushort[,] ImageData16Bit {
            get {
                return _textureDataCache.GetOrCacheImageData16Bit(() => {
                    var data = new ushort[Width, Height];
                    var colors = Colors;
                    for (int i = 0; i < ColorCount; i++)
                        data[i % Width, i / Width] = colors[i].ToABGR1555();
                    return data;
                });
            }
            set {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length != Width * Height)
                    throw new ArgumentException();

                var newColors = new PixelChannels[ColorCount];
                for (int i = 0; i < ColorCount; i++)
                    newColors[i] = PixelConversion.ABGR1555toChannels(value[i % Width, i / Width]);

                Replace(newColors);
            }
        }

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => _textureDataCache.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit));
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => _textureDataCache.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit));
        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555();
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888();

        public string Validate16BitImageData(ushort[,] data, int? oldStoredSize, int? newStoredSize)
            => TextureDataValidators.IsSameDimensions(data, Width, Height);
        public string Validate8BitImageData(byte[,] data, IPalette palette, int? oldStoredSize, int? newStoredSize)
            => TextureDataValidators.IsSameDimensions(data, Width, Height);

        public string Hash => BitmapDataARGB1555.CreateTextureHash();

        public ImageDataCanSet CanSetImageData { get => ImageDataCanSet.CanSet16Bit; set => throw new NotSupportedException(); }

        public void Invalidate(bool sendEvent = true) {
            _textureDataCache.Invalidate();
            if (sendEvent)
                Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Invalidated;

        private readonly AttrStruct[][] _attrLists;
        private TextureDataCache _textureDataCache = new TextureDataCache();
    }
}
