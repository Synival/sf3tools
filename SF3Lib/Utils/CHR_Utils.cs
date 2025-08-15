using SF3.Types;

namespace SF3.Utils {
    public static class CHR_Utils {
        /// <summary>
        /// Updates transparency pixels of a sprite image to indicate start and stop of rows.
        /// (Why SF3 does this is a mystery to me, but it's necessary to prevent visual bugs)
        /// </summary>
        /// <param name="frameImage">Image to encode.</param>
        /// <param name="useEncodingPixels">When true, the encoding pixels that indicate row start/end are set.</param>
        public static void EncodeSpriteFrameImage(ushort[,] frameImage, bool useEncodingPixels) {
            var width  = frameImage.GetLength(0);
            var height = frameImage.GetLength(1);

            // First, force all transparent pixels to be 0x0000. (This is a bit inefficient but makes life easier.)
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if ((frameImage[x, y] & 0x8000) == 0)
                        frameImage[x, y] = 0;

            // We don't have to do anything more if we're not adding encoding pixels.
            if (!useEncodingPixels)
                return;

            // Go row-by-row, applying 0x7FFF pixel encoding.
            for (int y = 0; y < height; y++) {
                // Get the first solid pixel on the left side.
                int firstSolid = -1;
                for (int x = 0; x < width; x++) {
                    if ((frameImage[x, y] & 0x8000) != 0) {
                        firstSolid = x;
                        break;
                    }
                }

                // If there was no solid pixel, the row is completely transparent. Use a special encoding.
                if (firstSolid == -1) {
                    if (width <= 4) {
                        for (int x = 0; x < width; x++)
                            frameImage[x, y] = 0x7FFF;
                    }
                    else {
                        frameImage[0, y] = 0x7FFF;
                        frameImage[1, y] = 0x7FFF;
                        frameImage[width - 2, y] = 0x7FFF;
                        frameImage[width - 1, y] = 0x7FFF;
                    }
                    continue;
                }

                // Get the first solid pixel on the right side.
                int lastSolid = -1;
                for (int x = width - 1; x >= 0; x--) {
                    if ((frameImage[x, y] & 0x8000) != 0) {
                        lastSolid = x;
                        break;
                    }
                }

                // Apply encoding pixel if there are at least 2 transparent pixels.
                if (firstSolid >= 2)
                    frameImage[firstSolid - 1, y] = 0x7FFF;
                if (lastSolid <= width - 3)
                    frameImage[lastSolid + 1, y] = 0x7FFF;
            }
        }

        public static SpriteFrameDirection GetFrameGroupDirection(int dirs, int num) {
            switch (dirs) {
                case 4:
                case 6:
                    switch (num) {
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        case 3:  return SpriteFrameDirection.NNE;
                        case 4:  return SpriteFrameDirection.S;
                        default: return SpriteFrameDirection.N;
                    }

                case 8:
                    switch (num) {
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        case 3:  return SpriteFrameDirection.NNE;
                        case 4:  return SpriteFrameDirection.NNW;
                        case 5:  return SpriteFrameDirection.WNW;
                        case 6:  return SpriteFrameDirection.WSW;
                        default: return SpriteFrameDirection.SSW;
                    }

                case 5:
                    switch (num) {
                        case 0:  return SpriteFrameDirection.S;
                        case 1:  return SpriteFrameDirection.SE;
                        case 2:  return SpriteFrameDirection.E;
                        case 3:  return SpriteFrameDirection.NE;
                        default: return SpriteFrameDirection.N;
                    }

                default:
                    return SpriteFrameDirection.First + num;
            }
        }

        public static SpriteFrameDirection[] GetFrameGroupDirections(int directions) {
            switch (directions) {
                case 1:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.First,
                    };

                case 2:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.First,
                        SpriteFrameDirection.Second,
                    };

                case 4:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                    };

                case 5:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.S,
                        SpriteFrameDirection.SE,
                        SpriteFrameDirection.E,
                        SpriteFrameDirection.NE,
                        SpriteFrameDirection.N,
                    };

                case 6:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.S,
                        SpriteFrameDirection.N,
                    };

                case 8:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.NNW,
                        SpriteFrameDirection.WNW,
                        SpriteFrameDirection.WSW,
                        SpriteFrameDirection.SSW,
                    };

                default:
                    return null;
            }
        }
    }
}
