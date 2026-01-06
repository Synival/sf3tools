namespace CommonLib.Geometry {
    public interface IRectangleBase<TPoint, TDimension> {
        IPointBase<TPoint> P1 { get; set; }
        IPointBase<TPoint> P2 { get; set; }

        TPoint X1 { get; set; }
        TPoint Y1 { get; set; }
        TPoint X2 { get; set; }
        TPoint Y2 { get; set; }

        TDimension Width { get; set; }
        TDimension Height { get; set; }
    }
}
