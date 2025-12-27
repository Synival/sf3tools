using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class ItemCG_Texture : FixedSizeTextureStructBase {
        public ItemCG_Texture(IByteData data, int id, string name, int address, Palette palette)
        : base(data, id, name, address, 24 * 24, 24, 24, TexturePixelFormat.Palette1, true, false) {
            _palette = palette;

            _textureData.Add8BitValidator((_1, _2, _3, newStoredSize) => {
                return (newStoredSize > _originalStoredSize)
                    ? $"New stored image size ({newStoredSize} / 0x{newStoredSize:X2}) cannot be larger than existing stored image size ({_originalStoredSize} / 0x{_originalStoredSize:X2})"
                    : null;
            });

            LoadImageData();
            _originalStoredSize = StoredImageDataSize;
        }

        protected override void OnSetImageData() {}

        protected override int StructImageDataOffset { get => Address; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        private readonly Palette _palette;
        private readonly int _originalStoredSize;
        protected override Palette StructPalette { get => _palette; set {} }
    }
}
