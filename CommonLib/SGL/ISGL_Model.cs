namespace CommonLib.SGL {
    public interface ISGL_Model {
        int ID { get; set; }
        IIndexedEnumerableWithLength<VECTOR> Vertices { get; }
        IIndexedEnumerableWithLength<ISGL_ModelFace> Faces { get; }
    }
}
