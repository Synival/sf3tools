using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Surface : MPD_SurfaceBase {
        public MPD_Surface(IMPD_Settings settings) : base(settings) {
            _normals = new VECTOR[Width + 1, Height + 1];
            this.UpdateVertexNormals();
        }

        public MPD_Surface(IMPD_Settings settings, IMPD_Surface original) : base(settings, original) {
            _normals = new VECTOR[Width + 1, Height + 1];
            for (int vy = 0; vy < Height + 1; vy++)
                for (int vx = 0; vx < Width + 1; vx++)
                    _normals[vx, vy] = original.GetVertexNormal(vx, vy);
        }

        public static MPD_Surface FromJToken(IMPD_Settings settings, JToken token) => new MPD_Surface(settings, token);
        private MPD_Surface(IMPD_Settings settings, JToken token) : base(settings, token) {
            var jObject = (JObject) token;

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
                        var base64Bytes = Convert.FromBase64String(row.Substring(pos, 8));
                        var components = base64Bytes.ToUShorts();
                        table[x, y] = new VECTOR(
                            components[0] * 2,
                            components[1] * 2,
                            components[2] * 2,
                            isRaw: true
                        );
                    }
                }

                return table;
            }

            _normals = FetchNormalTable("Normals");
        }

        public override void SetVertexNormal(int vx, int vy, VECTOR normal)
            => _normals[vx, vy] = normal;

        public override VECTOR GetVertexNormal(int vx, int vy)
            => _normals[vx, vy];

        private readonly VECTOR[,] _normals;
    }
}
