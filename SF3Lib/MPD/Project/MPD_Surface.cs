using System;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Surface : IMPD_Surface {
        public MPD_Surface(IMPD_Settings settings) {
            _settings = settings;

            Width  = 64;
            Height = 64;

            _tiles = new IMPD_Tile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_Tile(this, x, y);

            _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        public MPD_Surface(IMPD_Settings settings, IMPD_Surface original) {
            _settings = settings;

            Width  = original.Width;
            Height = original.Height;

            _tiles = new IMPD_Tile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_Tile(this, original.GetTile(x, y), x, y);

           _hasModelGetter = () => _settings.HasSurfaceModel;
         }

        public MPD_Surface(IMPD_Settings settings, IMPD_Tile[,] tiles, Func<bool> hasModelGetter) {
            _settings = settings;
            _tiles = tiles;
            Width  = tiles.GetLength(0);
            Height = tiles.GetLength(0);
            _hasModelGetter = hasModelGetter ?? (() => true);
        }

        public static MPD_Surface FromJToken(IMPD_Settings settings, JToken token) => new MPD_Surface(settings, token);
        private MPD_Surface(IMPD_Settings settings, JToken token) {
            _settings = settings;

            var jObject = (JObject) token;

            Width  = 64;
            Height = 64;

            byte[,] PopulateByteTable(string propertyName, byte emptyValue) {
                var rows = ((JArray) jObject[propertyName])
                    .Select(x => (string) x)
                    .ToArray();

                var table = new byte[Width, Height];
                for (int y = 0; y < rows.Length && y < table.GetLength(1); y++) {
                    var row = rows[y];
                    for (int x = 0; x * 2 < row.Length - 1 && x < table.GetLength(0); x++) {
                        var valueStr = row.Substring(x * 2, 2);
                        table[x, y] = valueStr == "  "
                            ? emptyValue
                            : byte.Parse(valueStr, System.Globalization.NumberStyles.HexNumber);
                    }
                }

                return table;
            }

            var textureIds = PopulateByteTable("TextureIDs", 0xFF);
            var eventIds   = PopulateByteTable("EventIDs", 0);

            _tiles = new IMPD_Tile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_Tile(this, x, y, textureIds[x, y], eventIds[x, y]);

           _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        public IMPD_Tile GetTile(int x, int y) => _tiles[x, y];
        public IMPD_Tile[] GetAllTiles() => _tiles.To1DArrayTransposed();

        public int Width { get; }
        public int Height { get; }

        private Func<bool> _hasModelGetter;
        public bool HasModel => _hasModelGetter();

        public bool HasRotatableTextures => _settings.HasSurfaceTextureRotation;

        public NormalCalculationSettings NormalSettings { get; set; } = new NormalCalculationSettings();

        private IMPD_Settings _settings;
        private IMPD_Tile[,] _tiles;
    }
}
