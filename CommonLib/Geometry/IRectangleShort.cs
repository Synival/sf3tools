namespace CommonLib.Geometry {
    public interface IRectangleShort : IRectangleBase<short, int> {
        new IPointShort P1 { get; set; }
        new IPointShort P2 { get; set; }
    }
}
