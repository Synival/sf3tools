using CommonLib.Imaging;
using CommonLib.Types;

namespace CommonLib.SGL {
    public class ATTR : IATTR {
        public ATTR() {}

        public ATTR(IATTR attr) {
            Plane               = attr.Plane;
            SortAndOptions      = attr.SortAndOptions;
            TextureNo           = attr.TextureNo;
            Mode                = attr.Mode;
            ColorNo             = attr.ColorNo;
            GouraudShadingTable = attr.GouraudShadingTable;
            Dir                 = attr.Dir;
        }

        public byte Plane { get; set; }
        public byte SortAndOptions { get; set; }
        public ushort TextureNo { get; set; }
        public ushort Mode { get; set; }
        public ushort ColorNo { get; set; }
        public ushort GouraudShadingTable { get; set; }
        public ushort Dir { get; set; }

        public bool IsTwoSided {
            get => (Plane & 0x01) == 0x01;
            set => Plane = (byte) ((Plane & ~0x01) | (value ? 0x01 : 0));
        }

        public SortOrder Sort {
            get => (SortOrder) (SortAndOptions & 0x03);
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x03) | ((int) value & 0x03));
        }

        public bool UseTexture {
            get => (SortAndOptions & 0x04) == 0x0004;
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x04) | (value ? 0x04 : 0));
        }

        public bool UseLight {
            get => (SortAndOptions & 0x08) == 0x08;
            set => SortAndOptions = (byte) ((SortAndOptions & ~0x08) | (value ? 0x08 : 0));
        }

        public bool Mode_MSBon {
            get => (Mode & (0x01 << 15)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 15)) : (Mode & ~(1 << 15)));
        }

        public WindowMode Mode_WindowMode {
            get => (WindowMode) ((Mode & (0x03 << 9)) >> 9);
            set => Mode = (ushort) ((Mode & ~(0x03 << 9)) | (((ushort) value & 0x03) << 9));
        }

        public bool Mode_HSSon {
            get => (Mode & (0x01 << 12)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 12)) : (Mode & ~(0x01 << 12)));
        }

        public bool Mode_MESHon {
            get => (Mode & (0x01 << 8)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 8)) : (Mode & ~(0x01 << 8)));
        }

        public bool Mode_ECdis {
            get => (Mode & (0x01 << 7)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 7)) : (Mode & ~(0x01 << 7)));
        }

        public bool Mode_SPdis {
            get => (Mode & (0x01 << 6)) != 0;
            set => Mode = (ushort) (value ? (Mode | (0x01 << 6)) : (Mode & ~(0x01 << 6)));
        }

        public ColorMode Mode_ColorMode {
            get => (ColorMode) ((Mode & (0x07 << 3)) >> 3);
            set => Mode = (ushort) ((Mode & ~(0x07 << 3)) | (((ushort) value & 0x07) << 3));
        }

        public DrawMode Mode_DrawMode {
            get => (DrawMode) (Mode & 0x07);
            set => Mode = (ushort) ((Mode & ~0x07) | ((ushort) value & 0x07));
        }

        public bool CL_Gouraud {
            get => Mode_DrawMode.HasFlag(DrawMode.CL_Gouraud);
            set => Mode_DrawMode = value ? (Mode_DrawMode | DrawMode.CL_Gouraud) : (Mode_DrawMode & ~DrawMode.CL_Gouraud);
        }

        public string HtmlColor {
            get => PixelConversion.ABGR1555toChannels(ColorNo).ToHtmlColor();
            set {
                if (!PixelConversion.IsValidHtmlColor(value))
                    return;
                ColorNo = PixelChannels.FromHtmlColor(value, 255).ToABGR1555();
            }
        }

        public bool HFlip {
            get => (Dir & 0x10) == 0x10;
            set => Dir = (byte) ((Dir & ~0x10) | (value ? 0x10 : 0));
        }

        public bool VFlip {
            get => (Dir & 0x20) == 0x20;
            set => Dir = (byte) ((Dir & ~0x20) | (value ? 0x20 : 0));
        }
    }
}
