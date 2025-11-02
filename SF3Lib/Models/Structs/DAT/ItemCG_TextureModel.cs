using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class ItemCG_TextureModel : TextureModelBase {
        public ItemCG_TextureModel(IByteData data, int id, string name, int address, Palette palette)
        : base(data, id, name, address, 24 * 24, 24, 24, TexturePixelFormat.Palette1, palette, true, false) {
            _ = FetchAndCacheTexture();
        }

        public override int ImageDataOffset => Address;
        public override bool HasImage => true;

        [TableViewModelColumn(addressField: null, displayName: nameof(ImageDataOffset), displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetView => ImageDataOffset;
    }
}
