using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face64_TextureModel : TextureModelBase {
        public Face64_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 64 * 64 * 2, 64, 64, TexturePixelFormat.ABGR1555, null, false, false) {
            _ = FetchAndCacheTexture();
        }

        public override int ImageDataOffset => Address;
        public override bool HasImage => true;

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetViewable => ImageDataOffset;
    }
}
