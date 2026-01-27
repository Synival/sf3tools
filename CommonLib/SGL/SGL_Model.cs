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

        public SGL_Model(ISGL_Model original) {
            if (original.Vertices != null)
                Vertices = original.Vertices.Select(x => new VECTOR(x)).ToArray().ToEnumerableWithLength();
            if (original.Faces != null)
                Faces = original.Faces.Select(x => (ISGL_ModelFace) (new SGL_ModelFace(x))).ToArray().ToEnumerableWithLength();
        }

        public SGL_Model(IEnumerable<VECTOR> vertices, IEnumerable<ISGL_ModelFace> faces) {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            Vertices = vertices.ToArray().ToEnumerableWithLength();
            Faces    = faces.ToArray().ToEnumerableWithLength();
        }

        public IIndexedEnumerableWithLength<VECTOR> Vertices { get; }
        public IIndexedEnumerableWithLength<ISGL_ModelFace> Faces { get; }
    }
}
