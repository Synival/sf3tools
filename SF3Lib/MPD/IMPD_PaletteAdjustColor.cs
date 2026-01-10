namespace SF3.MPD {
    /// <summary>
    /// Interface for a (-0x1F, 0x1F) range color adjust to the palette.
    /// </summary>
    public interface IMPD_PaletteAdjustColor {
        sbyte R { get; set; }
        sbyte G { get; set; }
        sbyte B { get; set; }
    }
}
