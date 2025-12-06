namespace SF3.MPD {
    /// <summary>
    /// Collection of specific plane types used in an MPD (ground, tiled ground, battle skybox, scene skybox,
    /// background, foreground).
    /// </summary>
    public interface IMPD_Planes {
        /// <summary>
        /// Image for non-tiled ground plane. Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITexture GroundImage { get; }

        /// <summary>
        /// Tileset image used for a tiled ground plane. Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITexture GroundTileset { get; }

        /// <summary>
        /// Table of tileset indices for a tiled ground image. Must be 256x256.
        /// </summary>
        IMPD_PlaneTileAssignment GroundTileAssignment { get; }

        /// <summary>
        /// Image generated using the GroundTileset and tileset assignment data. Must be 2048x2048 and 8-bit indexed.
        /// This image cannot be directly assigned.
        /// </summary>
        ITexture GroundTiledImage { get; }

        /// <summary>
        /// Image used for the background plane (Ishahakat's room). Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITexture BackgroundImage { get; }

        /// <summary>
        /// Image used for the skybox plane. Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITexture SkyBoxImage { get; }

        /// <summary>
        /// Tileset image used for a foregound plane (Ishahakat). Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITexture ForegroundTileset { get; }

        /// <summary>
        /// Table of tileset indices for a tiled foreground image. Must be 64x32.
        /// </summary>
        IMPD_PlaneTileAssignment ForegroundTileAssignment { get; }

        /// <summary>
        /// Image generated using the ForegroundTileset and tileset assignment data. Must be 512x256 and 8-bit indexed.
        /// This image cannot be directly assigned.
        /// </summary>
        ITexture ForegroundTiledImage { get; }
    }
}
