using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public class BtlEnkeiHeaderless_TextureModel : BtlEnkei_TextureModelBase {
        public BtlEnkeiHeaderless_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0, address, true)
        {}

        public override int ImageDataOffset => Address + 0x200;
        public override bool HasImage => true;
        public override bool CanLoadImage => true;
        public override int PaletteOffset => Address;
    }
}
