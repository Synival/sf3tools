using System;
using CommonLib.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteSurfaceDataChunk(IMPD_Surface surface)
            => WriteCompressedChunk(writer => writer.WriteSurfaceDataChunkContent(surface));

        public void WriteSurfaceDataChunkContent(IMPD_Surface surface) {
            var tilesWidth  = surface.Width;
            var tilesHeight = surface.Height;

            void ForEachTile(Action<IMPD_Tile> action) {
                for (int y = 0; y < tilesHeight; y++)
                    for (int x = 0; x < tilesWidth; x++)
                        action(surface.GetTile(x, y));
            };

            // Tile corner heights: 0x4000 bytes (64x64x4)
            ForEachTile(tile => {
                WriteByte((byte) Math.Round(tile.GetVertexHeight(CornerType.BottomRight) * 16.0f));
                WriteByte((byte) Math.Round(tile.GetVertexHeight(CornerType.BottomLeft) * 16.0f));
                WriteByte((byte) Math.Round(tile.GetVertexHeight(CornerType.TopLeft) * 16.0f));
                WriteByte((byte) Math.Round(tile.GetVertexHeight(CornerType.TopRight) * 16.0f));
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
