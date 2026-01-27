using CommonLib.Imaging;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Collection of specific plane types used in an MPD (ground, tiled ground, battle sky, scene sky,
    /// background, foreground).
    /// </summary>
    public interface IMPD_Planes {
        /// <summary>
        /// Image for non-tiled ground plane. Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITextureData GroundImage { get; }

        /// <summary>
        /// Tiled image used for ground plane. Typically used for towns. Resulting image is 2048x2048 with a 256x256
        /// tile assignment map.
        /// </summary>
        IMPD_TiledPlane GroundTiledImage { get; }

        /// <summary>
        /// Image used for the background plane (Ishahakat's room). Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITextureData BackgroundImage { get; }

        /// <summary>
        /// Image used for the sky plane. Must be 512x256 and 8-bit indexed.
        /// </summary>
        ITextureData SkyImage { get; }

        /// <summary>
        /// Tiled image used for foregrounds (Ishahakat). Resulting image is 512x256 with a 64x32 tile assignment map.
        /// </summary>
        IMPD_TiledPlane ForegroundTiledImage { get; }

        /// <summary>
        /// Scene X position of the ground plane (in pixels), repeating every 512 for normal images and every 2048
        /// (the full size of the surface grid) for tile-based images.
        /// </summary>
        short GroundX { get; set; }

        /// <summary>
        /// Scene Y position of the ground plane.
        /// </summary>
        short GroundY { get; set; }

        /// <summary>
        /// Scene Z position of the ground plane (in pixels), repeating every 256 for nomal images and every 2048
        /// (the full size of the surface grid) for tile-based images.
        /// </summary>
        short GroundZ { get; set; }

        /// <summary>
        /// X-axis rotation of the ground plane (in degrees). Usually -90, but is 0 in Scenario 3 TODI00.MPD.
        /// </summary>
        float GroundXRotation { get; set; }

        /// <summary>
        /// Screen X position of the sky (in pixels), repeating every 512.
        /// </summary>
        short BackgroundX { get; set; }

        /// <summary>
        /// Screen Y position of the sky (in pixels), repeating every 256.
        /// </summary>
        short BackgroundY { get; set; }

        /// <summary>
        /// Palette used for ground and background planes.
        /// </summary>
        Palette GroundPalette { get; }

        /// <summary>
        /// Palette used for sky and foreground planes.
        /// </summary>
        Palette SkyPalette { get; }
    }
}
