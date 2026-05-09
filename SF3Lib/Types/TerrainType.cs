using CommonLib.Attributes;

namespace SF3.Types {
    public enum TerrainType : byte {
        [EnumDisplayName("No Entry")]
        NoEntry       = 0x00,

        [EnumDisplayName("Air")]
        Air           = 0x01,

        [EnumDisplayName("Grassland")]
        Grassland     = 0x02,

        [EnumDisplayName("Dirt")]
        Dirt          = 0x03,

        [EnumDisplayName("Dark Grass")]
        DarkGrass     = 0x04,

        [EnumDisplayName("")]
        Forest        = 0x05,

        [EnumDisplayName("Brown Mountain")]
        BrownMountain = 0x06,

        [EnumDisplayName("Desert")]
        Desert        = 0x07,

        [EnumDisplayName("Grey Mountain")]
        GreyMountain  = 0x08,

        [EnumDisplayName("Water")]
        Water         = 0x09,

        [EnumDisplayName("Can't Stay")]
        CantStay      = 0x0A,

        [EnumDisplayName("Sand")]
        Sand          = 0x0B,

        [EnumDisplayName("Enemy Only")]
        EnemyOnly     = 0x0C,

        [EnumDisplayName("Player Only")]
        PlayerOnly    = 0x0D,

        [EnumDisplayName("Extra (0x0E)")]
        Extra0x0E      = 0x0E,

        [EnumDisplayName("Extra (0x0F)")]
        Extra0x0F      = 0x0F,
    }
}
