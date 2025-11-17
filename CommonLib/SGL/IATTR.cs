using CommonLib.Types;

namespace CommonLib.SGL {
    public interface IATTR {
        // Raw data
        byte Plane { get; set; }
        byte SortAndOptions { get; set; }
        ushort TextureNo { get; set; }
        ushort Mode { get; set; }
        ushort ColorNo { get; set; }
        ushort GouraudShadingTable { get; set; }
        ushort Dir { get; set; }

        // 'Plane' flags
        bool IsTwoSided { get; set; }

        // 'SortAndOptions' flags
        SortOrder Sort { get; set; }
        bool UseTexture { get; set; }
        bool UseLight { get; set; }

        // 'Mode' flags
        bool Mode_MSBon { get; set; }
        WindowMode Mode_WindowMode { get; set; }
        bool Mode_HSSon { get; set; }
        bool Mode_MESHon { get; set; }
        bool Mode_ECdis { get; set; }
        bool Mode_SPdis { get; set; }
        ColorMode Mode_ColorMode { get; set; }
        DrawMode Mode_DrawMode { get; set; }
        bool CL_Gouraud { get; set; }

        // 'ColorNo' in HTML color format.
        string HtmlColor { get; set; }

        // 'Dir' flags.
        bool HFlip { get; set; }
        bool VFlip { get; set; }
    }
}
