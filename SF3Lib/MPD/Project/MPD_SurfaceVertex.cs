using CommonLib.SGL;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_SurfaceVertex : IMPD_SurfaceVertex {
        public MPD_SurfaceVertex(IMPD_Surface surface, IMPD_SurfaceVertex original, int x, int y) {
            Surface = surface;
            X = x;
            Y = y;
            Normal = original.Normal;
        }

        public MPD_SurfaceVertex(IMPD_Surface surface, int x, int y, VECTOR normal) {
            Surface = surface;
            X = x;
            Y = y;
            Normal = normal;
        }

        public IMPD_Surface Surface { get; }
        public int X { get; }
        public int Y { get; }
        public VECTOR Normal { get; set; }
    }
}
