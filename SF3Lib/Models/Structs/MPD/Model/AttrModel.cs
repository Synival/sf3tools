using CommonLib.Attributes;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class AttrModel : Struct {
        public int _flagAddr;                // (Uint8) Single-sided or double-sided flag
        public int _sortAddr;                // (Uint8) Sort reference position and option settings
        public int _textureNoAddr;           // (Uint16) Texture number
        public int _modeAddr;                // (Uint16) Mode (bitset)
        public int _colorNoAddr;             // (Uint16) Color number
        public int _gouraudShadingTableAddr; // (Uint16) Gouraud shading table
        public int _dirAddr;                 // (Uint16) Texture inversion and function number

        public AttrModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _flagAddr                = Address + 0x00; // 1 byte
            _sortAddr                = Address + 0x01; // 1 byte
            _textureNoAddr           = Address + 0x02; // 2 byte
            _modeAddr                = Address + 0x04; // 2 byte
            _colorNoAddr             = Address + 0x06; // 2 byte
            _gouraudShadingTableAddr = Address + 0x08; // 2 byte
            _dirAddr                 = Address + 0x0A; // 2 byte
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public byte Flag {
            get => (byte) Data.GetByte(_flagAddr);
            set => Data.SetByte(_flagAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        public byte Sort {
            get => (byte) Data.GetByte(_sortAddr);
            set => Data.SetByte(_sortAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public ushort TextureNo {
            get => (ushort) Data.GetWord(_textureNoAddr);
            set => Data.SetWord(_textureNoAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, displayFormat: "X4")]
        public ushort Mode {
            get => (ushort) Data.GetWord(_modeAddr);
            set => Data.SetWord(_modeAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3.1f, displayName: "MSBon")]
        public bool Mode_MSBon {
            get => (Mode & (0x01 << 15)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 15)) : (Mode & ~(1 << 15)));
        }

        [TableViewModelColumn(displayOrder: 3.2f, displayName: "WindowMode")]
        public WindowMode Mode_WindowMode {
            get => (WindowMode) ((Mode & (0x03 << 9)) >> 9);
            set => Mode = (ushort) ((Mode & ~(0x03 << 9)) | (((ushort) value & 0x03) << 9));
        }

        [TableViewModelColumn(displayOrder: 3.3f, displayName: "HSSon")]
        public bool Mode_HSSon {
            get => (Mode & (0x01 << 12)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 12)) : (Mode & ~(0x01 << 12)));
        }

        [TableViewModelColumn(displayOrder: 3.4f, displayName: "MESHon")]
        public bool Mode_MESHon {
            get => (Mode & (0x01 << 8)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 8)) : (Mode & ~(0x01 << 8)));
        }

        [TableViewModelColumn(displayOrder: 3.4f, displayName: "ECdis")]
        public bool Mode_ECdis {
            get => (Mode & (0x01 << 7)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 7)) : (Mode & ~(0x01 << 7)));
        }

        [TableViewModelColumn(displayOrder: 3.5f, displayName: "SPdis")]
        public bool Mode_SPdis {
            get => (Mode & (0x01 << 6)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 6)) : (Mode & ~(0x01 << 6)));
        }

        [TableViewModelColumn(displayOrder: 3.6f, displayName: "ColorMode")]
        public ColorMode Mode_ColorMode {
            get => (ColorMode) ((Mode & (0x07 << 3)) >> 3);
            set => Mode = (ushort) ((Mode & ~(0x07 << 3)) | (((ushort) value & 0x07) << 3));
        }

        [TableViewModelColumn(displayOrder: 3.7f, displayName: "DrawMode")]
        public DrawMode Mode_DrawMode {
            get => (DrawMode) (Mode & 0x07);
            set => Mode = (ushort) ((Mode & ~0x07) | ((ushort) value & 0x07));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public ushort ColorNo {
            get => (ushort) Data.GetWord(_colorNoAddr);
            set => Data.SetWord(_colorNoAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 5, displayFormat: "X4")]
        public ushort GouraudShadingTable {
            get => (ushort) Data.GetWord(_gouraudShadingTableAddr);
            set => Data.SetWord(_gouraudShadingTableAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 6, displayFormat: "X4")]
        public ushort Dir {
            get => (ushort) Data.GetWord(_dirAddr);
            set => Data.SetWord(_dirAddr, value);
        }
    }
}
