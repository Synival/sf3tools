using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class BtlEnkeiHeaderless_TextureModel : TextureModelBase {
        public BtlEnkeiHeaderless_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0, 512, 256, TexturePixelFormat.Palette1,
               MakePalette(data, address), true, false
        ) {}

        private static Palette MakePalette(IByteData data, int addr) {
            var colors = data.GetDataCopyAt(addr, 0x200).ToUShorts();
            return new Palette(colors);
        }

        public override int ImageDataOffset => Address + 0x200;
        public override bool HasImage => true;
    }
}
