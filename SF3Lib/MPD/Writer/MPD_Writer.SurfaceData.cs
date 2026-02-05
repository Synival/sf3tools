using System;
using CommonLib.Types;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Writer {
    public partial class MPD_Writer {
        public void WriteSurfaceDataChunk(IMPD_Surface surface)
            => WriteCompressedChunk(writer => writer.WriteSurfaceDataChunkContent(surface));

        public void WriteSurfaceDataChunkContent(IMPD_Surface surface) {
            var tilesWidth  = surface.Width;
            var tilesHeight = surface.Height;

            void ForEachTile(Action<IMPD_SurfaceTile> action) {
                for (int y = 0; y < tilesHeight; y++)
                    for (int x = 0; x < tilesWidth; x++)
                        action(surface.GetTile(x, y));
            };

            // Tile corner heights: 0x4000 bytes (64x64x4)
            ForEachTile(tile => {
                WriteByte(tile.GetVertexHeight(CornerType.BottomRight));
                WriteByte(tile.GetVertexHeight(CornerType.BottomLeft));
                WriteByte(tile.GetVertexHeight(CornerType.TopLeft));
                WriteByte(tile.GetVertexHeight(CornerType.TopRight));
            });

            // Tile center heights + terrain: 0x2000 bytes (64x64x2)
            ForEachTile(tile => {
                ushort word = (ushort) (
                    (tile.CenterHeight << 8) |
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
