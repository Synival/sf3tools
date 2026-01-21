namespace SF3.MPD {
    /// <summary>
    /// Interface for settings that don't affect gameplay but are necessary for byte-for-byte reproduction of MPD files.
    /// </summary>
    public interface IMPD_BinaryReproductionFlags {
        /// <summary>
        /// When set, an empty animation table is written as 'FFFF' instead of the typical
        /// longer value. (This is likely a holdover from older MPDs.)
        /// </summary>
        bool ShortEmptyAnimationTable { get; set; }

        /// <summary>
        /// When set, Scenario 3 MPDs will have a truncated LightAdjustment struct without the
        /// extra Scenario 3 fields. This is present in a lot of Scenario 3 MPDs and may be a bug.
        /// </summary>
        bool PaletteAdjustmentIsTruncated { get; set; }

        /// <summary>
        /// When set, the sky palette occupies the same space as the ground palette.
        /// </summary>
        bool SkyPaletteSharesGroundPalette { get; set; }

        /// <summary>
        /// When set, the texture palette occupies the same space as the sky palette.
        /// </summary>
        bool TexturePaletteSharesSkyPalette { get; set; }

        /// <summary>
        /// When set, the "ignore texture" table is 0 bytes and the terminator is missing, so it's at the same address as the ground palette.
        /// This happens only in BAKA2.MPD.
        /// </summary>
        bool EmptyUnterminatedIgnoredTexturesTable { get; set; }
    }
}
