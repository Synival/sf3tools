namespace CommonLib.Imaging {
    /// <summary>
    /// Abstraction of a color palette.
    /// </summary>
    public interface IPalette {
        /// <summary>
        /// The number of colors in the palette.
        /// </summary>
        int ColorCount { get; }

        /// <summary>
        /// Gets a reference to all colors stored in the palette.
        /// </summary>
        PixelChannels[] Colors { get; }

        /// <summary>
        /// Replaces all colors in the palette.
        /// </summary>
        /// <param name="colors">The new channels to replace existing colors with.</param>
        void Replace(PixelChannels[] colors);

        /// <summary>
        /// Gets or sets a color in the palette.
        /// </summary>
        /// <param name="index">Index of the color to retrieve.</param>
        /// <returns></returns>
        PixelChannels this[int index] { get; set; }
    }
}
