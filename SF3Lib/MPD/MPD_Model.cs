using System.Collections.Generic;
using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD {
    public class MPD_Model : SGL_Model, IMPD_Model {
        public MPD_Model() : base() {}

        public MPD_Model(MPD_CollectionType collection, int id, IEnumerable<VECTOR> vertices, IEnumerable<ISGL_ModelFace> faces)
        : base(id, vertices, faces) {
            Collection = collection;
        }

        public MPD_CollectionType Collection { get; set; }
    }
}
