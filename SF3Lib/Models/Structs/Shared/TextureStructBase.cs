using System;
using System.Security.Cryptography;
using CommonLib.Arrays;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public abstract class TextureStructBase : Struct, ITextureData {
        public TextureStructBase(IByteData data, int id, string name, int address, int size,
            TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent)
        : base(data, id, name, address, size) {
            PixelFormat = pixelFormat;
            BytesPerPixel = PixelFormat.BytesPerPixel();
            IsCompressed = isCompressed;
            ZeroIsTransparent = zeroIsTransparent;
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0)]
        public abstract int Width { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 1)]
        public abstract int Height { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X4")]
        public int StoredImageDataSize { get; private set; }

        public void LoadImageData() {
            // Accessing the getter performs loading.
            if (BytesPerPixel == 1)
                _ = ImageData8Bit;
            else
                _ = ImageData16Bit;
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);

        private byte[] _bitmapDataARGB1555          = null;
        private byte[] _bitmapDataARGB1555_Endcodes = null;
        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) {
            if (BytesPerPixel == 1) {
                if (_bitmapDataARGB1555 == null)
                    _bitmapDataARGB1555 = BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent);
                return _bitmapDataARGB1555;
            }
            else if (highlightEndcodes) {
                if (_bitmapDataARGB1555_Endcodes == null)
                    _bitmapDataARGB1555_Endcodes = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit, true);
                return _bitmapDataARGB1555_Endcodes;
            }
            else {
                if (_bitmapDataARGB1555 == null)
                    _bitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit, false);
                return _bitmapDataARGB1555;
            }
        }

        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        private byte[] _bitmapDataARGB8888          = null;
        private byte[] _bitmapDataARGB8888_Endcodes = null;
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) {
            if (BytesPerPixel == 1) {
                if (_bitmapDataARGB8888 == null)
                    _bitmapDataARGB8888 = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, ZeroIsTransparent);
                return _bitmapDataARGB8888;
            }
            else if (highlightEndcodes) {
                if (_bitmapDataARGB8888_Endcodes == null)
                    _bitmapDataARGB8888_Endcodes = BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit, true);
                return _bitmapDataARGB8888_Endcodes;
            }
            else {
                if (_bitmapDataARGB8888 == null)
                    _bitmapDataARGB8888 = BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit, false);
                return _bitmapDataARGB8888;
            }
        }

        public virtual void InvalidateImage() {
            _hash                        = null;
            _imageData8Bit               = null;
            _imageData16Bit              = null;
            _bitmapDataARGB1555          = null;
            _bitmapDataARGB1555_Endcodes = null;
            _bitmapDataARGB8888          = null;
            _bitmapDataARGB8888_Endcodes = null;
        }

        public void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette);
            if (error != null)
                throw new ArgumentException(error);

            if (BytesPerPixel != 1)
                throw new InvalidOperationException("Incoming texture must be 1 byte-per-pixel");
            if (data.GetLength(0) != Width || data.GetLength(1) != Height)
                throw new ArgumentException("Incoming data dimensions must match specified width/height");

            var rawData = new byte[Width * Height];
            var off = 0;
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    rawData[off++] = data[x, y];

            if (IsCompressed) {
                var compressedData = Compression.CompressLZSS(rawData);
                Data.Data.SetDataAtTo(ImageDataOffset, compressedData.Length, compressedData);
            }
            else
                Data.Data.SetDataAtTo(ImageDataOffset, rawData.Length, rawData);

            InvalidateImage();
            _imageData8Bit = data;
            Palette = palette;
            OnSetImageData();
        }

        public string Validate8BitImageData(byte[,] data, Palette palette) {
            if (!CanSetImageData8Bit)
                return "Not supported";
            if (data.GetLength(0) != Width || data.GetLength(1) != Height)
                return $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {Width}x{Height}";
            return null;
        }

        public string Validate16BitImageData(ushort[,] data) {
            if (!CanSetImageData16Bit)
                return "Not supported";
            if (data.GetLength(0) != Width || data.GetLength(1) != Height)
                return $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {Width}x{Height}";
            if (IsCompressed)
                return "Changing compressed images is not yet supported";
            return null;
        }

        [TableViewModelColumn(addressField: null, displayOrder: 3, displayFormat: "X4")]
        public int ImageDataSize => Width * Height * BytesPerPixel;

        [TableViewModelColumn(addressField: null, displayName: "Pixel Format", displayOrder: 4)]
        public TexturePixelFormat PixelFormat { get; }

        public int BytesPerPixel { get; }
        public bool IsCompressed { get; }
        public bool ZeroIsTransparent { get; }

        private string _hash = null;
        [TableViewModelColumn(addressField: null, displayName: "Internal Hash", displayOrder: 4, minWidth: 225)]
        public string Hash {
            get {
                if (_hash == null) {
                    using (var md5 = MD5.Create())
                        _hash = BitConverter.ToString(md5.ComputeHash(BitmapDataARGB1555)).Replace("-", "").ToLower();
                }
                return _hash;
            }
        }

        private byte[,] _imageData8Bit = null;
        public byte[,] ImageData8Bit {
            get {
                if (_imageData8Bit != null)
                    return _imageData8Bit;
                if (BytesPerPixel != 1)
                    throw new InvalidOperationException();

                var storedSize = ImageDataSize;
                var inputData = IsCompressed
                    ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), ImageDataOffset, null, out storedSize, out var _)
                    : Data.GetDataCopyAt(ImageDataOffset, Math.Min(storedSize, Data.Length - ImageDataOffset));
                var outputData = new byte[Width, Height];

                var off = 0;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                        outputData[x, y] = texPixel;
                    }
                }

                StoredImageDataSize = storedSize;
                _imageData8Bit = outputData;
                return outputData;
            }
        }

        private ushort[,] _imageData16Bit = null;
        public ushort[,] ImageData16Bit {
            get {
                if (_imageData16Bit != null)
                    return _imageData16Bit;
                if (BytesPerPixel != 2)
                    throw new InvalidOperationException();

                var storedSize = ImageDataSize;
                var inputData = (IsCompressed
                    ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), ImageDataOffset, null, out storedSize, out var _)
                    : Data.GetDataCopyAt(ImageDataOffset, Math.Min(storedSize, Data.Length - ImageDataOffset)))
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
                _imageData16Bit = outputData;
                return outputData;
            }
            set {
                var error = Validate16BitImageData(value);
                if (error != null)
                    throw new ArgumentException(error);

                var off = 0;
                var newData = new ByteData.ByteData(new ByteArray(Width * Height * 2));
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        newData.SetWord(off, value[x, y]);
                        off += 2;
                    }
                }
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopyOrReference());

                InvalidateImage();
                _imageData16Bit = value;
                OnSetImageData();
            }
        }

        public bool CanSetImageData8Bit => BytesPerPixel == 1 && CanLoadImage;
        public bool CanSetImageData16Bit => BytesPerPixel == 2 && CanLoadImage;

        public abstract void OnSetImageData();

        public abstract int ImageDataOffset { get; set; }
        public abstract bool HasImage { get; }
        public abstract bool CanLoadImage { get; }
        public abstract Palette Palette { get; protected set; }
    }
}
