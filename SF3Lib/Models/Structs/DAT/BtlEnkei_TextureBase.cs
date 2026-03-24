using System.Linq;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public abstract class BtlEnkei_TextureBase : DAT_FileTextureBase {
        protected BtlEnkei_TextureBase(IByteData data, int id, string name, int address, int size, int paletteOffset)
        : base(data, id, name, address, size, 512, 256, TexturePixelFormat.Indexed8Bit, true, false) {}

        public abstract int PaletteOffset { get; }

        protected override Palette StructPalette {
            get {
                var colors = Data.GetDataCopyAt(PaletteOffset, 0x200).ToUShorts();
                return new Palette(colors);
            }
            set {
                var newColors = value.Colors.Select(x => x.ToABGR1555()).ToArray();
                int pos = PaletteOffset;
                var newPaletteData = new byte[0x200];
                for (int inPos = 0, outPos = 0; inPos < 0x100 && inPos < newColors.Length; inPos++) {
                    newPaletteData[outPos++] = (byte) (newColors[inPos] >> 8);
                    newPaletteData[outPos++] = (byte) newColors[inPos];
                }
                Data.Data.SetDataAtTo(PaletteOffset, 0x200, newPaletteData);
            }
        }
    }
}
