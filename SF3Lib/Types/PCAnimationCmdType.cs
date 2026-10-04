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
        ShieldOn           = 0x16,
        ShieldOff          = 0x17,
        GlowingEyesOn      = 0x18,
        GlowingEyesOff     = 0x19,
        HandImpactOn       = 0x1a,
        HandImpactOff      = 0x1b,
        MuzzleFlashOn      = 0x1c,
        MuzzleFlashOff     = 0x1d,
    }
}
