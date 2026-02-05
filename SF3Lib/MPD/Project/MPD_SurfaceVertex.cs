using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_SurfaceVertex : IMPD_SurfaceVertex {
        public MPD_SurfaceVertex(IMPD_Surface surface, int x, int y) {
            Surface = surface;
            X = x;
            Y = y;
        }

        public IMPD_Surface Surface { get; }
        public int X { get; }
        public int Y { get; }
    }
}
