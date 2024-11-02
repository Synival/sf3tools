namespace SF3.Types {
    /// <summary>
    /// Types of bonus stats.
    /// </summary>
    public enum StatUpType {
        None          = 0x00,
        Atk           = 0x01,
        Def           = 0x02,
        Agi           = 0x03,
        Mov           = 0x04,
        Luck          = 0x05,
        Turns         = 0x06,
        HPRegen       = 0x07,
        MPRegen       = 0x08,
        EarthRes      = 0x09,
        FireRes       = 0x0A,
        IceRes        = 0x0B,
        SparkRes      = 0x0C,
        WindRes       = 0x0D,
        LightRes      = 0x0E,
        DarkRes       = 0x0F,
        UnknownRes    = 0x10,
        Spell         = 0x11,
        Special       = 0x12,
        Crit          = 0x13,
        Count         = 0x14,
        CurseRes      = 0x15,
    }
}
