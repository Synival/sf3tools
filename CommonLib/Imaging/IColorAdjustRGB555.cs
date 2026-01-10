namespace CommonLib.Imaging {
    /// <summary>
    /// Interface for a (-0x1F, 0x1F) range RGB color.
    /// </summary>
    public interface IColorAdjustRGB555 {
        sbyte R { get; set; }
        sbyte G { get; set; }
        sbyte B { get; set; }
    }
}
