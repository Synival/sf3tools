namespace CommonLib.SGL {
    public interface IPDATA {
        uint VerticesOffset { get; }
        int VertexCount { get; }
        uint PolygonsOffset { get; }
        int FaceCount { get; }
        uint AttributesOffset { get; }
    }
}
