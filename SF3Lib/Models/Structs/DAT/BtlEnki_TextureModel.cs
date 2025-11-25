using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class BtlEnkei_TextureModel : TextureModelBase {
        private readonly int _unknown1Addr;
        private readonly int _unknown2Addr;
        private readonly int _unknown3Addr;
        private readonly int _unknown4Addr;

        public BtlEnkei_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10, 0x200, 0x100, TexturePixelFormat.Palette1, MakePalette(data, address), true, false) {
            _unknown1Addr = Address + 0x00; // 4 bytes
            _unknown2Addr = Address + 0x04; // 4 bytes
            _unknown3Addr = Address + 0x06; // 4 bytes
            _unknown4Addr = Address + 0x0C; // 4 bytes

            _ = FetchAndCacheTexture();
        }

        private static Palette MakePalette(IByteData data, int addr) {
            var paletteOffset = data.GetDouble(addr);
            if (paletteOffset == 0)
                return new Palette(256);
    
            var colors = data.GetDataCopyAt(paletteOffset, 0x200).ToUShorts();
            return new Palette(colors);
        }

        public override int ImageDataOffset => (Unknown1 == 0) ? 0 : (Unknown1 + 0x200);
        public override bool HasImage => Unknown1 != 0;

        [TableViewModelColumn(addressField: nameof(_unknown1Addr), displayOrder: 0, isPointer: true)]
        public int Unknown1 {
            get => Data.GetDouble(_unknown1Addr);
            set => Data.SetDouble(_unknown1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown2Addr), displayOrder: 1, isPointer: true)]
        public int Unknown2 {
            get => Data.GetDouble(_unknown2Addr);
            set => Data.SetDouble(_unknown2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown3Addr), displayOrder: 2, isPointer: true)]
        public int Unknown3 {
            get => Data.GetDouble(_unknown3Addr);
            set => Data.SetDouble(_unknown3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown4Addr), displayOrder: 3, isPointer: true)]
        public int Unknown4 {
            get => Data.GetDouble(_unknown4Addr);
            set => Data.SetDouble(_unknown4Addr, value);
        }

        [TableViewModelColumn(addressField: null, displayName: nameof(ImageDataOffset), displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetView => ImageDataOffset;
    }
}
