﻿using System;
using System.Collections.Generic;
using CommonLib.Types;

namespace CommonLib.Utils {
    /// <summary>
    /// Helper functions for dealing with blocks, which have their own coordinate system.
    /// Each block is a 5x5 grid of vertices. The blocks themselves are arranged in a 16x16 grid.
    /// The Y coordinate is upside-down.
    /// The edges of adjacent blocks (should) have shared values.
    /// </summary>
    public static class BlockHelpers {
        /// <summary>
        /// Indicator of a block and the vertex coordinate within it.
        /// </summary>
        public struct BlockVertexLocation {
            public BlockVertexLocation(int num, int x, int y) {
                Num = num;
                X   = x;
                Y   = y;
            }

            public int Num;
            public int X;
            public int Y;
        }

        /// <summary>
        /// Converts a tile X coordinate and corner to a vertex coordinate.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileX'.</param>
        /// <returns>A vertex X coordinate.</returns>
        public static int TileToVertexX(int tileX, CornerType corner)
            => tileX + corner.GetX();

        /// <summary>
        /// Converts a tile Y coordinate and corner to a vertex coordinate.
        /// </summary>
        /// <param name="tileX">Tile Y coordinate.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileY'.</param>
        /// <returns>A vertex Y coordinate.</returns>
        public static int TileToVertexY(int tileY, CornerType corner)
            => 64 - (tileY + corner.GetY());

        /// <summary>
        /// Converts a tile position and corner to a set of shared block locations.
        /// </summary>
        /// <param name="tileX">X coordinate of a tile.</param>
        /// <param name="tileY">Y coordinate of a tile.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileX' and 'tileY'.</param>
        /// <returns>A set of shared block vertex locations for the tile's corner.</returns>
        public static BlockVertexLocation[] GetBlockLocations(int tileX, int tileY, CornerType corner)
            // Get the vertex position for the entire mesh. Blocks are upside-down, so correct for this.
            => GetBlockLocations(tileX + corner.GetX(), 64 - (tileY + corner.GetY()));

        /// <summary>
        /// Converts a vertex position to a set of shared block locations.
        /// </summary>
        /// <param name="tileX">X coordinate of a vertex.</param>
        /// <param name="tileY">Y coordinate of a vertex.</param>
        /// <returns>A set of shared block vertex locations for the vertex.</returns>
        public static BlockVertexLocation[] GetBlockLocations(int vertexX, int vertexY) {
            if (vertexX < 0 || vertexY < 0 || vertexX > 64 || vertexY > 64)
                return new BlockVertexLocation[0];

            // Get the vertex position within the block (only using the right/bottom sides if it's the last block).
            var bx = (vertexX == 64) ? 4 : vertexX % 4;
            var by = (vertexY == 64) ? 4 : vertexY % 4;

            // Get the block to update.
            int Clamp(int num, int min, int max) => Math.Min(Math.Max(num, min), max);
            int blockNum = (Clamp(vertexY, 0, 63) / 4) * 16 + (Clamp(vertexX, 0, 63) / 4);

            var locations = new List<BlockVertexLocation>() { new BlockVertexLocation(blockNum, bx, by) };

            // If requesting the left/top sides (and not the overall edge), provide adjacent blocks.
            if (bx == 0 && vertexX > 0)
                locations.Add(new BlockVertexLocation(blockNum - 1, 4, by));
            if (by == 0 && vertexY > 0)
                locations.Add(new BlockVertexLocation(blockNum - 16, bx, 4));
            if (bx == 0 && by == 0 && vertexX > 0 && vertexY > 0)
                locations.Add(new BlockVertexLocation(blockNum - 17, 4, 4));

            return locations.ToArray();
        }
    }
}
