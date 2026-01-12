using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;

namespace CommonLib.SGL {
    public class SGL_Model : ISGL_Model {
        public SGL_Model() {
            Vertices = new VECTOR[0].ToEnumerableWithLength();
            Faces    = new ISGL_ModelFace[0].ToEnumerableWithLength();
        }

        public SGL_Model(int id, IEnumerable<VECTOR> vertices, IEnumerable<ISGL_ModelFace> faces) {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            ID       = id;
            Vertices = vertices.ToArray().ToEnumerableWithLength();
            Faces    = faces.ToArray().ToEnumerableWithLength();
        }

        public int ID { get; set; }
        public IIndexedEnumerableWithLength<VECTOR> Vertices { get; }
        public IIndexedEnumerableWithLength<ISGL_ModelFace> Faces { get; }
    }
}
