namespace CommonLib.SGL {
    public interface ISGL_ModelFace {
        IIndexedEnumerableWithLength<int> VertexIndices { get; }
        VECTOR Normal { get; set; }
        IATTR Attributes { get; set; }
    }
}
