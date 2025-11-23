using System;
using CommonLib.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteSurfaceChunk(IMPD_Tile[,] tiles)
            => WriteCompressedChunk(writer => writer.WriteSurfaceChunkContent(tiles));

        public void WriteSurfaceChunkContent(IMPD_Tile[,] tiles) {
            var tilesWidth  = tiles.GetLength(0);
            var tilesHeight = tiles.GetLength(1);

            void ForEachTile(Action<IMPD_Tile> action) {
                for (int y = 0; y < tilesHeight; y++)
                    for (int x = 0; x < tilesWidth; x++)
                        action(tiles[x, y]);
            };

            // Tile corner heights: 0x4000 bytes (64x64x4)
            ForEachTile(tile => {
                WriteByte((byte) Math.Round(tile.GetVisualVertexHeight(CornerType.BottomRight) * 16.0f));
                WriteByte((byte) Math.Round(tile.GetVisualVertexHeight(CornerType.BottomLeft) * 16.0f));
                WriteByte((byte) Math.Round(tile.GetVisualVertexHeight(CornerType.TopLeft) * 16.0f));
                WriteByte((byte) Math.Round(tile.GetVisualVertexHeight(CornerType.TopRight) * 16.0f));
            });

            // Tile center heights + terrain: 0x2000 bytes (64x64x2)
            ForEachTile(tile => {
                ushort word = (ushort) (
                    ((byte) Math.Round(tile.CenterHeight * 16.0f) << 8) |
                    (((byte) tile.TerrainFlags & 0x0F) << 4) |
                    ((byte) tile.TerrainType & 0x0F)
                );
                WriteUShort(word);
            });

            // Tile event IDs: 0x1000 bytes (64x64x1)
            ForEachTile(tile => WriteByte(tile.EventID));
        }
    }
}
