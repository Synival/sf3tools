namespace CommonLib.Types {
    /// <summary>
    /// Corners for quads, starting at 0 going clockwise.
    /// </summary>
    public enum CornerType {
        BottomLeft  = 0,
        BottomRight = 1,
        TopRight    = 2,
        TopLeft     = 3,
    }

    public static class CornerTypeExtensions {
        public static bool IsLeftSide(this CornerType type) => (type == CornerType.TopLeft || type == CornerType.BottomLeft);
        public static bool IsTopSide(this CornerType type) => (type == CornerType.TopLeft || type == CornerType.TopRight);

        public static int GetVertexOffsetX(this CornerType type) => type.IsLeftSide() ? 0 : 1;
        public static int GetVertexOffsetY(this CornerType type) => type.IsTopSide() ? 1 : 0;
    }
}
