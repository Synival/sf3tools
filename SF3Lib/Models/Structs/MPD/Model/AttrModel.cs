﻿using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class AttrModel : Struct {
        public int _flagsAddr;               // (Uint16) Single/double-sided flag, and other flags
        public int _textureNoAddr;           // (Uint16) Texture number
        public int _modeAddr;                // (Uint16) Mode (bitset)
        public int _colorNoAddr;             // (Uint16) Color number
        public int _gouraudShadingTableAddr; // (Uint16) Gouraud shading table
        public int _dirAddr;                 // (Uint16) Texture inversion and function number

        public AttrModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _flagsAddr               = Address + 0x00; // 2 bytes
            _textureNoAddr           = Address + 0x02; // 2 bytes
            _modeAddr                = Address + 0x04; // 2 bytes
            _colorNoAddr             = Address + 0x06; // 2 bytes
            _gouraudShadingTableAddr = Address + 0x08; // 2 bytes
            _dirAddr                 = Address + 0x0A; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        public ushort Flags {
            get => (ushort) Data.GetWord(_flagsAddr);
            set => Data.SetWord(_flagsAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.05f)]
        public bool HasTexture {
            get => (Flags & 0x0004) == 0x0004;
            set => Flags = (ushort) ((Flags & ~0x0004) | (value ? 0x0004 : 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.1f)]
        public bool ApplyLighting {
            get => (Flags & 0x0008) == 0x0008;
            set => Flags = (ushort) ((Flags & ~0x0008) | (value ? 0x0008 : 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.2f)]
        public bool TwoSided {
            get => (Flags & 0x0100) == 0x0100;
            set => Flags = (ushort) ((Flags & ~0x0100) | (value ? 0x0100 : 0));
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
