using System.Collections.Generic;

namespace CommonLib.SGL {
    public interface ISGL_Model {
        int Collection { get; set; }
        int ID { get; set; }
        List<VECTOR> Vertices { get; }
        List<ISGL_ModelFace> Faces { get; }
    }
}
