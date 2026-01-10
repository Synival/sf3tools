namespace CommonLib.Imaging {
    /// <summary>
    /// Interface for a (0x00, 0x1F) range RGB color.
    /// </summary>
    public interface IColorRGB555 {
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
    }
}
