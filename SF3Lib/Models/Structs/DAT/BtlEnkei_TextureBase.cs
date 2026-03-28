using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public abstract class BtlEnkei_TextureBase : DAT_FileTextureBase {
        protected BtlEnkei_TextureBase(IByteData data, int id, string name, int address, int size, int paletteOffset)
        : base(data, id, name, address, size, 512, 256, TexturePixelFormat.Indexed8Bit, true, false, IndexedColorUpdateStrategy.UpdateExistingPalette) {
            _palette = new CachedInDataPalette(data.Data, paletteOffset, 0x100);
        }

        public abstract int PaletteOffset { get; }

        CachedInDataPalette _palette;
        protected override IPalette StructPalette {
            get => _palette;
            set => _palette.Replace(value.Colors);
        }
    }
}
