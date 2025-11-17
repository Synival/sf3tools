using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLib.SGL {
    public class SGL_Model {
        public SGL_Model() {
            Vertices = new List<VECTOR>();
            Faces    = new List<SGL_ModelFace>();
        }

        public SGL_Model(int id, IEnumerable<VECTOR> vertices, IEnumerable<SGL_ModelFace> faces) {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            ID       = id;
            Vertices = vertices.ToList();
            Faces    = faces.ToList();
        }

        public int ID { get; set; }
        public List<VECTOR> Vertices { get; }
        public List<SGL_ModelFace> Faces { get; }
    }
}
