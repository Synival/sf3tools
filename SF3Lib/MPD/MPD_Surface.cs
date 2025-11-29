using System;
using CommonLib.Extensions;
using SF3.Types;

namespace SF3.MPD {
    public class MPD_Surface : IMPD_Surface {
        public MPD_Surface(IMPD_AllFlags flags, IMPD_Tile[,] tiles, Func<bool> hasModelGetter) {
            _flags = flags;
            _tiles = tiles;
            Width  = tiles.GetLength(0);
            Height = tiles.GetLength(0);
            _hasModelGetter = hasModelGetter ?? (() => true);
        }

        public IMPD_Tile GetTile(int x, int y) => _tiles[x, y];
        public IMPD_Tile[] GetAllTiles() => _tiles.To1DArrayTransposed();

        public int Width { get; }
        public int Height { get; }

        private Func<bool> _hasModelGetter;
        public bool HasModel => _hasModelGetter();

        public bool HasRotatableTextures => _flags.Bit_0x0002_HasSurfaceTextureRotation;

        public NormalCalculationSettings NormalSettings { get; set; } = new NormalCalculationSettings();

        private IMPD_AllFlags _flags;
        private IMPD_Tile[,] _tiles;
    }
}
