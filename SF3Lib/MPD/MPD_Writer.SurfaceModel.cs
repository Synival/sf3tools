using System;
using CommonLib.SGL;
using CommonLib.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        private const int c_surfaceModelBlockCount          = 0x100;
        private const int c_surfaceModelBlockTilesWidth     = 4;
        private const int c_surfaceModelBlockTilesHeight    = 4;
        private const int c_surfaceModelBlockVerticesWidth  = 5;
        private const int c_surfaceModelBlockVerticesHeight = 5;

        public void WriteSurfaceModelChunk(IMPD_Surface surface)
            => WriteUncompressedChunk(writer => writer.WriteSurfaceModelChunkContent(surface));

        public void WriteSurfaceModelChunkContent(IMPD_Surface surface) {
            // The surface model is stored as 256 4x4 blocks, in row major order.
            // There are 16 columns of blocks and 16 rows of blocks.
            void ForEachBlock(Action<int /*blockX*/, int /*blockY*/> action) {
                for (int block = 0; block < c_surfaceModelBlockCount; block++)
                    action(block % 0x10, block / 0x10);
            }

            void ForEachBlockTile(Action<int /*blockX*/, int /*blockY*/, int /*tileX*/, int /*tileY*/> action) {
                ForEachBlock((blockX, blockY) => {
                    for (int y = 0; y < c_surfaceModelBlockTilesHeight; y++)
                        for (int x = 0; x < c_surfaceModelBlockTilesWidth; x++)
                            action(blockX, blockY, x, y);
                });
            }

            void ForEachBlockVertex(Action<int /*blockX*/, int /*blockY*/, int /*tileX*/, int /*tileY*/> action) {
                ForEachBlock((blockX, blockY) => {
                    for (int y = 0; y < c_surfaceModelBlockVerticesHeight; y++)
                        for (int x = 0; x < c_surfaceModelBlockVerticesWidth; x++)
                            action(blockX, blockY, x, y);
                });
            }

            // 0x10 words (2 bytes each) for 0x100 blocks.
            // 0x2000 bytes total.
            ForEachBlockTile((blockX, blockY, inBlockX, inBlockY) => {
                int tileX = blockX * c_surfaceModelBlockTilesWidth + inBlockX;
                int tileY = blockY * c_surfaceModelBlockTilesHeight + inBlockY;
                var tile = surface.GetTile(tileX, tileY);
                WriteUShort((ushort) ((tile.TextureFlags << 8) | tile.TextureID));
            });

            // 0x03 "weird" compressed fixed decimal values (2 bytes each) per vertex in a 5x5 mesh for 0x100 blocks.
            // 0x9600 bytes total.
            ForEachBlockVertex((blockX, blockY, inBlockX, inBlockY) => {
                var (tile, corner) = GetNonFlatTileAtVertex(surface, blockX, blockY, inBlockX, inBlockY, mustBeInBlock: false);
                if (tile != null) {
                    var normal = tile.GetVertexNormal(corner);
                    WriteUShort(new CompressedFIXED(normal.X).WeirdRawShort);
                    WriteUShort(new CompressedFIXED(normal.Y).WeirdRawShort);
                    WriteUShort(new CompressedFIXED(normal.Z).WeirdRawShort);
                }
                else {
                    WriteUShort(0);
                    WriteUShort(1);
                    WriteUShort(0);
                }
            });

            // 0x01 byte per vertex in a 5x5 mesh for 0x100 blocks.
            // 0x1900 bytes total.
            ForEachBlockVertex((blockX, blockY, inBlockX, inBlockY) => {
                var (tile, corner) = GetNonFlatTileAtVertex(surface, blockX, blockY, inBlockX, inBlockY, mustBeInBlock: true);
                if (tile != null)
                    WriteByte((byte) Math.Round(tile.GetVertexHeight(corner) * 16.00f));
                else
                    WriteByte(0);
            });
        }

        private static readonly (sbyte X, sbyte Y, CornerType ConnectedCorner)[] _getNonFlatTileAtVertexOffsets = new (sbyte, sbyte, CornerType)[] {
            (-1, -1, CornerType.TopRight),
            ( 0, -1, CornerType.TopLeft),
            (-1,  0, CornerType.BottomRight),
            ( 0,  0, CornerType.BottomLeft),
        };

        private (IMPD_Tile Tile, CornerType ConnectedCorner) GetNonFlatTileAtVertex(IMPD_Surface surface, int blockX, int blockY, int inBlockVertexX, int inBlockVertexY, bool mustBeInBlock) {
            var tilesWidth  = surface.Width;
            var tilesHeight = surface.Height;

            for (int i = 0; i < 4; i++) {
                // Get the tile for the corner 'i'.
                var relativePosition = _getNonFlatTileAtVertexOffsets[i];
                var inBlockTileX = inBlockVertexX + relativePosition.X;
                var inBlockTileY = inBlockVertexY + relativePosition.Y;

                if (mustBeInBlock)
                    if (inBlockTileX < 0 || inBlockTileY < 0 || inBlockTileX >= c_surfaceModelBlockTilesWidth || inBlockTileY >= c_surfaceModelBlockTilesHeight)
                        continue;

                var tileX = blockX * c_surfaceModelBlockTilesWidth  + inBlockTileX;
                var tileY = blockY * c_surfaceModelBlockTilesHeight + inBlockTileY;

                if (tileX < 0 || tileY < 0 || tileX >= tilesWidth || tileY >= tilesHeight)
                    continue;

                var tile = surface.GetTile(tileX, tileY);
                if (tile.IsFlat)
                    continue;

                // Looks like a valid tile -- return it!
                return (tile, relativePosition.ConnectedCorner);
            }

            // No non-flat tile found for this vertex.
            return default;
        }
    }
}
