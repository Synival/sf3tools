using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLib.SGL {
    public class SGL_Model : ISGL_Model {
        public SGL_Model() {
            Vertices = new List<VECTOR>();
            Faces    = new List<ISGL_ModelFace>();
        }

        public SGL_Model(int collection, int id, IEnumerable<VECTOR> vertices, IEnumerable<ISGL_ModelFace> faces) {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            Collection = collection;
            ID         = id;
            Vertices   = vertices.ToList();
            Faces      = faces.ToList();
        }

        public int Collection { get; set; }
        public int ID { get; set; }
        public List<VECTOR> Vertices { get; }
        public List<ISGL_ModelFace> Faces { get; }
    }
}
