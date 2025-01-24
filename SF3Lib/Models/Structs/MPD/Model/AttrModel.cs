using System;
using CommonLib.Attributes;
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
        public bool Mode_MSBon => (Mode & (1 << 15)) != 0;

        [TableViewModelColumn(displayOrder: 3.2f, displayName: "WindowMode")]
        public string Mode_WindowMode {
            get {
                var windowMode = (Mode & (3 << 9)) >> 9;
                switch (windowMode) {
                    case 0: return "No_Window";
                    case 1: return "2 (invalid)";
                    case 2: return "Window_In";
                    case 3: return "Window_Out";
                    default: throw new InvalidOperationException("Unreachable code");
                }
            }
        }

        [TableViewModelColumn(displayOrder: 3.3f, displayName: "HSSon")]
        public bool Mode_HSSon => (Mode & (1 << 12)) != 0;

        [TableViewModelColumn(displayOrder: 3.4f, displayName: "MESHon")]
        public bool Mode_MESHon => (Mode & (1 << 8)) != 0;

        [TableViewModelColumn(displayOrder: 3.4f, displayName: "ECdis")]
        public bool Mode_ECdis => (Mode & (1 << 7)) != 0;

        [TableViewModelColumn(displayOrder: 3.5f, displayName: "SPdis")]
        public bool Mode_SPdis => (Mode & (1 << 6)) != 0;

        [TableViewModelColumn(displayOrder: 3.6f, displayName: "ColorMode")]
        public string Mode_ColorMode {
            get {
                var colorMode = (Mode & (0x07 << 3)) >> 3;
                switch (colorMode) {
                    case 0: return "CL16Bnk";
                    case 1: return "CL16Look";
                    case 2: return "CL64Bnk";
                    case 3: return "CL128Bnk";
                    case 4: return "CL256Bnk";
                    case 5: return "CL32KRGB";
                    case 6: return "6 (invalid)";
                    case 7: return "7 (invalid)";
                    default: throw new InvalidOperationException("Unreachable code");
                }
            }
        }

        [TableViewModelColumn(displayOrder: 3.7f, displayName: "DrawMode")]
        public string Mode_DrawMode {
            get {
                var drawMode = Mode & 0x07;
                switch (drawMode) {
                    case 0: return "CL_Replace";
                    case 1: return "CL_Shadow";
                    case 2: return "CL_Half";
                    case 3: return "CL_Trans";
                    case 4: return "CL_Gouraud";
                    case 5: return "5 (invalid)";
                    case 6: return "6 (invalid)";
                    case 7: return "7 (invalid)";
                    default: throw new InvalidOperationException("Unreachable code");
                }
            }
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
