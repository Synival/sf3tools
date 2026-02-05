using CommonLib.SGL;
using CommonLib.Types;
using SF3.MPD.Interfaces;

namespace SF3.Models.Files.MPD {
    public class SurfaceVertex : IMPD_SurfaceVertex {
        public SurfaceVertex(IMPD_File mpdFile, IMPD_SurfaceTile[,] tiles, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
            (NormalTile, NormalTileCorner) = GetTile(tiles);
        }

        public IMPD_File MPD_File { get; }
        public IMPD_Surface Surface => MPD_File.Surface;
        public int X { get; }
        public int Y { get; }

        public VECTOR Normal {
            get => NormalTile.GetVertexNormal(NormalTileCorner);
            set => NormalTile.SetVertexNormal(NormalTileCorner, value);
        }

        private (IMPD_SurfaceTile tile, CornerType Corner) GetTile(IMPD_SurfaceTile[,] tiles) {
            if (X < 63 && Y < 63)
                return (tiles[X, Y], CornerType.BottomLeft);
            else if (X < 63)
                return (tiles[X, Y - 1], CornerType.TopLeft);
            else if (Y < 63)
                return (tiles[X - 1, Y], CornerType.BottomRight);
            else
                return (tiles[X - 1, Y - 1], CornerType.TopRight);
        }

        private IMPD_SurfaceTile NormalTile { get; }
        private CornerType NormalTileCorner { get; }
    }
}
