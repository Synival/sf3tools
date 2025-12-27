using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face32_Texture : FixedSizeTextureStructBase {
        private readonly int _imageDataOffsetAddr;

        public Face32_Texture(IByteData data, int id, string name, int address, Palette palette, bool isCompressed)
        : base(data, id, name, address, 4, 32, 32, TexturePixelFormat.Palette1, isCompressed, false) {
            _palette = palette;
            _imageDataOffsetAddr = address + 0;

            _textureData.Add8BitValidator((_1, _2, _3, newStoredSize) => {
                return (newStoredSize > _originalStoredSize)
                    ? $"New stored image size ({newStoredSize} / 0x{newStoredSize:X2}) cannot be larger than existing stored image size ({_originalStoredSize} / 0x{_originalStoredSize:X2})"
                    : null;
            });

            LoadImageData();
            _originalStoredSize = StoredImageDataSize;
        }

        protected override void OnSetImageData() {}

        protected override int StructImageDataOffset {
            get => Data.GetDouble(_imageDataOffsetAddr);
            set => Data.SetDouble(_imageDataOffsetAddr, value);
        }

        public override bool HasImage => ImageDataOffset != -1;
        public override bool CanLoadImage => HasImage;

        private readonly Palette _palette;
        private readonly int _originalStoredSize;
        protected override Palette StructPalette { get => _palette; set {} }
    }
}
