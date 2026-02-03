using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_PlaneTileAssignment : IMPD_PlaneTileAssignment {
        public MPD_PlaneTileAssignment(IMPD_PlaneTileAssignment original) {
            Width  = original.Width;
            Height = original.Height;
            _assignments = new (byte X, byte Y)[Width, Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _assignments[x, y] = original[(byte) x, (byte) y];
        }

        public static MPD_PlaneTileAssignment FromJToken(JToken token, int width, int height) => new MPD_PlaneTileAssignment(token, width, height);
        private MPD_PlaneTileAssignment(JToken token, int width, int height) {
            Width  = width;
            Height = height;
            _assignments = new (byte x, byte y)[width, height];

            var jTiles = (JArray) token;
            var rows = jTiles.Count;
            for (int y = 0; y < height && y < rows; y++) {
                var str = (string) jTiles[y];
                for (int x = 0; x < width && x < str.Length * 2; x++) {
                    _assignments[x, y] = (
                        Base64CharToValue(str[x * 2 + 1]),
                        Base64CharToValue(str[x * 2 + 0])
                    );
                }
            }
        }

        private static byte Base64CharToValue(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
                return (byte) (ch - 'A');
            if (ch >= 'a' && ch <= 'z')
                return (byte) (ch - 'a' + 26);
            if (ch >= '0' && ch <= '9')
                return (byte) (ch - '0' + 52);
            if (ch == '+')
                return 62;
            if (ch == '/')
                return 63;
            else
                return 0;
        }

        private (byte X, byte Y)[,] _assignments;
        public (byte X, byte Y) this[byte x, byte y] {
            get => _assignments[x, y];
            set => _assignments[x, y] = value;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
