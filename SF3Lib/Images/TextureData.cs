using System;
using System.Security.Cryptography;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Types;

namespace SF3.Images {
    public class TextureData : ITextureData {
        public TextureData(
            IByteArray data, int address,
            int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool isCompressed, bool zeroIsTransparent, bool canSetImage
        ) {
            _data              = data;
            _address           = address;
            _width             = width;
            _height            = height;
            _pixelFormat       = pixelFormat;
            _palette           = palette;
            _isCompressed      = isCompressed;
            _zeroIsTransparent = zeroIsTransparent;
            CanSetImage        = canSetImage;
        }

        private IByteArray _data;
        public IByteArray Data {
            get => _data;
            set {
                if (_data != value) {
                    _data = value;
                    Invalidate();
                }
            }
        }

        private int _address;
        public virtual int Address {
            get => _address;
            set {
                if (_address != value) {
                    _address = value;
                    Invalidate();
                }
            }
        }

        private int _width;
        public virtual int Width {
            get => _width;
            set {
                if (_width != value) {
                    _width = value;
                    Invalidate();
                }
            }
        }

        private int _height;
        public virtual int Height {
            get => _height;
            set {
                if (_height != value) {
                    _height = value;
                    Invalidate();
                }
            }
        }

        private TexturePixelFormat _pixelFormat;
        public TexturePixelFormat PixelFormat {
            get => _pixelFormat;
            set {
                if (_pixelFormat != value) {
                    _pixelFormat = value;
                    Invalidate();
                }
            }
        }

        private Palette _palette;
        public virtual Palette Palette {
            get => _palette;
            set {
                if (_palette != value) {
                    _palette = value;
                    Invalidate();
                }
            }
        }

        private bool _isCompressed;
        public bool IsCompressed {
            get => _isCompressed;
            set {
                if (_isCompressed != value) {
                    _isCompressed = value;
                    Invalidate();
                }
            }
        }

        private bool _zeroIsTransparent;
        public bool ZeroIsTransparent {
            get => _zeroIsTransparent;
            set {
                if (_zeroIsTransparent != value) {
                    _zeroIsTransparent = value;
                    Invalidate();
                }
            }
        }

        public int BytesPerPixel => PixelFormat.BytesPerPixel();
        public int StoredImageDataSize { get; private set; }
        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);
        public virtual bool CanSetImageData8Bit => CanSetImage;
        public virtual bool CanSetImageData16Bit => CanSetImage;
        public virtual bool CanSetImage { get; set; }

