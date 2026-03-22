using System.Collections.Generic;

namespace CommonLib.SGL {
    public interface ISGL_ModelFace {
        IReadOnlyList<int> VertexIndices { get; }
        VECTOR Normal { get; set; }
        IATTR Attributes { get; set; }
    }
}
