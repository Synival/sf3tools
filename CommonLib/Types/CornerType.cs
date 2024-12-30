namespace CommonLib.Types {
    /// <summary>
    /// Corners for quads, starting at 0 going clockwise.
    /// </summary>
    public enum CornerType {
        TopLeft     = 0,
        TopRight    = 1,
        BottomRight = 2,
        BottomLeft  = 3,
    }

    public static class CornerTypeExtensions {
        public static bool IsLeftSide(this CornerType type) => (type == CornerType.TopLeft || type == CornerType.BottomLeft);
        public static bool IsTopSide(this CornerType type) => (type == CornerType.TopLeft || type == CornerType.TopRight);

        public static int GetX(this CornerType type) => type.IsLeftSide() ? 0 : 1;
        public static int GetY(this CornerType type) => type.IsTopSide() ? 0 : 1;
    }
}
