using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public class BtlEnkeiHeaderless_Texture : BtlEnkei_TextureBase {
        public BtlEnkeiHeaderless_Texture(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0, address) {
            LoadImageData();
        }

        protected override void OnSetImageData() {}

        protected override int StructImageDataOffset { get => Address + 0x200; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => true;
        public override int PaletteOffset => Address;
    }
}
