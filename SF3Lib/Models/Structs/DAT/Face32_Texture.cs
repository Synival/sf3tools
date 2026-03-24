using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public class Face32_Texture : DAT_FileTextureBase {
        private readonly int _imageDataOffsetAddr;

        public Face32_Texture(IByteData data, int id, string name, int address, Palette palette, bool isCompressed)
        : base(data, id, name, address, 4, 32, 32, TexturePixelFormat.Indexed8Bit, isCompressed, false) {
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

        public override bool CanUpdateAddress => false;

        protected override (byte[,], Palette) PreProcessIncomingImageData8Bit(byte[,] newData, Palette palette) {
            (newData, palette) = base.PreProcessIncomingImageData8Bit(newData, palette);
            return (ImageUtils.GetImageDataConformingToPalette(newData, palette, _palette), _palette);
        }

        protected override void OnImageUpdated() {}

        protected override int StructImageDataOffset {
            get => Data.GetDouble(_imageDataOffsetAddr);
            set => Data.SetDouble(_imageDataOffsetAddr, value);
        }

        public override bool HasImage => ImageDataOffset != -1;
        public override bool CanLoadImage => HasImage;

        private readonly Palette _palette;
        private readonly int? _originalStoredSize;
        protected override Palette StructPalette { get => _palette; set {} }
    }
}
