using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class BtlEnkei_TextureModel : TextureModelBase {
        private readonly int _paletteImageOffsetAddr;
        private readonly int _paletteImageSizeAddr;
        private readonly int _loadSizeAddr;
        private readonly int _paddingAddr;

        public BtlEnkei_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10, 512, 256, TexturePixelFormat.Palette1,
               MakePalette(data, address), true, false
        ) {
            _paletteImageOffsetAddr = Address + 0x00; // 4 bytes
            _paletteImageSizeAddr   = Address + 0x04; // 4 bytes
            _loadSizeAddr           = Address + 0x08; // 4 bytes
            _paddingAddr            = Address + 0x0C; // 4 bytes

            _ = FetchAndCacheTexture();
        }

        private static Palette MakePalette(IByteData data, int addr) {
            var paletteOffset = data.GetDouble(addr);
            if (paletteOffset == 0)
                return new Palette(256);
    
            var colors = data.GetDataCopyAt(paletteOffset, 0x200).ToUShorts();
            return new Palette(colors);
        }

        public override int ImageDataOffset => HasImage ? (PaletteImageOffset + 0x200) : 0;
        public override bool HasImage => PaletteImageOffset != 0;

        [TableViewModelColumn(addressField: nameof(_paletteImageOffsetAddr), displayOrder: -0.5f, isPointer: true)]
        public int PaletteImageOffset {
            get => Data.GetDouble(_paletteImageOffsetAddr);
            set => Data.SetDouble(_paletteImageOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paletteImageSizeAddr), displayOrder: -0.4f, displayFormat: "X4")]
        public int PaletteImageSize {
            get => Data.GetDouble(_paletteImageSizeAddr);
            set => Data.SetDouble(_paletteImageSizeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_loadSizeAddr), displayOrder: -0.3f, displayFormat: "X4")]
        public int LoadSize {
            get => Data.GetDouble(_loadSizeAddr);
            set => Data.SetDouble(_loadSizeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paddingAddr), displayOrder: -0.2f, displayFormat: "X4")]
        public int Padding {
            get => Data.GetDouble(_paddingAddr);
            set => Data.SetDouble(_paddingAddr, value);
        }
    }
}
