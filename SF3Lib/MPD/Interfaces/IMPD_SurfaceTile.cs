using System;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Abstract implementation of a tile that belongs to an MPD's surface. Includes the surface heightmap, event IDs,
    /// and model info like textures and texture flags.
    /// </summary>
    public interface IMPD_SurfaceTile {
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
        /// Texture flags that are set but are likely invalid.
        /// </summary>
        byte UnknownTextureFlags { get; set; }

        /// <summary>
        /// When set, the tile is flat and independent from the surface model mesh. Only 150 of this tiles can exist
        /// in any one MPD, excluding flat tiles with no texture (unless the 'Bit_0x0008_KeepTexturelessFlatTiles' MPD flag is on).
        /// Setting this from 'false' to 'true' will automatically update heights to their lowest point and update the
        /// center height, adjacent tile heights, and adjacent tile normals as required.
        /// </summary>
        bool IsFlat { get; set; }

        /// <summary>
        /// Gets the height of a corner of a tile for display.
        /// </summary>
        /// <param name="corner">The corner of the tile whose height should be retrieved.</param>
        /// <returns>A height value for the tile's corner.</returns>
        byte GetVertexHeight(CornerType corner);

        /// <summary>
        /// Gets the heights of all corners of a tile for display. The heights returned are relative to the
        /// size of a tile (i.e, a height difference between "0" and "1" is equal to the width/height of a tile).
        /// </summary>
        /// <returns>A height value for every corner of the tile.</returns>
        byte[] GetVertexHeights();

        /// <summary>
        /// Sets the height of a corner of a tile for display.
        /// The tile center, adjacent tiles, and all impacted normals are updated as required.
        /// </summary>
        /// <param name="corner">The corner of the tile whose height should be modified.</param>
        /// <param name="value">The new height of the tile corner.</param>
        void SetVertexHeight(CornerType corner, byte value);

        /// <summary>
        /// Sets the height of all corners of a tile for display.
        /// The tile center, adjacent tiles, and all impacted normals are updated as required.
        /// <param name="values">The new heights of all tile corners.</param>
        /// </summary>
        void SetVertexHeights(byte[] values);

        /// <summary>
        /// The height of the tile's center. The center is always an average of the four corner vertex heights
        /// rounded down.
        /// </summary>
        byte CenterHeight { get; }

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
        /// Is invoked when the tile is modified.
        /// </summary>
        event EventHandler Modified;
    }
}
