using CommonLib;

namespace SF3.MPD.Interfaces {
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

        /// <summary>
        /// When set, this is the maximum size a texture chunk can be before textures are placed in the next chunk.
        /// Occurs in BTL42 and MITI00.
        /// </summary>
        int? NonStandardTextureChunkDecompressedSizeLimit { get; set; }

        /// <summary>
        /// When set, this is the index the models chunk can be found in, which is unexpected from logic.
        /// </summary>
        int? MisplacedModelsChunkIndex { get; set; }

        /// <summary>
        /// When set, this is the index the surface model chunk can be found in, which is unexpected from logic.
        /// </summary>
        int? MisplacedSurfaceModelChunkIndex { get; set; }

        /// <summary>
        /// When set, this junk data is written after the "palette adjustment" table.
        /// </summary>
        IIndexedEnumerableWithLength<byte> UnreferencedDataAfterPaletteAdjustmentTable { get; set; }
    }
}
