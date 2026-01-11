using CommonLib.Imaging;
using CommonLib.SGL;

namespace SF3.MPD {
    /// <summary>
    /// Interface for MPD lighting with a palette and direction vector.
    /// </summary>
    public interface IMPD_Lighting {

        /// <summary>
        /// Palette used for lighting models and the surface model.
        /// </summary>
        Palette Palette { get; }

        /// <summary>
        /// Pitch (X/Z-rotation) of light direction in degrees with range (-180.0, 180.0].
        /// </summary>
        float Pitch { get; set; }

        /// <summary>
        /// Yaw (Y-rotation) of light direction in degrees with range (-180.0, 180.0].
        /// </summary>
        float Yaw { get; set; }
    }
}
