namespace CommonLib.SGL {
    public interface ISGL_Model {
        int ID { get; }
        IIndexedEnumerableWithLength<VECTOR> Vertices { get; }
        IIndexedEnumerableWithLength<ISGL_ModelFace> Faces { get; }
    }
}
