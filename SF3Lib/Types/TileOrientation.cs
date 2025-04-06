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
        public static TextureFlipType GetTextureFlip(this TileOrientation tileOrientation) {
            switch (tileOrientation) {
                case TileOrientation.Normal:
                case TileOrientation.Rotate90CW:
                case TileOrientation.Rotate270CW:
                    return TextureFlipType.NoFlip;
                case TileOrientation.FlipH:
                case TileOrientation.FlipCornersURandDL:
                    return TextureFlipType.Horizontal;
                case TileOrientation.FlipV:
                case TileOrientation.FlipCornersULandDR:
                    return TextureFlipType.Vertical;
                case TileOrientation.FlipHV:
                    return TextureFlipType.Both;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static TextureRotateType GetTextureRotate(this TileOrientation tileOrientation) {
            switch (tileOrientation) {
                case TileOrientation.Normal:
                case TileOrientation.FlipH:
                case TileOrientation.FlipV:
                case TileOrientation.FlipHV:
                    return TextureRotateType.NoRotation;

                case TileOrientation.Rotate90CW:
                case TileOrientation.FlipCornersURandDL:
                case TileOrientation.FlipCornersULandDR:
                    return TextureRotateType.Rotate90CW;

                case TileOrientation.Rotate270CW:
                    return TextureRotateType.Rotate270CW;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
