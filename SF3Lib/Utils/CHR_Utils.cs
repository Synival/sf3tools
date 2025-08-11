using SF3.Types;

namespace SF3.Utils {
    public static class CHR_Utils {
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
