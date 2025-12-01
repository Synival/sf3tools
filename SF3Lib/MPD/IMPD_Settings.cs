namespace SF3.MPD {
    public interface IMPD_Settings {
        /// <summary>
        /// When set, models are accessed from low memory (0x00292100) rather than high memory (0x060A0000).
        /// </summary>
        bool ForceLowMemoryModels { get; set; }

        /// <summary>
        /// When set, the lightmap index for the surface has an additional calculation step that converts the dot
        /// product of the lighting to an angle, which is used for the lightmap index using this formula:
        /// 
        ///    lightmapIndex = ((dotProduct < 0) ? 0 : atan2(dotProduct, (1 - dotProduct)^2) / PI * 128 + 64) % 32
        ///    
        /// The resulting angle-to-lightmap-index is:
        /// 
        /// Angle  Lightmap-Index
        ///  0      0
        ///  15     5
        ///  30     10
        ///  45     15
        ///  60     21
        ///  75     26
        ///  90     31
        /// --- wraps ---
        ///  105    5
        ///  120    10
        ///  135    15
        ///  150    21
        ///  165    26
        ///  180    31
        ///  
        /// To prevent wrapping, the pitch of the light direction should be pointing very close to straight down, or
        /// --- with poorer results --- directly from the side.
        /// </summary>
        bool NarrowAngleBasedLightmap { get; set; }

        /// <summary>
        /// When set, the MSB for all values in Palette1 will be turned on. This seems to be important for maps with a
        /// gradient, but seems redundant because the code that activates the gradient turns this on anyway. Could be
        /// useful for forcing it if a gradient is not immediately present.
        /// </summary>
        bool SetMSBForPalette1 { get; set; }

        /// <summary>
        /// When set, tiles' textures can be rotated as well as flipped. The surface model should be in Chunk[2] if
        /// this is on.
        /// </summary>
        bool HasSurfaceTextureRotation { get; set; }

        /// <summary>
        /// Y-axis rotation of the entire scene (in degrees), but just the models. Always set to 0x8000, otherwise sprites are
        /// facing the wrong way.
        /// </summary>
        float ModelsYRotation { get; set; }

        /// <summary>
        /// Some sort of value that determines what models are visible. Usually 0x48, goes up to 0x90.
        /// </summary>
        ushort ModelsViewDistance { get; set; }

        /// <summary>
        /// Lower angle (in degrees) of the view area in which models appear. Usually -108.
        /// </summary>
        float ModelsViewAngleMin { get; set; }

        /// <summary>
        /// Upper angle (in degrees) of the view area in which models appear. Usually +108.
        /// </summary>
        float ModelsViewAngleMax { get; set; }

        /// <summary>
        /// Scene X position of the ground plane (in pixels), repeating every 512 for images and every 2048 for tile-based images.
        /// </summary>
        short GroundX { get; set; }

        /// <summary>
        /// Scene Y position of the ground plane.
        /// </summary>
        short GroundY { get; set; }

        /// <summary>
        /// X-axis rotation of the ground plane (in degrees). Usually -90, but is 0 in Scenario 3 TODI00.MPD.
        /// </summary>
        float GroundXRotation { get; set; }

        /// <summary>
        /// Scene Z position of the ground plane (in pixels), repeating every 256 for images and every 2048 for tile-based images.
        /// </summary>
        short GroundZ { get; set; }

        /// <summary>
        /// Screen X position of the skybox (in pixels), repeating every 512.
        /// </summary>
        short BackgroundX { get; set; }

        /// <summary>
        /// Screen Y position of the skybox (in pixels), repeating every 256.
        /// </summary>
        short BackgroundY { get; set; }
    }
}
