using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public class ItemCG_Texture : DAT_FileTextureBase {
        public ItemCG_Texture(IByteData data, int id, string name, int address, IPalette palette)
        : base(data, id, name, address, 24 * 24, 24, 24, TexturePixelFormat.Indexed8Bit, true, false, IndexedColorUpdateStrategy.MatchToExistingPalette) {
            _palette = palette;

            _textureData.Add8BitValidator((_1, _2, _3, newStoredSize) => {
                return (MaxStoredImageSize.HasValue && newStoredSize > MaxStoredImageSize.Value)
                    ? $"New stored image size ({newStoredSize} / 0x{newStoredSize:X2}) cannot be larger than existing stored image size ({MaxStoredImageSize.Value} / 0x{MaxStoredImageSize.Value:X2})"
                    : null;
            });

            LoadImageData();
        }

        public override bool CanUpdateAddress => true;

        [TableViewModelColumn(displayOrder: 2.1f, displayFormat: "X4", isReadOnly: true, displayGroup: "Metadata")]
        public int? MaxStoredImageSize { get; set; }

        protected override void OnImageUpdated() {}

        public override void SetImageData8Bit(byte[,] data, IPalette palette) {
            base.SetImageData8Bit(data, palette);

            // Zero-out unused data.
            var maxSize = MaxStoredImageSize ?? (Data.Length - Address);
            var remainder = maxSize - StoredImageDataSize.Value;
            if (remainder > 0)
                Data.Data.SetDataAtTo(Address + StoredImageDataSize.Value, remainder, new byte[remainder]);
        }

        public override void UpdateAddress(int address) {
            if (address != Address) {
                base.UpdateAddress(address);
                ImageDataOffset = address;
                InvalidateImage();
            }
        }

        protected override int StructImageDataOffset { get => Address; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        private readonly IPalette _palette;
        protected override IPalette StructPalette { get => _palette; set {} }
    }
}
