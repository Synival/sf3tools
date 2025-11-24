using System;
using CommonLib.Extensions;
using SF3.Types;

namespace SF3.MPD {
    public class MPD_Surface : IMPD_Surface {
        public MPD_Surface(IMPD_Tile[,] tiles) {
            _tiles = tiles;
            Width  = tiles.GetLength(0);
            Height = tiles.GetLength(0);
        }

        public IMPD_Tile GetTile(int x, int y) => _tiles[x, y];
        public IMPD_Tile[] GetAllTiles() => _tiles.To1DArrayTransposed();

        public int Width { get; }
        public int Height { get; }

        public NormalCalculationSettings NormalSettings { get; set; } = new NormalCalculationSettings();

        private IMPD_Tile[,] _tiles;
    }
}
