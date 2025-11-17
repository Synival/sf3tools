using System.Collections.Generic;

namespace CommonLib.SGL {
    public class MPD_Model {
        public List<SGL_ModelFace> Faces { get; } = new List<SGL_ModelFace>();
        public List<VECTOR> Vertices { get; } = new List<VECTOR>();
    }
}
