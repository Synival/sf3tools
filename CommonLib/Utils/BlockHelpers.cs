﻿using System;
using System.Collections.Generic;
using CommonLib.Types;

namespace CommonLib.Utils {
    /// <summary>
    /// Helper functions for dealing with blocks, which have their own coordinate system.
    /// Each block is a 5x5 grid of vertices. The blocks themselves are arranged in a 16x16 grid.
    /// The edges of adjacent blocks (should) have shared values.
    /// </summary>
    public static class BlockHelpers {
        /// <summary>
        /// Indicator of a block and the tile coordinate within it.
        /// </summary>
        public struct BlockTileLocation {
            public BlockTileLocation(int num, int x, int y) {
                Num = num;
                X   = x;
                Y   = y;
            }

            public int Num;
            public int X;
            public int Y;

            public override string ToString()
                => Num + "[" + X + ", " + Y + "]";
        }

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

            public override string ToString()
                => Num + "[" + X + ", " + Y + "]";
        }

        /// <summary>
        /// Retrieves the block number for a requested tile.
        /// </summary>
        /// <param name="tileX">X coordinate of the requested tile.</param>
        /// <param name="tileY">Y coordinate of the requested tile.</param>
        /// <returns>The block number for the requested tile.</returns>
        public static int TileToBlockNum(int tileX, int tileY)
            => (tileX / 4) + (tileY / 4) * 16;

        /// <summary>
        /// Converts a tile X coordinate and corner to a tile coordinate within a block.
        /// </summary>
        /// <param name="tileX">X coordinate of the requested tile.</param>
        /// <returns>An X coordinate within the block.</returns>
        public static int TileToBlockX(int tileX)
            => tileX % 4;

        /// <summary>
        /// Converts a tile Y coordinate and corner to a tile coordinate within a block.
        /// </summary>
        /// <param name="tileY">Y coordinate of the requested tile.</param>
        /// <returns>A Y coordinate within the block.</returns>
        public static int TileToBlockY(int tileY)
            => tileY % 4;

        /// <summary>
        /// Converts a tile X coordinate and corner to a vertex coordinate.
        /// </summary>
        /// <param name="tileX">X coordinate of the requested tile.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileX'.</param>
        /// <returns>A vertex X coordinate.</returns>
        public static int TileToVertexX(int tileX, CornerType corner)
            => tileX + corner.GetVertexOffsetX();

        /// <summary>
        /// Converts a tile Y coordinate and corner to a vertex coordinate.
        /// </summary>
        /// <param name="tileY">Y coordinate of the requested tile.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileY'.</param>
        /// <returns>A vertex Y coordinate.</returns>
        public static int TileToVertexY(int tileY, CornerType corner)
            => tileY + corner.GetVertexOffsetY();

        /// <summary>
        /// Converts a tile position to a block location.
        /// </summary>
        /// <param name="tileX">X coordinate of a tile.</param>
        /// <param name="tileY">Y coordinate of a tile.</param>
        /// <returns>A block tile location for the tile.</returns>
        public static BlockTileLocation GetTileBlockLocation(int tileX, int tileY)
            => new BlockTileLocation { Num = TileToBlockNum(tileX, tileY), X = TileToBlockX(tileX), Y = TileToBlockY(tileY) };

        /// <summary>
        /// Converts a tile position and corner to a set of shared block locations.
        /// </summary>
        /// <param name="tileX">X coordinate of a tile.</param>
        /// <param name="tileY">Y coordinate of a tile.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileX' and 'tileY'.</param>
        /// <param name="onlyInBlock">If 'true', only the location in the block belonging to 'tileX' and 'tileY' will be returned.</param>
        /// <returns>A set of shared block vertex locations for the tile's corner.</returns>
        public static BlockVertexLocation[] GetVertexBlockLocations(int tileX, int tileY, CornerType corner, bool onlyInBlock = false)
            => GetVertexBlockLocations(tileX + corner.GetVertexOffsetX(), tileY + corner.GetVertexOffsetY(), onlyInBlock ? TileToBlockNum(tileX, tileY) : (int?) null);

        /// <summary>
        /// Converts a vertex position to a set of shared block locations.
        /// </summary>
        /// <param name="tileX">X coordinate of a vertex.</param>
        /// <param name="tileY">Y coordinate of a vertex.</param>
        /// <returns>A set of shared block vertex locations for the vertex.</returns>
        public static BlockVertexLocation[] GetVertexBlockLocations(int vertexX, int vertexY, int? onlyInBlockNum = null) {
            if (vertexX < 0 || vertexY < 0 || vertexX > 64 || vertexY > 64)
                return new BlockVertexLocation[0];

            // Get the vertex position within the block (only using the right/bottom sides if it's the last block).
            var bx = (vertexX == 64) ? 4 : vertexX % 4;
            var by = (vertexY == 64) ? 4 : vertexY % 4;

            // Get the block to update.
            int Clamp(int num, int min, int max) => Math.Min(Math.Max(num, min), max);
            int blockNum = (Clamp(vertexY, 0, 63) / 4) * 16 + (Clamp(vertexX, 0, 63) / 4);

            var locations = new List<BlockVertexLocation>();

            void AddBlockVertexLocation(int blockNum2, int bx2, int by2) {
                if (onlyInBlockNum.HasValue ? (blockNum2 == onlyInBlockNum.Value) : true)
                    locations.Add(new BlockVertexLocation(blockNum2, bx2, by2));
            }
            AddBlockVertexLocation(blockNum, bx, by);

            // If requesting the left/top sides (and not the overall edge), provide adjacent blocks.
            if (bx == 0 && vertexX > 0)
                AddBlockVertexLocation(blockNum - 1, 4, by);
            if (by == 0 && vertexY > 0)
                AddBlockVertexLocation(blockNum - 16, bx, 4);
            if (bx == 0 && by == 0 && vertexX > 0 && vertexY > 0)
                AddBlockVertexLocation(blockNum - 17, 4, 4);

            return locations.ToArray();
        }
    }
}
