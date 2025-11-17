using System.Collections.Generic;
using CommonLib.SGL;

namespace SF3.MPD {
    public class SGL_Model {
        public List<SGL_ModelFace> Faces { get; } = new List<SGL_ModelFace>();
        public List<VECTOR> Vertices { get; } = new List<VECTOR>();
    }
}
