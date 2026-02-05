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

            _tiles = new IMPD_SurfaceTile[Width, Height];
            for (int ty = 0; ty < Height; ty++)
                for (int tx = 0; tx < Width; tx++)
                    _tiles[tx, ty] = new MPD_SurfaceTile(this, tx, ty);

            _vertices = new IMPD_SurfaceVertex[Width + 1, Height + 1];
            for (int vy = 0; vy < Height; vy++)
                for (int vx = 0; vx < Width; vx++)
                    _vertices[vx, vy] = new MPD_SurfaceVertex(this, vx, vy, new VECTOR(0, -1, 0));

            _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        protected MPD_SurfaceBase(IMPD_Settings settings, IMPD_Surface original) {
            _settings = settings;

            Width  = original.Width;
            Height = original.Height;

            _tiles = new IMPD_SurfaceTile[Width, Height];
            for (int ty = 0; ty < Width; ty++)
                for (int tx = 0; tx < Height; tx++)
                    _tiles[tx, ty] = new MPD_SurfaceTile(this, original.GetTile(tx, ty), tx, ty);

            _vertices = new IMPD_SurfaceVertex[Width + 1, Height + 1];
            for (int vy = 0; vy < Width + 1; vy++)
                for (int vx = 0; vx < Height + 1; vx++)
                    _vertices[vx, vy] = new MPD_SurfaceVertex(this, original.GetVertex(vx, vy), vx, vy);

            _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        protected MPD_SurfaceBase(IMPD_Settings settings, IMPD_SurfaceTile[,] tiles, IMPD_SurfaceVertex[,] vertices, Func<bool> hasModelGetter) {
            _settings = settings;
            _tiles = tiles;
            _vertices = vertices;
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

            VECTOR[,] FetchNormalTable(string propertyName) {
                var rows = ((JArray) jObject.GetValueIfExists(propertyName))
                    ?.Select(x => (string) x)
                    ?.ToArray();

                var table = new VECTOR[Width + 1, Height + 1];
                var rowCount = rows?.Length ?? 0;

                for (int y = 0; y < rowCount && y < table.GetLength(1); y++) {
                    var row = rows[y];
                    for (int x = 0; x * 9 < row.Length - 7 && x < table.GetLength(0); x++) {
                        var pos = x * 9;
                        var str = row.Substring(pos, 8);
                        if (str == "        ")
                            table[x, y] = new VECTOR(0, -1, 0);
                        else {
                            var base64Bytes = Convert.FromBase64String(str);
                            var components = base64Bytes.ToUShorts();
                            table[x, y] = new VECTOR(
                                components[0] * 2,
                                components[1] * 2,
                                components[2] * 2,
                                isRaw: true
                            );
                        }
                    }
                }

                return table;
            }

            var textureIds = FetchByteTable("TextureIDs", 0xFF);
            var eventIds   = FetchByteTable("EventIDs", 0);
            var heights    = FetchHeightTable("Heights");
            var normals    = FetchNormalTable("Normals");

            _tiles = new IMPD_SurfaceTile[Width, Height];
            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    _tiles[x, y] = new MPD_SurfaceTile(this, x, y, textureIds[x, y], eventIds[x, y], heights[x, y]);

            _vertices = new IMPD_SurfaceVertex[Width + 1, Height + 1];
            for (int y = 0; y < Width + 1; y++)
                for (int x = 0; x < Height + 1; x++)
                    _vertices[x, y] = new MPD_SurfaceVertex(this, x, y, normals[x, y]);

           _hasModelGetter = () => _settings.HasSurfaceModel;
        }

        public IMPD_SurfaceTile GetTile(int x, int y) => _tiles[x, y];
        public IMPD_SurfaceVertex GetVertex(int vx, int vy) => _vertices[vx, vy];
        public IMPD_SurfaceTile[] GetAllTiles() => _tiles.To1DArrayTransposed();

        public void UpdateVertexNormal(int vx, int vy)
            => UpdateVertexNormals(vx, vy, vx, vy);

        public void UpdateVertexNormals(int vx1, int vy1, int vx2, int vy2) {
            // Generate a heightmap for the vertices to update.
            var heightmap = this.GetVertexHeightMeshForNormalCalculation(vx1, vy1, vx2, vy2);

            var verticesWidth  = Width  + 1;
            var verticesHeight = Height + 1;
            for (int vy = 0; vy < verticesHeight; vy++)
                for (int vx = 0; vx < verticesWidth; vx++)
                    _vertices[vx, vy].Normal = this.CalculateVertexNormal(vx, vy, heightmap, vx1 - 1, vy1 - 1);
        }

        public int Width { get; }
        public int Height { get; }

        protected Func<bool> _hasModelGetter;
        public bool HasModel => _hasModelGetter();

        public bool HasRotatableTextures => _settings.HasSurfaceTextureRotation;

        public NormalCalculationSettings NormalSettings { get; set; } = new NormalCalculationSettings();

        protected IMPD_Settings _settings;
        protected IMPD_SurfaceTile[,] _tiles;
        protected IMPD_SurfaceVertex[,] _vertices;
    }
}
