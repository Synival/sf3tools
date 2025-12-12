using SF3.Images;

namespace SF3.MPD {
    /// <summary>
    /// Abstract representation of a plane that is built from a tileset and assignment map.
    /// </summary>
    public interface IMPD_TiledPlane {
        /// <summary>
        /// Tileset image used for the plane. Should be 512x256.
        /// </summary>
        ITexture Tileset { get; }

        /// <summary>
        /// Table of tileset coordinates for each tile in the plane.
        /// </summary>
        IMPD_PlaneTileAssignment TileAssignment { get; }

        /// <summary>
        /// Resulting image from the TilesetImage and TileAssignment.
        /// This image cannot be directly assigned.
        /// </summary>
        ITexture TiledImage { get; }
    }
}
