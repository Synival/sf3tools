using System;
using System.Globalization;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public abstract class MPD_SurfaceBase : IMPD_Surface {
        protected MPD_SurfaceBase(IMPD_Settings settings) {
            _settings = settings;

            Width  = 64;
            Height = 64;

            _tiles = new IMPD_Tile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_Tile(this, x, y);

            _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        protected MPD_SurfaceBase(IMPD_Settings settings, IMPD_Surface original) {
            _settings = settings;

            Width  = original.Width;
            Height = original.Height;

            _tiles = new IMPD_Tile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_Tile(this, original.GetTile(x, y), x, y);

            _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        protected MPD_SurfaceBase(IMPD_Settings settings, IMPD_Tile[,] tiles, Func<bool> hasModelGetter) {
            _settings = settings;
            _tiles = tiles;
            Width  = tiles.GetLength(0);
            Height = tiles.GetLength(0);
            _hasModelGetter = hasModelGetter ?? (() => true);
        }

        protected MPD_SurfaceBase(IMPD_Settings settings, JToken token) {
            _settings = settings;

            var jObject = (JObject) token;

            Width  = 64;
            Height = 64;

            byte[,] FetchByteTable(string propertyName, byte emptyValue) {
                var rows = ((JArray) jObject.GetValueIfExists(propertyName))
                    ?.Select(x => (string) x)
                    ?.ToArray();

                var table = new byte[Width, Height];
                var rowCount = rows?.Length ?? 0;

                for (int y = 0; y < rowCount && y < table.GetLength(1); y++) {
                    var row = rows[y];
                    for (int x = 0; x * 2 < row.Length - 1 && x < table.GetLength(0); x++) {
                        var valueStr = row.Substring(x * 2, 2);
                        table[x, y] = valueStr == "  "
                            ? emptyValue
                            : byte.Parse(valueStr, NumberStyles.HexNumber);
                    }
                }

                return table;
            }

            byte[,][] FetchHeightTable(string propertyName) {
                var rows = ((JArray) jObject.GetValueIfExists(propertyName))
                    ?.Select(x => (string) x)
                    ?.ToArray();

                var table = new byte[Width, Height][];
                var rowCount = rows?.Length ?? 0;

                for (int y = 0; y < rowCount && y < table.GetLength(1); y++) {
                    var row = rows[y];
                    for (int x = 0; x * 9 < row.Length - 7 && x < table.GetLength(0); x++) {
                        var pos = x * 9;
                        table[x, y] = new byte[] {
                            byte.Parse(row.Substring(pos + 0, 2), NumberStyles.HexNumber),
                            byte.Parse(row.Substring(pos + 2, 2), NumberStyles.HexNumber),
                            byte.Parse(row.Substring(pos + 4, 2), NumberStyles.HexNumber),
                            byte.Parse(row.Substring(pos + 6, 2), NumberStyles.HexNumber),
                        };
                    }
                }

                return table;
            }

            var textureIds = FetchByteTable("TextureIDs", 0xFF);
            var eventIds   = FetchByteTable("EventIDs", 0);
            var heights    = FetchHeightTable("Heights");

            _tiles = new IMPD_Tile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_Tile(this, x, y, textureIds[x, y], eventIds[x, y], heights[x, y]);

           _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        public IMPD_Tile GetTile(int x, int y) => _tiles[x, y];
        public IMPD_Tile[] GetAllTiles() => _tiles.To1DArrayTransposed();

        public void UpdateVertexNormal(int vx, int vy)
            => UpdateVertexNormals(vx, vy, vx, vy);

        public void UpdateVertexNormals(int vx1, int vy1, int vx2, int vy2) {
            // Generate a heightmap for the vertices to update.
            var heightmap = this.GetVertexHeightMeshForNormalCalculation(vx1, vy1, vx2, vy2);

            var verticesWidth  = Width  + 1;
            var verticesHeight = Height + 1;
            for (int vy = 0; vy < verticesHeight; vy++)
                for (int vx = 0; vx < verticesWidth; vx++)
                    SetVertexNormal(vx, vy, this.CalculateVertexNormal(vx, vy, heightmap, vx1 - 1, vy1 - 1));
        }

        public abstract void SetVertexNormal(int vx, int vy, VECTOR normal);
        public abstract VECTOR GetVertexNormal(int vx, int vy);

        public int Width { get; }
        public int Height { get; }

        protected Func<bool> _hasModelGetter;
        public bool HasModel => _hasModelGetter();

        public bool HasRotatableTextures => _settings.HasSurfaceTextureRotation;

        public NormalCalculationSettings NormalSettings { get; set; } = new NormalCalculationSettings();

        protected IMPD_Settings _settings;
        protected IMPD_Tile[,] _tiles;
    }
}
