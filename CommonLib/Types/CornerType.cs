using System;

namespace CommonLib.Types {
    /// <summary>
    /// Corners for quads, starting at 0 going counter-clockwise.
    /// This matches the order seen used in SGL textured quads.
    /// </summary>
    public enum CornerType {
        TopRight    = 0,
        TopLeft     = 1,
        BottomLeft  = 2,
        BottomRight = 3,
    }

    public static class CornerTypeConsts {
        // X coordinate in OpenGL space is (0:left --> 1:right).
        public const int LeftX = 0;
        public const int RightX = 1;

        // Y coordinate in OpenGL space is (0:bottom -> 1:top).
        // Use Top/BottomY (not Z) for the corner with front-facing polygons.
        public const int TopY = 1;
        public const int BottomY = 0;

        // Z coordinate in OpenGL space is (0:back -> 1:front).
        // Use Top/BottomZ (not Y) for the corner with top-facing polygons.
        public const int TopZ = 0;
        public const int BottomZ = 1;

        // X coordinate in texture space is (0:left --> 1:right).
        public const int LeftUVX = 0;
        public const int RightUVX = 1;

        // Y coordinate in texture space is (0:top --> 1:bottom).
        public const int TopUVY = 0;
        public const int BottomUVY = 1;

        // X coordinate in OpenGL space in range (0..1) for vertices in CornerType order.
        public const int Corner1X = RightX;
        public const int Corner2X = LeftX;
        public const int Corner3X = LeftX;
        public const int Corner4X = RightX;

        // Y coordinate in OpenGL space in range (0..1) for front-facing vertices in CornerType order.
        public const int Corner1Y = TopY;
        public const int Corner2Y = TopY;
        public const int Corner3Y = BottomY;
        public const int Corner4Y = BottomY;

        // Z coordinate in OpenGL space in range (0..1) for top-facing vertices in CornerType order.
        public const int Corner1Z = TopZ;
        public const int Corner2Z = TopZ;
        public const int Corner3Z = BottomZ;
        public const int Corner4Z = BottomZ;

        // X coordinate in OpenGL space in range (-1..1) for vertices in CornerType order.
        public const int Corner1DirX = RightX * 2 - 1;
        public const int Corner2DirX = LeftX * 2 - 1;
        public const int Corner3DirX = LeftX * 2 - 1;
        public const int Corner4DirX = RightX * 2 - 1;

        // Y coordinate in OpenGL space in range (-1..1) for front-facing vertices in CornerType order.
        public const int Corner1DirY = TopY * 2 - 1;
        public const int Corner2DirY = TopY * 2 - 1;
        public const int Corner3DirY = BottomY * 2 - 1;
        public const int Corner4DirY = BottomY * 2 - 1;

        // Z coordinate in OpenGL space in range (-1..1) for top-facing vertices in CornerType order.
        public const int Corner1DirZ = TopZ * 2 - 1;
        public const int Corner2DirZ = TopZ * 2 - 1;
        public const int Corner3DirZ = BottomZ * 2 - 1;
        public const int Corner4DirZ = BottomZ * 2 - 1;

        // X coordinate in texture space in range (0..1) for vertices in CornerType order.
        public const int Corner1UVX = RightUVX;
        public const int Corner2UVX = LeftUVX;
        public const int Corner3UVX = LeftUVX;
        public const int Corner4UVX = RightUVX;

        // Y coordinate in texture space in range (0..1) for vertices in CornerType order.
        public const int Corner1UVY = TopUVY;
        public const int Corner2UVY = TopUVY;
        public const int Corner3UVY = BottomUVY;
        public const int Corner4UVY = BottomUVY;
    }

    public static class CornerTypeExtensions {
        public static bool IsLeftSide(this CornerType type) => (type == CornerType.TopLeft || type == CornerType.BottomLeft);
        public static bool IsTopSide(this CornerType type) => (type == CornerType.TopLeft || type == CornerType.TopRight);

        public static int GetVertexOffsetX(this CornerType type) => type.IsLeftSide() ? CornerTypeConsts.LeftX : CornerTypeConsts.RightX;
        public static int GetVertexOffsetY(this CornerType type) => type.IsTopSide() ? CornerTypeConsts.TopY : CornerTypeConsts.BottomY;
        public static int GetVertexOffsetZ(this CornerType type) => type.IsTopSide() ? CornerTypeConsts.TopZ : CornerTypeConsts.BottomZ;

        public static int GetDirectionX(this CornerType type) => -1 + type.GetVertexOffsetX() * 2;
        public static int GetDirectionY(this CornerType type) => -1 + type.GetVertexOffsetY() * 2;
        public static int GetDirectionZ(this CornerType type) => -1 + type.GetVertexOffsetZ() * 2;

        public static int GetUVX(this CornerType type) => type.IsLeftSide() ? CornerTypeConsts.LeftUVX : CornerTypeConsts.RightUVX;
        public static int GetUVY(this CornerType type) => type.IsTopSide() ? CornerTypeConsts.TopUVY : CornerTypeConsts.BottomUVY;

        public static int GetUVDirectionX(this CornerType type) => -1 + type.GetUVX() * 2;
        public static int GetUVDirectionY(this CornerType type) => -1 + type.GetUVY() * 2;

        public static CornerType FromVertexOffsetXY(int x, int y) {
            if (x == CornerTypeConsts.LeftX) {
                if (y == CornerTypeConsts.TopY)
                    return CornerType.TopLeft;
                else if (y == CornerTypeConsts.BottomY)
                    return CornerType.BottomLeft;
            }
            else if (x == CornerTypeConsts.RightX) {
                if (y == CornerTypeConsts.TopY)
                    return CornerType.TopRight;
                else if (y == CornerTypeConsts.BottomY)
                    return CornerType.BottomRight;
            }

            throw new ArgumentException(nameof(x) + ", " + nameof(y));
        }

        public static CornerType FromVertexOffsetXZ(int x, int z) {
            if (x == CornerTypeConsts.LeftX) {
                if (z == CornerTypeConsts.TopZ)
                    return CornerType.TopLeft;
                else if (z == CornerTypeConsts.BottomZ)
                    return CornerType.BottomLeft;
            }
            else if (x == CornerTypeConsts.RightX) {
                if (z == CornerTypeConsts.TopZ)
                    return CornerType.TopRight;
                else if (z == CornerTypeConsts.BottomZ)
                    return CornerType.BottomRight;
            }

            throw new ArgumentException(nameof(x) + ", " + nameof(z));
        }
    }
}
