namespace SF3.Types {
    /// <summary>
    /// Flags for filling/connectivity for a tile that can be filled or connectivity in the URDL directions,
    /// including diagonals and a center. Also has additional flags for how the tile may behave.
    /// </summary>
    public enum TileFill {
        None = 0x0000,

        U  = 0x0001,
        R  = 0x0002,
        D  = 0x0004,
        L  = 0x0008,

        UL = 0x0010,
        UR = 0x0020,
        DR = 0x0040,
        DL = 0x0080,

        C  = 0x0100,

        Full = 0x01FF,

        SteepUL = 0x1000,
        SteepUR = 0x2000,
        SteepDR = 0x4000,
        SteepDL = 0x8000,
    }

    public static class TileFillExtensions {
        public static string FlagString(this TileFill fill) {
            string result = "";

            var baseFill = (int) fill & 0x1FF;
            if (baseFill == 0x1FF)
                result += "*";
            else {
                if (fill.HasFlag(TileFill.UL)) result += "1";
                if (fill.HasFlag(TileFill.U))  result += "U";
                if (fill.HasFlag(TileFill.UR)) result += "2";
                if (fill.HasFlag(TileFill.R))  result += "R";
                if (fill.HasFlag(TileFill.DR)) result += "3";
                if (fill.HasFlag(TileFill.D))  result += "D";
                if (fill.HasFlag(TileFill.DL)) result += "4";
                if (fill.HasFlag(TileFill.L))  result += "L";
                if (fill.HasFlag(TileFill.C))  result += "C";
            }

            if (fill.HasFlag(TileFill.SteepUL)) result += "!";
            if (fill.HasFlag(TileFill.SteepUR)) result += "@";
            if (fill.HasFlag(TileFill.SteepDR)) result += "#";
            if (fill.HasFlag(TileFill.SteepDL)) result += "$";

            return result;
        }

        private static readonly TileFill[,] _offsetToTileFillBit = new TileFill[3, 3] {
            { TileFill.DL, TileFill.L,  TileFill.UL },
            { TileFill.D,  TileFill.C,  TileFill.U  },
            { TileFill.DR, TileFill.R,  TileFill.UR },
        };

        public static TileFill GetTileFillBitForPosition(int x, int y)
            => _offsetToTileFillBit[x + 1, y + 1];
    }
}
