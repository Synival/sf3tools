using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public abstract class TextureStructBase : Struct, ITextureData {
        public TextureStructBase(IByteData data, IByteData imageData, int id, string name, int address, int size,
            TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent)
        : base(data, id, name, address, size) {
            _textureData = new TextureStructData(imageData, pixelFormat, isCompressed, zeroIsTransparent, this);
            _textureData.ImageDataSet += (s, e) => OnSetImageData();
        }

        public TextureStructBase(IByteData data, int id, string name, int address, int size,
            TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent)
        : this(data, data, id, name, address, size, pixelFormat, isCompressed, zeroIsTransparent) {}

        public void LoadImageData() => _textureData.LoadImageData();

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB1555(highlightEndcodes);
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB8888(highlightEndcodes);
        public void InvalidateImage() => _textureData.Invalidate();
        public string Validate8BitImageData(byte[,] data, Palette palette) => _textureData.Validate8BitImageData(data, palette);
        public string Validate16BitImageData(ushort[,] data) => _textureData.Validate16BitImageData(data);

        public byte[] BitmapDataARGB1555 => _textureData.BitmapDataARGB1555;
        public byte[] BitmapDataARGB8888 => _textureData.BitmapDataARGB8888;
        public int BytesPerPixel => _textureData.BytesPerPixel;
        public bool IsCompressed => _textureData.IsCompressed;
        public bool ZeroIsTransparent => _textureData.ZeroIsTransparent;

        public byte[,] ImageData8Bit => _textureData.ImageData8Bit;
        public void SetImageData8Bit(byte[,] data, Palette palette) => _textureData.SetImageData8Bit(data, palette);

        public ushort[,] ImageData16Bit {
            get => _textureData.ImageData16Bit;
            set => _textureData.ImageData16Bit = value;
        }

        public bool CanSetImageData8Bit => _textureData.CanSetImageData8Bit;
        public bool CanSetImageData16Bit => _textureData.CanSetImageData16Bit;

        [TableViewModelColumn(addressField: null, displayOrder: 0)]
        public abstract int Width { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 1)]
        public abstract int Height { get; set; }

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X4")]
        public int StoredImageDataSize => _textureData.StoredImageDataSize;

        [TableViewModelColumn(addressField: null, displayOrder: 3, displayFormat: "X4")]
        public int ImageDataSize => _textureData.ImageDataSize;

        [TableViewModelColumn(addressField: null, displayName: "Pixel Format", displayOrder: 4)]
        public TexturePixelFormat PixelFormat => _textureData.PixelFormat;

        [TableViewModelColumn(addressField: null, displayOrder: 5, minWidth: 225)]
        public string Hash => _textureData.Hash;

        public virtual int ImageDataOffset {
            get => _textureData.Address;
            set => _textureData.Address = value;
        }

        protected abstract void OnSetImageData();

        public abstract bool HasImage { get; }
        public abstract bool CanLoadImage { get; }
        public abstract Palette Palette { get; set; }

        protected TextureStructData _textureData;
    }
}
