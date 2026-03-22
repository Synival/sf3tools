using System.Collections.Generic;

namespace CommonLib.SGL {
    public interface ISGL_Model {
        /// <summary>
        /// All vertices for the model.
        /// </summary>
        IReadOnlyList<VECTOR> Vertices { get; }

        /// <summary>
        /// All faces (quads) for the model. Contains the set of vertices to use for each quad and its ATTR information.
        /// </summary>
        IReadOnlyList<ISGL_ModelFace> Faces { get; }
    }
}
