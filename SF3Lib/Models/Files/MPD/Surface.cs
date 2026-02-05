using System;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;

namespace SF3.Models.Files.MPD {
    public class Surface : MPD_SurfaceBase {
        public Surface(IMPD_File mpdFile, IMPD_SurfaceTile[,] tiles, Func<bool> hasModelGetter) : base(mpdFile.Settings, tiles, hasModelGetter) {
            MPD_File = mpdFile;
        }

        public IMPD_File MPD_File { get; }

        public override VECTOR GetVertexNormal(int vx, int vy) {
            var (x, y, corner) = GetTileForVertex(vx, vy);
            return GetTile(x, y).GetVertexNormal(corner);
        }

        public override void SetVertexNormal(int vx, int vy, VECTOR normal) {
            var (x, y, corner) = GetTileForVertex(vx, vy);
            GetTile(x, y).SetVertexNormal(corner, normal);
        }

        private (int X, int Y, CornerType Corner) GetTileForVertex(int vx, int vy) {
            if (vx < 63 && vy < 63)
                return (vx, vy, CornerType.BottomLeft);
            else if (vx < 63)
                return (vx, vy - 1, CornerType.TopLeft);
            else if (vy < 63)
                return (vx - 1, vy, CornerType.BottomRight);
            else
                return (vx - 1, vy - 1, CornerType.TopRight);
        }
    }
}
