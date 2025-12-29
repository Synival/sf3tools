using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face64_Texture : FixedSizeTextureStructBase {
        public Face64_Texture(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 64 * 64 * 2, 64, 64, TexturePixelFormat.ABGR1555, false, false) {
            LoadImageData();
        }

        protected override void OnImageUpdated() {}

        protected override int StructImageDataOffset { get => Address; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        protected override Palette StructPalette { get => null; set {} }
    }
}
