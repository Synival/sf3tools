using System;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;

namespace SF3.Models.Files.MPD {
    public class Surface : MPD_SurfaceBase {
        public Surface(IMPD_File mpdFile, Func<bool> hasModelGetter)
        : this(mpdFile, hasModelGetter, MakeTiles(mpdFile)) {
        }

        private Surface(IMPD_File mpdFile, Func<bool> hasModelGetter, IMPD_SurfaceTile[,] tiles)
        : base(mpdFile, mpdFile.Settings, tiles, MakeVertices(mpdFile, tiles), hasModelGetter) {
        }

        private static IMPD_SurfaceTile[,] MakeTiles(IMPD_File file) {
            var tiles = new SurfaceTile[64, 64];
            for (var x = 0; x < 64; x++)
                for (var y = 0; y < 64; y++)
                    tiles[x, y] = new SurfaceTile(file, x, y);
            return tiles;
        }

        private static IMPD_SurfaceVertex[,] MakeVertices(IMPD_File file, IMPD_SurfaceTile[,] tiles) {
            var vertices = new SurfaceVertex[65, 65];
            for (var x = 0; x < 65; x++)
                for (var y = 0; y < 65; y++)
                    vertices[x, y] = new SurfaceVertex(file, tiles, x, y);
            return vertices;
        }
    }
}
