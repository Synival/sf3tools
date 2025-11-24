using System;
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
        /// The surface to which this tile belongs.
        /// </summary>
        IMPD_Surface Surface { get; }

        /// <summary>
        /// The X position of the tile, where 0 is left-most.
        /// </summary>
        int X { get; }

        /// <summary>
        /// The Y position of the tile, where 0 is bottom-most.
        /// </summary>
        int Y { get; }

        /// <summary>
        /// Random seed assigned to this tile for random noise, tree placement offsets, etc.
        /// </summary>
        int RandomSeed { get; }

        /// <summary>
        /// Flipping, rotation, and "is flat" tile flags.
        /// </summary>
        byte TextureFlags { get; set; }

        /// <summary>
        /// Texture ID for this paticular tile. Set to 0xFF for an empty tile.
        /// </summary>
        byte TextureID { get; set; }

        /// <summary>
        /// Horizontal or vertical flipping for the texture of this tile.
        /// </summary>
        TextureFlipType TextureFlip { get; set; }

        /// <summary>
        /// Rotation of the texture of this tile in increments of 90 degrees.
        /// </summary>
        TextureRotateType TextureRotate { get; set; }

        /// <summary>
        /// When set, the tile is flat and independent from the surface model mesh. Only 150 of this tiles can exist
        /// in any one MPD, excluding flat tiles with no texture (unless the 'KeepTexturelessFlatTiles' MPD flag is on).
        /// Setting this from 'false' to 'true' will automatically update heights to their lowest point and update the
        /// center height, adjacent tile heights, and adjacent tile normals as required.
        /// </summary>
        bool IsFlat { get; set; }

        /// <summary>
        /// Gets the normal vector of the tile for the specified corner.
        /// </summary>
        /// <param name="corner">The corner of the tile whose normal vector should be retrieved.</param>
        /// <returns>A normal VECTOR.</returns>
        VECTOR GetVertexNormal(CornerType corner);

        /// <summary>
        /// Gets normal vectors for all corners of the tile.
        /// </summary>
        /// <returns>A normal VECTOR for every corner of the tile.</returns>
        VECTOR[] GetVertexNormals();

        /// <summary>
        /// Gets the height of a corner of a tile for display. The height returned is relative to the
        /// size of a tile (i.e, a height difference between "0" and "1" is equal to the width/height of a tile).
        /// </summary>
        /// <param name="corner">The corner of the tile whose height should be retrieved.</param>
        /// <returns>A height value for the tile's corner with a scale where 1 = width/height of tile.</returns>
        float GetVisualVertexHeight(CornerType corner);

        /// <summary>
        /// Gets the heights of all corners of a tile for display. The heights returned are relative to the
        /// size of a tile (i.e, a height difference between "0" and "1" is equal to the width/height of a tile).
        /// </summary>
        /// <returns>A height value for every corner of the tile with a scale where 1 = width/height of tile.</returns>
        float[] GetVisualVertexHeights();

        /// <summary>
        /// Sets the height of a corner of a tile for display. The height to set is relative to the
        /// size of a tile (i.e, a height difference between "0" and "1" is equal to the width/height of a tile).
        /// The tile center, adjacent tiles, and all impacted normals are updated as required.
        /// </summary>
        /// <param name="corner">The corner of the tile whose height should be modified.</param>
        /// <param name="value">The new height of the tile corner.</param>
        void SetVisualVertexHeight(CornerType corner, float value);

        /// <summary>
        /// Sets the height of all corners of a tile for display. The heights set are relative to the
        /// size of a tile (i.e, a height difference between "0" and "1" is equal to the width/height of a tile).
        /// The tile center, adjacent tiles, and all impacted normals are updated as required.
        /// <param name="value">The new heights of all tile corners.</param>
        /// </summary>
        void SetVisualVertexHeights(float[] values);

        /// <summary>
        /// The height of the tile's center. The center is always be an average of the four corner vertex heights.
        /// </summary>
        float CenterHeight { get; }

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

        /// <summary>
        /// When true, this tile has visual data (i.e. a SurfaceModel) so its texture ID and flags can be set.
        /// </summary>
        bool HasModel { get; }

        /// <summary>
        /// When true, the texture of this tile can be rotated, which is a Scenario 3+ feature that must be enabled.
        /// </summary>
        bool HasRotatableTexture { get; }

        /// <summary>
        /// Is invoked when the tile is modified.
        /// </summary>
        event EventHandler Modified;
    }
}
