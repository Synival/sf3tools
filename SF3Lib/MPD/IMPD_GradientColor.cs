namespace SF3.MPD {
    /// <summary>
    /// Interface for a (0x00, 0x1F) range color for the gradient top or bottom.
    /// </summary>
    public interface IMPD_GradientColor {
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
    }
}
