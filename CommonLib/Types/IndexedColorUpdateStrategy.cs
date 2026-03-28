namespace CommonLib.Types {
    public enum IndexedColorUpdateStrategy {
        /// <summary>
        /// Doesn't update either 8-bit data to match the palette or update the palette.
        /// </summary>
        DontUpdate,

        /// <summary>
        /// Updates data to match the existing color palette.
        /// </summary>
        MatchToExistingPalette,

        /// <summary>
        /// Updates the palette to the palette provided by the input.
        /// </summary>
        UpdateExistingPalette
    }
}
