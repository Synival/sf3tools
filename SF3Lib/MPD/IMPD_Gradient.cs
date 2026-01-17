using CommonLib.Imaging;

namespace SF3.MPD {
    public interface IMPD_Gradient {
        /// <summary>
        /// Position with range (0.00, 1.00) of the top color of the gradient with 0.00 being the top.
        /// Everything above this line is the top color.
        /// </summary>
        float TopPosition { get; set; }

        /// <summary>
        /// Position with range (0.00, 1.00) of the bottom color of the gradient with 0.00 being the top.
        /// Everything below this line is the bottom color.
        /// </summary>
        float BottomPosition { get; set; }

        /// <summary>
        /// Top color of the gradient.
        /// </summary>
        IColorRGB555 TopColor { get; set; }

        /// <summary>
        /// Bottom color of the gradient.
        /// </summary>
        IColorRGB555 BottomColor { get; set; }

        /// <summary>
        /// When set, the gradient applies to models + the surface model at 'ModelsAndSurfaceIntensity'.
        /// </summary>
        bool AffectsModelsAndSurface { get; set; }

        /// <summary>
        /// Number with range (0.00, 1.00) with the intensity of the gradient for models + the surface model.
        /// Only active when 'AffectsModelsAndSurface' is also on.
        /// </summary>
        float ModelsAndSurfaceIntensity { get; set; }

        /// <summary>
        /// When set, the gradient applies to the ground plane at 'GroundPlaneIntensity'.
        /// </summary>
        bool AffectsGround { get; set; }

        /// <summary>
        /// Number with range (0.00, 1.00) with the intensity of the gradient for the ground plane.
        /// Only active when 'AffectsGround' is also on.
        /// </summary>
        float GroundIntensity { get; set; }

        /// <summary>
        /// When set, the gradient applies to the sky plane at 'SkyPlaneIntensity'.
        /// </summary>
        bool AffectsSky { get; set; }

        /// <summary>
        /// Number with range (0.00, 1.00) with the intensity of the gradient for the sky plane.
        /// Only active when 'AffectsSky' is also on.
        /// </summary>
        float SkyIntensity { get; set; }

        /// <summary>
        /// Technical flag. When true, the gradient is serialized, but "dummied-out" with a preceeding
        /// 0xFFFF that prevents it from loading.
        /// </summary>
        bool IsDummiedOut { get; set; }
    }
}
