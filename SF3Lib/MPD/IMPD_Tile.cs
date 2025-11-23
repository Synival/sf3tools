using CommonLib.SGL;
using CommonLib.Types;
using SF3.Types;

namespace SF3.MPD {
    /// <summary>
    /// Abstract implementation of a tile that belongs to an MPD's surface. Includes the surface heightmap, event IDs,
    /// and model info like textures and texture flags.
    /// </summary>
    public interface IMPD_Tile {
        /// <summary>
        /// Flipping, rotation, and "is flat" tile flags.
        /// </summary>
        byte TextureFlags { get; set; }

        /// <summary>
        /// Texture ID for this paticular tile. Set to 0xFF for an empty tile.
        /// </summary>
        byte TextureID { get; set; }

        /// <summary>
        /// When set, the tile is flat and independent from the surface model mesh. Only 150 of this tiles can exist
        /// in any one MPD, excluding flat tiles with no texture (unless the 'KeepTexturelessFlatTiles' MPD flag is on).
        /// </summary>
        bool IsFlat { get; set; }

        /// <summary>
        /// Gets the normal vector of the tile for the specified corner.
        /// </summary>
        /// <param name="corner">The corner of the tile whose normal vector should be retrieved.</param>
        /// <returns>A normal VECTOR.</returns>
        VECTOR GetVertexNormal(CornerType corner);

        /// <summary>
        /// Gets the height of a corner of a tile for use in the surface model. The height returns is relative to the
        /// size of a tile (i.e, a height difference between "0" and "1" is equal to the width/height of a tile).
        /// </summary>
        /// <param name="corner">The corner of the tile whose height should be retrieved.</param>
        /// <returns>A height value for the tile's corner with a scale where 1 = width/height of tile.</returns>
        float GetVisualVertexHeight(CornerType corner);

        /// <summary>
        /// The height of the tile's center. The center should always be an average of the four corner
        /// vertex heights.
        /// </summary>
        float CenterHeight { get; set; }

        /// <summary>
        /// Type of terrain during battle. Used for movement and land effect calculations.
        /// </summary>
        TerrainType TerrainType { get; set; }

        /// <summary>
        /// Special flags applied to the terrain for battles.
        /// </summary>
        TerrainFlags TerrainFlags { get; set; }

        /// <summary>
        /// Event ID of the tile that ties in with warps, interactables, searchable items, pre-loading triggers, etc.
        /// </summary>
        byte EventID { get; set; }
    }
}
