namespace CommonLib.SGL {
    public interface ISGL_Model {
        /// <summary>
        /// All vertices for the model.
        /// </summary>
        IIndexedEnumerableWithLength<VECTOR> Vertices { get; }

        /// <summary>
        /// All faces (quads) for the model. Contains the set of vertices to use for each quad and its ATTR information.
        /// </summary>
        IIndexedEnumerableWithLength<ISGL_ModelFace> Faces { get; }

        /// <summary>
        /// Lowest (in visual space) Y component of all vertices.
        /// </summary>
        float TopY { get; }

        /// <summary>
        /// Lowest (in visual space) Y component of all vertices.
        /// </summary>
        float BottomY { get; }
    }
}
