using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class AttrModel : Struct {
        public int _flagAddr;                // (Uint8) Single-sided or double-sided flag
        public int _sortAddr;                // (Uint8) Sort reference position and option settings
        public int _textureNoAddr;           // (Uint16) Texture number
        public int _displayModeAddr;         // (Uint16) Attribute data (display mode)
        public int _colorNoAddr;             // (Uint16) Color number
        public int _gouraudShadingTableAddr; // (Uint16) Gouraud shading table
        public int _dirAddr;                 // (Uint16) Texture inversion and function number

        public AttrModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _flagAddr                = Address + 0x00; // 1 byte
            _sortAddr                = Address + 0x01; // 1 byte
            _textureNoAddr           = Address + 0x02; // 2 byte
            _displayModeAddr         = Address + 0x04; // 2 byte
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
        public ushort DisplayMode {
            get => (ushort) Data.GetWord(_displayModeAddr);
            set => Data.SetWord(_displayModeAddr, value);
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
