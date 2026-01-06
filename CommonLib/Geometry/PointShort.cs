namespace CommonLib.Geometry {
    public struct PointShort : IPointShort {
        public PointShort(short x, short y) {
            X = x;
            Y = y;
        }

        public PointShort(IPointBase<short> p) {
            X = p.X;
            Y = p.Y;
        }

        public short X { get; set; }
        public short Y { get; set; }
    }
}
