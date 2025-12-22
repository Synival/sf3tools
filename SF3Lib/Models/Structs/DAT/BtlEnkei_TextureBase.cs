using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public abstract class BtlEnkei_TextureBase : FixedSizeTextureStructBase {
        protected BtlEnkei_TextureBase(IByteData data, int id, string name, int address, int size, int paletteOffset)
        : base(data, id, name, address, size, 512, 256, TexturePixelFormat.Palette1, true, false) {}

        public abstract int PaletteOffset { get; }

        public override Palette Palette {
            get {
                var colors = Data.GetDataCopyAt(PaletteOffset, 0x200).ToUShorts();
                return new Palette(colors);
            }
            protected set {
                var newColors = value.Channels.Select(x => x.ToABGR1555()).ToArray();
                int pos = PaletteOffset;
                for (int i = 0; i < 0x100; i++) {
                    Data.SetWord(pos, (i < newColors.Length) ? newColors[i] : 0x0000);
                    pos += 2;
                }
            }
        }
    }
}
