namespace CommonLib.Geometry {
    public struct RectangleShort : IRectangleShort {
        public IPointShort P1 { get; set; }
        public IPointShort P2 { get; set; }

        public short X1 { get => P1.X; set => P1.X = value; }
        public short Y1 { get => P1.Y; set => P1.Y = value; }
        public short X2 { get => P2.X; set => P2.X = value; }
        public short Y2 { get => P2.Y; set => P2.Y = value; }

        public int Width { get => X2 - X1; set => X2 = (short) (X1 + value); }
        public int Height { get => Y2 - Y1; set => Y2 = (short) (Y1 + value); }

        IPointBase<short> IRectangleBase<short, int>.P1 { get => P1; set => P1 = (IPointShort) value; }
        IPointBase<short> IRectangleBase<short, int>.P2 { get => P2; set => P2 = (IPointShort) value; }
    }
}
