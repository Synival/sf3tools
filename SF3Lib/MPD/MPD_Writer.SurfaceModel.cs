using CommonLib.SGL;
using CommonLib.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        private const int c_surfaceModelBlockCount          = 0x100;
        private const int c_surfaceModelBlockTilesWidth     = 4;
        private const int c_surfaceModelBlockTilesHeight    = 4;
        private const int c_surfaceModelBlockVerticesWidth  = 5;
        private const int c_surfaceModelBlockVerticesHeight = 5;

        public void WriteSurfaceModelChunk(IMPD_Tile[,] tiles)
            => WriteUncompressedChunk(writer => writer.WriteSurfaceModelChunkContent(tiles));

        public void WriteSurfaceModelChunkContent(IMPD_Tile[,] tiles) {
/*
            0x0000 - 0x2000: TileTextureRowTable    | Textures
            0x2000 - 0xB600: VertexNormalBlockTable | Normals
            0xB600 - 0xCF00: VertexHeightBlockTable | Heightmap
*/

            // The surface model is stored as 256 4x4 blocks, in row major order.
            // There are 16 columns of blocks and 16 rows of blocks.

            // 0x10 words (2 bytes each) for 0x100 blocks.
            // 0x2000 bytes total.
            for (int block = 0; block < c_surfaceModelBlockCount; block++) {
                int blockX = (block % 0x10) * c_surfaceModelBlockTilesWidth;
                int blockY = (block / 0x10) * c_surfaceModelBlockTilesHeight;
                for (int y = 0; y < c_surfaceModelBlockTilesHeight; y++) {
                    for (int x = 0; x < c_surfaceModelBlockTilesWidth; x++) {
                        var tile = tiles[x + blockX, y + blockY];
                        WriteUShort((ushort) ((tile.ModelTextureFlags << 8) | tile.ModelTextureID));
                    }
                }
            }

            // 0x03 "weird" compressed fixed decimal values (2 bytes each) in a 5x5 mesh for 0x100 blocks.
            // 0x9600 bytes total.
            for (int block = 0; block < c_surfaceModelBlockCount; block++) {
                int blockX = (block % 0x10) * c_surfaceModelBlockTilesWidth;
                int blockY = (block / 0x10) * c_surfaceModelBlockTilesHeight;
                for (int y = 0; y < c_surfaceModelBlockVerticesHeight; y++) {
                    for (int x = 0; x < c_surfaceModelBlockVerticesWidth; x++) {
                        var tile = GetNonFlatTileAtVertex(tiles, x + blockX, y + blockY, out var corner);
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
                    }
                }
            }

            // TODO: Mesh heightmap.
            const int remainingBytes = 0xCF00 - 0xB600;
            WriteBytes(new byte[remainingBytes]);
        }

        private static readonly (sbyte X, sbyte Y, CornerType ConnectedCorner)[] _getNonFlatTileAtVertexOffsets = new (sbyte, sbyte, CornerType)[] {
            (-1, -1, CornerType.TopRight),
            ( 0, -1, CornerType.TopLeft),
            (-1,  0, CornerType.BottomRight),
            ( 0,  0, CornerType.BottomLeft),
        };

        private IMPD_Tile GetNonFlatTileAtVertex(IMPD_Tile[,] tiles, int upperTileX, int upperTileY, out CornerType connectedCorner) {
            var tilesWidth  = tiles.GetLength(0);
            var tilesHeight = tiles.GetLength(1);

            for (int i = 0; i < 4; i++) {
                var relativePosition = _getNonFlatTileAtVertexOffsets[i];
                var x = upperTileX + relativePosition.X;
                var y = upperTileX + relativePosition.Y;
                if (x < 0 || y < 0 || x >= tilesWidth || y >= tilesHeight)
                    continue;

                var tile = tiles[x, y];
                if (!tile.ModelIsFlat) {
                    connectedCorner = relativePosition.ConnectedCorner;
                    return tile;
                }
            }

            connectedCorner = default;
            return null;
        }
    }
}
