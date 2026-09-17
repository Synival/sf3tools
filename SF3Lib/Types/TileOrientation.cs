using System;

namespace SF3.Types {
    public enum TileOrientation {
        Normal,
        FlipH,
        FlipV,
        FlipHV,
        Rotate90CW,
        Rotate270CW,
        FlipCornersURandDL,
        FlipCornersULandDR
    }

    public static class TileOrientationExtensions {
        public static TileTextureFlipType GetTextureFlip(this TileOrientation tileOrientation) {
            switch (tileOrientation) {
                case TileOrientation.Normal:
                case TileOrientation.Rotate90CW:
                case TileOrientation.Rotate270CW:
                    return TileTextureFlipType.NoFlip;
                case TileOrientation.FlipH:
                case TileOrientation.FlipCornersURandDL:
                    return TileTextureFlipType.Horizontal;
                case TileOrientation.FlipV:
                case TileOrientation.FlipCornersULandDR:
                    return TileTextureFlipType.Vertical;
                case TileOrientation.FlipHV:
                    return TileTextureFlipType.Both;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static TileTextureRotateType GetTextureRotate(this TileOrientation tileOrientation) {
            switch (tileOrientation) {
                case TileOrientation.Normal:
                case TileOrientation.FlipH:
                case TileOrientation.FlipV:
                case TileOrientation.FlipHV:
                    return TileTextureRotateType.NoRotation;

                case TileOrientation.Rotate90CW:
                case TileOrientation.FlipCornersURandDL:
                case TileOrientation.FlipCornersULandDR:
                    return TileTextureRotateType.Rotate90CW;

                case TileOrientation.Rotate270CW:
                    return TileTextureRotateType.Rotate270CW;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
