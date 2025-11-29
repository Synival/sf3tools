namespace SF3.MPD {
    public interface IMPD_AllScenarioFlags {
        /// <summary>
        /// Always on for all MPDs across all four scenarios. Not known if this is read anywhere and must be set.
        /// </summary>
        bool Bit_0x0001_Unknown { get; set; }

        /// <summary>
        /// When set, the first bit of the lightmap index to use is toggled by pseudo-random values taken from certain
        /// bits in the dot product produced during lighting calculations.
        /// </summary>
        bool Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap { get; set; }

        /// <summary>
        /// When set, flat surface tiles without a texture (set to 0xFF) aren't discarded but added to the set of 150
        /// flat tiles. This is so they can be rendered in battle with the grid or the "move" or "attack" masks.
        /// (Potentially exists in Scenario 1 but unused)
        /// </summary>
        bool Bit_0x0008_KeepTexturelessFlatTiles { get; set; }

        /// <summary>
        /// When set, there are three chunks in total that are:
        ///   Foreground image tileset (2 chunks)
        ///   Fileground image tile assignment (1 chunk)
        /// This is used in BLACK.MPD (Ishahakat). This uses the same chunks as the ground plane; they are mutually
        /// exclusive.
        /// </summary>
        bool Bit_0x0010_HasTileBasedForegroundImage { get; set; }

        /// <summary>
        /// Is set sometimes in Scenario 2 and 3 but appears to do nothing (no breakpoints hit).
        /// (Potentially exists in Scenario 1 but unused)
        /// </summary>
        bool Bit_0x0020_Unknown { get; set; }

        /// <summary>
        /// When set, there are two chunks that, together, form a 512x256 image used as a background. Used in BLACK.MPD
        /// (Ishahakat). This uses the same chunks as the ground plane; they are mutually exclusive.
        /// </summary>
        bool Bit_0x0040_HasBackgroundImage { get; set; }

        /// <summary>
        /// When set, models are present in this MPD. This can be toggled during gameplay to prevent the rendering of
        /// models.
        /// </summary>
        bool Bit_0x0100_HasModels { get; set; }

        /// <summary>
        /// When set, the surface model is present in this MPD. This can be toggled during gameplay to prevent the
        /// rendering of the surface model.
        /// </summary>
        bool Bit_0x0200_HasSurfaceModel { get; set; }
    }
}
