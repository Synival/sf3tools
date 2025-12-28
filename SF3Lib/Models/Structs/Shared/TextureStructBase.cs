using System;
using CommonLib.Arrays;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public abstract class TextureStructBase : Struct, ITextureData {
        public TextureStructBase(IByteData data, IByteArray imageData, int id, string name, int address, int size,
            TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent)
        : base(data, id, name, address, size) {
            _textureData = new TextureData(imageData, 0, 0, 0, pixelFormat, null,
                isCompressed: isCompressed, zeroIsTransparent: zeroIsTransparent, canSetImage: true);

            _textureData.Add8BitValidator((texData, _1, _2, _3) => {
                return (texData.GetLength(0) != Width || texData.GetLength(1) != Height)
                    ? $"Incoming texture height ({texData.GetLength(0)}x{texData.GetLength(1)}) should be {Width}x{Height}"
                    : null;
            });

            _textureData.Add16BitValidator((texData, _1, _2) => {
                return (texData.GetLength(0) != Width || texData.GetLength(1) != Height)
                    ? $"Incoming texture height ({texData.GetLength(0)}x{texData.GetLength(1)}) should be {Width}x{Height}"
                    : null;
            });

            _textureData.ImageDataSet += (s, e) => OnSetImageData();
        }

        public TextureStructBase(IByteData data, int id, string name, int address, int size,
            TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent)
        : this(data, data.Data, id, name, address, size, pixelFormat, isCompressed, zeroIsTransparent) {}

        public void LoadImageData() {
            _textureData.ImageDataOffset = StructImageDataOffset;
            _textureData.Width   = StructWidth;
            _textureData.Height  = StructHeight;
            _textureData.Palette = StructPalette;
            _textureData.LoadImageData();
        }

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB1555(highlightEndcodes);
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB8888(highlightEndcodes);
        public void InvalidateImage() => _textureData.Invalidate();

        public virtual string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize)
            => _textureData.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize);

        public virtual string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize)
            => _textureData.Validate16BitImageData(data, oldStoredSize, newStoredSize);

        public byte[] BitmapDataARGB1555 => _textureData.BitmapDataARGB1555;
        public byte[] BitmapDataARGB8888 => _textureData.BitmapDataARGB8888;
        public int BytesPerPixel => _textureData.BytesPerPixel;
        public bool IsCompressed => _textureData.IsCompressed;
        public bool ZeroIsTransparent => _textureData.ZeroIsTransparent;

        public byte[,] ImageData8Bit => _textureData.ImageData8Bit;
        public void SetImageData8Bit(byte[,] data, Palette palette) {
            _textureData.SetImageData8Bit(data, palette);
            StructPalette = palette;
        }

        public ushort[,] ImageData16Bit {
            get => _textureData.ImageData16Bit;
            set => _textureData.ImageData16Bit = value;
        }

        public bool CanSetImageData8Bit => _textureData.CanSetImageData8Bit && BytesPerPixel == 1 && CanLoadImage && HasImage;
        public bool CanSetImageData16Bit => _textureData.CanSetImageData16Bit && BytesPerPixel == 2 && CanLoadImage && HasImage;

        [TableViewModelColumn(addressField: null, displayOrder: 0.5f, displayFormat: "-X4")]
        public int ImageDataOffset {
            get => StructImageDataOffset;
            set {
                StructImageDataOffset = value;
                _textureData.ImageDataOffset = StructImageDataOffset;
            }
        }

        protected abstract int StructImageDataOffset { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 0.5f)]
        public int Width {
            get => StructWidth;
            set {
                StructWidth = value;
                _textureData.Width = StructWidth;
            }
        }

        protected abstract int StructWidth { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 1)]
        public int Height {
            get => StructHeight;
            set {
                StructHeight = value;
                _textureData.Height = StructHeight;
            }
        }

        protected abstract int StructHeight { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X4")]
        public int StoredImageDataSize => _textureData.StoredImageDataSize;

        [TableViewModelColumn(addressField: null, displayOrder: 3, displayFormat: "X4")]
        public int ImageDataSize => _textureData.ImageDataSize;

        [TableViewModelColumn(addressField: null, displayName: "Pixel Format", displayOrder: 4)]
        public TexturePixelFormat PixelFormat => _textureData.PixelFormat;

        [TableViewModelColumn(addressField: null, displayOrder: 5, minWidth: 225)]
        public string Hash => _textureData.Hash;

        protected abstract void OnSetImageData();

        public abstract bool HasImage { get; }
        public abstract bool CanLoadImage { get; }

        public Palette Palette {
            get => StructPalette;
            set {
                StructPalette = value;
                _textureData.Palette = StructPalette;
            }
        }

        protected abstract Palette StructPalette { get; set; }

        protected TextureData _textureData;
    }
}
