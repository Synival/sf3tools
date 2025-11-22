using CommonLib.SGL;
using CommonLib.Types;

namespace SF3.MPD {
    /// <summary>
    /// Abstract implementation of a tile that belongs to an MPD's surface. Includes the surface heightmap, event IDs,
    /// and model info like textures and texture flags.
    /// </summary>
    public interface IMPD_Tile {
        /// <summary>
        /// Flipping, rotation, and "is flat" tile flags.
        /// </summary>
        byte ModelTextureFlags { get; set; }

        /// <summary>
        /// Texture ID for this paticular tile. Set to 0xFF for an empty tile.
        /// </summary>
        byte ModelTextureID { get; set; }

        /// <summary>
        /// When set, the tile is flat and independent from the surface model mesh. Only 150 of this tiles can exist
        /// in any one MPD, excluding flat tiles with no texture (unless the 'KeepTexturelessFlatTiles' MPD flag is on).
        /// </summary>
        bool ModelIsFlat { get; set; }

        /// <summary>
        /// Gets the normal vector of the tile for the specified corner.
        /// </summary>
        /// <param name="corner">The corner of the tile whose normal vector should be retrieved.</param>
        /// <returns>A normal VECTOR.</returns>
        VECTOR GetVertexNormal(CornerType corner);
    }
}