        public void LoadImageData() {
            // Accessing the getter performs loading.
            if (BytesPerPixel == 1)
                _ = ImageData8Bit;
            else
                _ = ImageData16Bit;
        }

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) {
            if (BytesPerPixel == 1) {
                if (_textureDataBuffer.BitmapDataARGB1555 == null)
                    _textureDataBuffer.BitmapDataARGB1555 = BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent);
                return _textureDataBuffer.BitmapDataARGB1555;
            }
            else if (highlightEndcodes) {
                if (_textureDataBuffer.BitmapDataARGB1555_Endcodes == null)
                    _textureDataBuffer.BitmapDataARGB1555_Endcodes = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit, true);
                return _textureDataBuffer.BitmapDataARGB1555_Endcodes;
            }
            else {
                if (_textureDataBuffer.BitmapDataARGB1555 == null)
                    _textureDataBuffer.BitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit, false);
                return _textureDataBuffer.BitmapDataARGB1555;
            }
        }

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) {
            if (BytesPerPixel == 1) {
                if (_textureDataBuffer.BitmapDataARGB8888 == null)
                    _textureDataBuffer.BitmapDataARGB8888 = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, ZeroIsTransparent);
                return _textureDataBuffer.BitmapDataARGB8888;
            }
            else if (highlightEndcodes) {
                if (_textureDataBuffer.BitmapDataARGB8888_Endcodes == null)
                    _textureDataBuffer.BitmapDataARGB8888_Endcodes = BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit, true);
                return _textureDataBuffer.BitmapDataARGB8888_Endcodes;
            }
            else {
                if (_textureDataBuffer.BitmapDataARGB8888 == null)
                    _textureDataBuffer.BitmapDataARGB8888 = BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit, false);
                return _textureDataBuffer.BitmapDataARGB8888;
            }
        }

        public void Invalidate() {
            if (_invalidateGuard == 0)
                _textureDataBuffer.Invalidate();
        }

        public virtual string Validate8BitImageData(byte[,] data, Palette palette) {
            if (!CanSetImageData8Bit)
                return "Not supported";
            return null;
        }

        public virtual string Validate16BitImageData(ushort[,] data) {
            if (!CanSetImageData16Bit)
                return "Not supported";
            if (IsCompressed)
                return "Changing compressed images is not yet supported";
            return null;
        }

        public int ImageDataSize => Width * Height * BytesPerPixel;

        public string Hash {
            get {
                if (_textureDataBuffer.Hash == null) {
                    using (var md5 = MD5.Create())
                        _textureDataBuffer.Hash = BitConverter.ToString(md5.ComputeHash(BitmapDataARGB1555)).Replace("-", "").ToLower();
                }
                return _textureDataBuffer.Hash;
            }
        }

        public byte[,] ImageData8Bit {
            get {
                if (_textureDataBuffer.ImageData8Bit != null)
                    return _textureDataBuffer.ImageData8Bit;
                if (BytesPerPixel != 1)
                    throw new InvalidOperationException();

                var storedSize = ImageDataSize;
                var inputData = IsCompressed
                    ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), Address, null, out storedSize, out var _)
                    : Data.GetDataCopyAt(Address, Math.Min(storedSize, Data.Length - Address));
                var outputData = new byte[Width, Height];

                var off = 0;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                        outputData[x, y] = texPixel;
                    }
                }

                StoredImageDataSize = storedSize;
                _textureDataBuffer.ImageData8Bit = outputData;
                return outputData;
            }
        }

        public void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette);
            if (error != null)
                throw new ArgumentException(error);

            if (_pixelFormat.BytesPerPixel() != 1)
                _pixelFormat = TexturePixelFormat.UnknownPalette;
            _width  = data.GetLength(0);
            _height = data.GetLength(1);

            var newData = new byte[Width * Height];
            var off = 0;
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    newData[off++] = data[x, y];

            if (IsCompressed) {
                var compressedData = Compression.CompressLZSS(newData);
                Data.SetDataAtTo(Address, compressedData.Length, compressedData);
            }
            else
                Data.SetDataAtTo(Address, newData.Length, newData);

            Invalidate();
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                _textureDataBuffer.ImageData8Bit = data;
                Palette = palette;
            }

            ImageDataSet?.Invoke(this, EventArgs.Empty);
        }

        public ushort[,] ImageData16Bit {
            get {
                if (_textureDataBuffer.ImageData16Bit != null)
                    return _textureDataBuffer.ImageData16Bit;
                if (BytesPerPixel != 2)
                    throw new InvalidOperationException();

                var storedSize = ImageDataSize;
                var inputData = (IsCompressed
                    ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), Address, null, out storedSize, out var _)
                    : Data.GetDataCopyAt(Address, Math.Min(storedSize, Data.Length - Address)))
                    .ToUShorts();

                var outputData = new ushort[Width, Height];

                var off = 0;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                        outputData[x, y] = texPixel;
                    }
                }

                StoredImageDataSize = storedSize;
                _textureDataBuffer.ImageData16Bit = outputData;
                return outputData;
            }
            set {
                var error = Validate16BitImageData(value);
                if (error != null)
                    throw new ArgumentException(error);

                PixelFormat = TexturePixelFormat.ABGR1555;
                Width  = value.GetLength(0);
                Height = value.GetLength(1);

                var off = 0;
                var newData = new byte[Width * Height * 2];
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var val = value[x, y];
                        newData[off++] = (byte) (val >> 8);
                        newData[off++] = (byte) val;
                    }
                }
                Data.SetDataAtTo(Address, newData.Length, newData);

                Invalidate();
                _textureDataBuffer.ImageData16Bit = value;

                ImageDataSet?.Invoke(this, EventArgs.Empty);
            }
        }

        private TextureDataBuffer _textureDataBuffer = new TextureDataBuffer();
        private int _invalidateGuard = 0;

        public event EventHandler ImageDataSet;
    }
}
