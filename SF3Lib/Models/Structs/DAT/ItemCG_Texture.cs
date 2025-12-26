using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class ItemCG_Texture : FixedSizeTextureStructBase {
        public ItemCG_Texture(IByteData data, int id, string name, int address, Palette palette)
        : base(data, id, name, address, 24 * 24, 24, 24, TexturePixelFormat.Palette1, true, false) {
            _palette = palette;
            LoadImageData();
        }

        protected override void OnSetImageData() {}

        protected override int StructImageDataOffset { get => Address; set {} }
        public override bool HasImage => true;
        public override bool CanLoadImage => false;

        private readonly Palette _palette;
        protected override Palette StructPalette { get => _palette; set {} }
    }
}
