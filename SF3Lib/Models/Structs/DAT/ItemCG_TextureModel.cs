using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class ItemCG_TextureModel : TextureModelBase {
        public ItemCG_TextureModel(IByteData data, int id, string name, int address, Palette palette)
        : base(data, id, name, address, 24 * 24, 24, 24, TexturePixelFormat.Palette1, true, false) {
            _palette = palette;
            LoadImageData();
        }

        public override int ImageDataOffset => Address;
        public override bool HasImage => true;
        public override bool CanLoadImage => false;

        [TableViewModelColumn(addressField: null, displayName: nameof(ImageDataOffset), displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetView => ImageDataOffset;

        public override void OnSetImageData() {}

        private readonly Palette _palette;
        public override Palette Palette { get => _palette; protected set {} }
    }
}
