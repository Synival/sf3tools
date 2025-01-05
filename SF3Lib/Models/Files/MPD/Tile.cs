namespace SF3.Models.Files.MPD {
    public class Tile {
        public Tile(IMPD_File mpdFile, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
        }

        public IMPD_File MPD_File { get; }
        public int X { get; }
        public int Y { get; }
    }
}
