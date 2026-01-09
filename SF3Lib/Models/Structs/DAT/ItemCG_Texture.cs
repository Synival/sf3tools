using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Structs.DAT {
    public class ItemCG_Texture : FixedSizeTextureStructBase {
        public ItemCG_Texture(IByteData data, int id, string name, int address, Palette palette)
        : base(data, id, name, address, 24 * 24, 24, 24, TexturePixelFormat.Indexed8Bit, true, false) {
            _palette = palette;

            _textureData.Add8BitValidator((_1, _2, _3, newStoredSize) => {
                return (MaxStoredImageSize.HasValue && newStoredSize > MaxStoredImageSize.Value)
                    ? $"New stored image size ({newStoredSize} / 0x{newStoredSize:X2}) cannot be larger than existing stored image size ({MaxStoredImageSize.Value} / 0x{MaxStoredImageSize.Value:X2})"
                    : null;
            });

            LoadImageData();
        }

        [TableViewModelColumn(displayOrder: 2.1f, displayFormat: "X4", isReadOnly: true, displayGroup: "Metadata")]
        public int? MaxStoredImageSize { get; set; }

        protected override (byte[,], Palette) PreProcessIncomingImageData8Bit(byte[,] newData, Palette palette) {
            (newData, palette) = base.PreProcessIncomingImageData8Bit(newData, palette);
            return (ImageUtils.GetImageDataConformingToPalette(newData, palette, _palette), _palette);
        }

        protected override void OnImageUpdated() {}

        protected override int StructImageDataOffset { get => Address; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        private readonly Palette _palette;
        protected override Palette StructPalette { get => _palette; set {} }
    }
}
