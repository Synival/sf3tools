using System.Collections.Generic;

namespace CommonLib.SGL {
    public interface ISGL_Model {
        /// <summary>
        /// "Meta" ID that this collection belongs to if you have to render multiple groups.
        /// </summary>
        int ModelCollectionID { get; }

        /// <summary>
        /// ID of the model this belongs to.
        /// </summary>
        int ModelID { get; }

        /// <summary>
        /// Level-of-detail index for the model.
        /// </summary>
        int LevelOfDetail { get; }

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
