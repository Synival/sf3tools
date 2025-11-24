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
            void ForEachBlock(Action<int /*blockTileX*/, int /*blockTileY*/> action) {
                for (int block = 0; block < c_surfaceModelBlockCount; block++) {
                    int tileBlockX = (block % 0x10) * c_surfaceModelBlockTilesWidth;
                    int tileBlockY = (block / 0x10) * c_surfaceModelBlockTilesHeight;
                    action(tileBlockX, tileBlockY);
                }
            }

            void ForEachBlockTile(Action<int /*tileX*/, int /*tileY*/> action) {
                ForEachBlock((blockTileX, blockTileY) => {
                    for (int y = 0; y < c_surfaceModelBlockTilesHeight; y++)
                        for (int x = 0; x < c_surfaceModelBlockTilesWidth; x++)
                            action(x + blockTileX, y + blockTileY);
                });
            }

            void ForEachBlockVertex(Action<int /*tileX*/, int /*tileY*/> action) {
                ForEachBlock((blockTileX, blockTileY) => {
                    for (int y = 0; y < c_surfaceModelBlockVerticesHeight; y++)
                        for (int x = 0; x < c_surfaceModelBlockVerticesWidth; x++)
                            action(x + blockTileX, y + blockTileY);
                });
            }

            // 0x10 words (2 bytes each) for 0x100 blocks.
            // 0x2000 bytes total.
            ForEachBlockTile((x, y) => {
                var tile = surface.GetTile(x, y);
                WriteUShort((ushort) ((tile.TextureFlags << 8) | tile.TextureID));
            });

            // 0x03 "weird" compressed fixed decimal values (2 bytes each) per vertex in a 5x5 mesh for 0x100 blocks.
            // 0x9600 bytes total.
            ForEachBlockVertex((x, y) => {
                var tile = GetNonFlatTileAtVertex(surface, x, y, out var corner);
                if (tile != null) {
                    var normal = tile.GetVertexNormal(corner);
                    WriteUShort(new CompressedFIXED(normal.X).WeirdRawShort);
                    WriteUShort(new CompressedFIXED(normal.Y).WeirdRawShort);
                    WriteUShort(new CompressedFIXED(normal.Z).WeirdRawShort);
                }
                else {
                    WriteUShort(0);
                    WriteUShort(0);
                    WriteUShort(0);
                }
            });

            // 0x01 byte per vertex in a 5x5 mesh for 0x100 blocks.
            // 0x1900 bytes total.
            ForEachBlockVertex((x, y) => {
                var tile = GetNonFlatTileAtVertex(surface, x, y, out var corner);
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

        private IMPD_Tile GetNonFlatTileAtVertex(IMPD_Surface surface, int upperTileX, int upperTileY, out CornerType connectedCorner) {
            var tilesWidth  = surface.Width;
            var tilesHeight = surface.Height;

            for (int i = 0; i < 4; i++) {
                var relativePosition = _getNonFlatTileAtVertexOffsets[i];
                var x = upperTileX + relativePosition.X;
                var y = upperTileY + relativePosition.Y;
                if (x < 0 || y < 0 || x >= tilesWidth || y >= tilesHeight)
                    continue;

                var tile = surface.GetTile(x, y);
                if (!tile.IsFlat) {
                    connectedCorner = relativePosition.ConnectedCorner;
                    return tile;
                }
            }

            connectedCorner = default;
            return null;
        }
    }
}
