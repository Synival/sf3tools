using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class AttrModel : Struct {
        public int _planeAddr;               // (Uint8) Single/double-sided flag
        public int _sortAndOptionsAddr;      // (Uint8) Options for lighting, has texture, sorting
        public int _textureNoAddr;           // (Uint16) Texture number
        public int _modeAddr;                // (Uint16) Mode (bitset)
        public int _colorNoAddr;             // (Uint16) Color number
        public int _gouraudShadingTableAddr; // (Uint16) Gouraud shading table
        public int _dirAddr;                 // (Uint16) Texture inversion and function number

        public AttrModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _planeAddr               = Address + 0x00; // 1 byte
            _sortAndOptionsAddr      = Address + 0x01; // 1 byte
            _textureNoAddr           = Address + 0x02; // 2 bytes
            _modeAddr                = Address + 0x04; // 2 bytes
            _colorNoAddr             = Address + 0x06; // 2 bytes
            _gouraudShadingTableAddr = Address + 0x08; // 2 bytes
            _dirAddr                 = Address + 0x0A; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public byte Plane {
            get => (byte) Data.GetByte(_planeAddr);
            set => Data.SetByte(_planeAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.01f)]
        public bool IsTwoSided {
            get => (Plane & 0x01) == 0x01;
            set => Plane = (byte) ((Plane & ~0x01) | (value ? 0x01 : 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.04f, displayFormat: "X2")]
        public byte SortAndOptions {
            get => (byte) Data.GetByte(_sortAndOptionsAddr);
            set => Data.SetByte(_sortAndOptionsAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.05f)]
        public bool HasTexture {
            get => (SortAndOptions & 0x04) == 0x0004;
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x04) | (value ? 0x04 : 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.1f)]
        public bool ApplyLighting {
            get => (SortAndOptions & 0x08) == 0x08;
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x08) | (value ? 0x08 : 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.2f)]
        public bool FlipH {
            get => (SortAndOptions & 0x10) == 0x10;
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x10) | (value ? 0x10 : 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.3f)]
        public bool FlipV {
            get => (SortAndOptions & 0x20) == 0x20;
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x20) | (value ? 0x20 : 0));
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

        [TableViewModelColumn(displayOrder: 3.8f)]
        public bool CL_Gouraud {
            get => Mode_DrawMode.HasFlag(DrawMode.CL_Gouraud);
            set => Mode_DrawMode = value ? (Mode_DrawMode | DrawMode.CL_Gouraud) : (Mode_DrawMode & ~DrawMode.CL_Gouraud);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public ushort ColorNo {
            get => (ushort) Data.GetWord(_colorNoAddr);
            set => Data.SetWord(_colorNoAddr, value);
        }

        [TableViewModelColumn(displayName: "HTML Color", displayOrder: 4.5f, displayFormat: "X", minWidth: 80)]
        public string HtmlColor {
            get => PixelConversion.ABGR1555toChannels((ushort) Data.GetWord(_colorNoAddr)).ToHtmlColor();
            set {
                if (!PixelConversion.IsValidHtmlColor(value))
                    return;
                ColorNo = PixelChannels.FromHtmlColor(value, 255).ToABGR1555();
            }
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
