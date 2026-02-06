using CommonLib.SGL;
using CommonLib.Utils;
using SF3.MPD.Interfaces;

namespace SF3.Models.Files.MPD {
    public class SurfaceVertex : IMPD_SurfaceVertex {
        public SurfaceVertex(IMPD_File mpdFile, IMPD_SurfaceTile[,] tiles, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
            _sharedBlockVertexLocations = BlockHelpers.GetVertexBlockLocations(X, Y);
        }

        public IMPD_File MPD_File { get; }
        public IMPD_Surface Surface => MPD_File.Surface;
        public int X { get; }
        public int Y { get; }

        public VECTOR Normal {
            get {
                if (MPD_File.SurfaceModelChunk?.VertexNormalBlockTable == null)
                    return new VECTOR(0f, 1f, 0f);
                var bl = _sharedBlockVertexLocations[0];
                return MPD_File.SurfaceModelChunk.VertexNormalBlockTable[bl.Num][bl.X, bl.Y];
            }
            set {
                if (MPD_File.SurfaceModelChunk?.VertexNormalBlockTable != null)
                    foreach (var bl in _sharedBlockVertexLocations)
                        MPD_File.SurfaceModelChunk.VertexNormalBlockTable[bl.Num][bl.X, bl.Y] = value;
            }
        }

        private readonly BlockHelpers.BlockVertexLocation[] _sharedBlockVertexLocations;
    }
}
