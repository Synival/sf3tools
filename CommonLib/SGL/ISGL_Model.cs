namespace CommonLib.SGL {
    public interface ISGL_Model {
        IIndexedEnumerableWithLength<VECTOR> Vertices { get; }
        IIndexedEnumerableWithLength<ISGL_ModelFace> Faces { get; }
    }
}
