using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public class Face64_Texture : DAT_FileTextureBase {
        public Face64_Texture(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 64 * 64 * 2, 64, 64, TexturePixelFormat.ABGR1555, false, false, IndexedColorUpdateStrategy.DontUpdate) {
            LoadImageData();
        }

        public override bool CanUpdateAddress => false;

        protected override void OnImageUpdated() {}

        protected override int StructImageDataOffset { get => Address; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        protected override IPalette StructPalette { get => null; set {} }
    }
}
