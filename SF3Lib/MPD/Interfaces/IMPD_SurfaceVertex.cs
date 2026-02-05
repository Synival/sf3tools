using CommonLib.SGL;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Abstract representation of a single vertex for a surface.
    /// </summary>
    public interface IMPD_SurfaceVertex {
        /// <summary>
        /// Surface to which this vertex belongs.
        /// </summary>
        IMPD_Surface Surface { get; }

        /// <summary>
        /// X coordinate of the vertex.
        /// </summary>
        int X { get; }

        /// <summary>
        /// Y coordinate of the vertex.
        /// </summary>
        int Y { get; }

        /// <summary>
        /// Normal vector used for the surface model.
        /// </summary>
        VECTOR Normal { get; set; }
    }
}
