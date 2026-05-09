using CommonLib.Attributes;

namespace SF3.Types {
    /// <summary>
    /// Types of bonus stats.
    /// </summary>
    public enum StatUpType {
        [EnumDisplayName("None")]        None       = 0x00,
        [EnumDisplayName("Atk")]         Atk        = 0x01,
        [EnumDisplayName("Def")]         Def        = 0x02,
        [EnumDisplayName("Agi")]         Agi        = 0x03,
        [EnumDisplayName("Mov")]         Mov        = 0x04,
        [EnumDisplayName("Luck")]        Luck       = 0x05,
        [EnumDisplayName("Turns")]       Turns      = 0x06,
        [EnumDisplayName("HP Regen")]    HPRegen    = 0x07,
        [EnumDisplayName("MP Regen")]    MPRegen    = 0x08,
        [EnumDisplayName("Earth Res")]   EarthRes   = 0x09,
        [EnumDisplayName("Fire Res")]    FireRes    = 0x0A,
        [EnumDisplayName("Ice Res")]     IceRes     = 0x0B,
        [EnumDisplayName("Spark Res")]   SparkRes   = 0x0C,
        [EnumDisplayName("Wind Res")]    WindRes    = 0x0D,
        [EnumDisplayName("Light Res")]   LightRes   = 0x0E,
        [EnumDisplayName("Dark Res")]    DarkRes    = 0x0F,
        [EnumDisplayName("Unknown Res")] UnknownRes = 0x10,
        [EnumDisplayName("Spell")]       Spell      = 0x11,
        [EnumDisplayName("Special")]     Special    = 0x12,
        [EnumDisplayName("Crit")]        Crit       = 0x13,
        [EnumDisplayName("Count")]       Count      = 0x14,
        [EnumDisplayName("CurseRes")]    CurseRes   = 0x15,
    }
}
