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

        private T[,] FetchTable<T>(JObject jObject, string propertyName, int width, int height, int strlen, bool spaceBetween, Func<string, T> fetcher, T emptyValue) {
            var rows = ((JArray) jObject.GetValueIfExists(propertyName))
                ?.Select(x => (string) x)
                ?.ToArray();

            var table = new T[width, height];
            var rowCount = rows?.Length ?? 0;

            var span = strlen + (spaceBetween ? 1 : 0);
            var emptyStr = new string(' ', strlen);

            for (int y = 0; y < height; y++) {
                var rowY = height - y - 1;
                var row = (rowY < rowCount && rowY >= 0) ? rows[rowY] : null;
                var maxlen = (row == null) ? 0 : (row.Length - strlen + 1);

                for (int x = 0; x < width; x++) {
                    if (x * span < maxlen) {
                        var valueStr = row.Substring(x * span, strlen);
                        table[x, y] = valueStr == emptyStr ? emptyValue : fetcher(valueStr);
                    }
                    else
                        table[x, y] = emptyValue;
                }
            }

            return table;
        }

        protected MPD_SurfaceBase(IMPD_Settings settings, JToken token) {
            _settings = settings;

            var jObject = (JObject) token;

            Width  = 64;
            Height = 64;

            byte[,] FetchByteTable(string propertyName, byte emptyValue)
                => FetchTable(jObject, propertyName, Width, Height, 2, false, s => byte.Parse(s, NumberStyles.HexNumber), emptyValue);

            byte[,][] FetchHeightTable(string propertyName) {
                return FetchTable(
                    jObject, propertyName, Width, Height, 8, true,
                    s => new byte[] {
                        byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber),
                        byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber),
                        byte.Parse(s.Substring(4, 2), NumberStyles.HexNumber),
                        byte.Parse(s.Substring(6, 2), NumberStyles.HexNumber),
                    },
                    new byte[4]
                );
            }

            VECTOR[,] FetchNormalTable(string propertyName) {
                return FetchTable(
                    jObject, propertyName, Width + 1, Height + 1, 8, true,
                    s => {
                        var base64Bytes = Convert.FromBase64String(s);
                        var components = base64Bytes.ToUShorts();
                        return new VECTOR(
                            components[0] * 2,
                            components[1] * 2,
                            components[2] * 2,
                            isRaw: true
                        );
                    },
                    new VECTOR(0, -1, 0)
                );
            }

            var textureIds   = FetchByteTable("TextureIDs", 0xFF);
            var textureFlags = FetchByteTable("TextureFlags", 0x00);
            var unknownTextureFlags = FetchByteTable("UnknownTextureFlags", 0x00);
            var eventIds     = FetchByteTable("EventIDs", 0);
            var terrainWithFlags = FetchByteTable("Terrain", 0x00);
            var heights      = FetchHeightTable("Heights");
            var normals      = FetchNormalTable("Normals");

            _tiles = new IMPD_SurfaceTile[Width, Height];
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    var terrain = (TerrainType) (terrainWithFlags[x, y] & 0x0F);
                    var terrainFlags = (TerrainFlags) ((terrainWithFlags[x, y] & 0xF0) >> 4);
                    _tiles[x, y] = new MPD_SurfaceTile(this, x, y, textureIds[x, y], textureFlags[x, y], unknownTextureFlags[x, y], eventIds[x, y], terrain, terrainFlags, heights[x, y]);
                }
            }

            _vertices = new IMPD_SurfaceVertex[Width + 1, Height + 1];
            for (int y = 0; y < Height + 1; y++)
                for (int x = 0; x < Width + 1; x++)
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
                    _vertices[vx, vy].Normal = _vertices[vx, vy].CalculateNormal(heightmap, vx1 - 1, vy1 - 1);
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
