using System;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteSurfaceChunk(IMPD_Tile[,] tiles)
            => WriteCompressedChunk(writer => writer.WriteSurfaceChunkContent(tiles));

        public void WriteSurfaceChunkContent(IMPD_Tile[,] tiles) {
/*
                (HeightmapRowTable     = HeightmapRowTable.Create    (Data, "HeightmapRows",     0x0000)),
                (HeightTerrainRowTable = HeightTerrainRowTable.Create(Data, "HeightTerrainRows", 0x4000)),
                (EventIDRowTable       = EventIDRowTable.Create      (Data, "EventIDRows",       0x6000)),
*/
            var tilesWidth  = tiles.GetLength(0);
            var tilesHeight = tiles.GetLength(1);

            void ForEachTile(Action<IMPD_Tile> action) {
                for (int y = 0; y < tilesHeight; y++)
                    for (int x = 0; x < tilesWidth; x++)
                        action(tiles[x, y]);
            };

            // Tile corner heights: 0x4000 bytes (64x64x4)
            ForEachTile(tile => {
                // TODO: actually write the thing
                WriteBytes(new byte[4]);
            });

            // Tile center heights + terrain: 0x2000 bytes (64x64x2)
            ForEachTile(tile => {
                // TODO: actually write the thing
                WriteBytes(new byte[2]);
            });

            // Tile event IDs: 0x1000 bytes (64x64x1)
            ForEachTile(tile => WriteByte(tile.EventID));
        }
    }
}
