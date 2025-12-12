using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face64_TextureModel : TextureStructBase {
        public Face64_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 64 * 64 * 2, 64, 64, TexturePixelFormat.ABGR1555, false, false) {
            LoadImageData();
        }

        public override int ImageDataOffset => Address;
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetViewable => ImageDataOffset;

        public override void OnSetImageData() {}

        public override Palette Palette { get => null; protected set {} }
    }
}
