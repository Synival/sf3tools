namespace SF3.MPD {
    public class TileSeeds {
        private static int[,] s_tileSeeds = null;

        public static void GenerateTileSeeds() {
            s_tileSeeds = new int[64, 64];
            for (int x = 0; x < 64; x++) {
                for (int y = 0; y < 64; y++) {
                    var seed = 2166136261;
                    seed += (uint) x;
                    seed *= 16777619;
                    seed += (uint) y;
                    seed *= 16777619;
                    s_tileSeeds[x, y] = (int) seed;
                }
            }
        }

        public static int GetTileSeed(int x, int y) {
            if (s_tileSeeds == null)
                GenerateTileSeeds();
            return s_tileSeeds[x, y];
        }
    }
}
