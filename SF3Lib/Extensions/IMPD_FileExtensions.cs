using System;
using System.Linq;
using CommonLib.Types;
using SF3.MPD;

namespace SF3.Extensions {
    public static class IMPD_FileExtensions {
        public static float GetAverageVertexHeight(this IMPD_Tile tile) {
            return (float) Math.Round(((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Average(x => tile.GetVertexHeight(x) * 16f)) / 16f;
        }
    }
}
