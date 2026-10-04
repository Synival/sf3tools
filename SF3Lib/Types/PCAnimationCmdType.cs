namespace SF3.Types {
    public enum PCAnimationCmdType {
        HoldStart          = 0x00,
        HoldEnd            = 0x01,
        WeaponSwooshOn     = 0x02,
        WeaponSwooshOff    = 0x03,
        SpecialFreezeStart = 0x04,
        SpecialFreezeEnd   = 0x05,
        Hit                = 0x06,
        DustPoof           = 0x07,
        StopIfDead         = 0x08,
        UnusedHold         = 0x0a,
        Color1On           = 0x16,
        Color1Off          = 0x17,
        Color2On           = 0x18,
        Color2Off          = 0x19,
        MaybeGlowyEyesOn   = 0x1a,
        MaybeGlowyEyesOff  = 0x1b,
        MuzzleFlashOn      = 0x1c,
        MuzzleFlashOff     = 0x1d,
    }
}
