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
        /// Directional vector for lighting.
        /// </summary>
        VECTOR Direction { get; }
    }
}
