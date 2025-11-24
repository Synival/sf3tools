using System;
using System.Linq;
using CommonLib.Types;
using SF3.MPD;

namespace SF3.Extensions {
    public static class IMPD_FileExtensions {
        public static float GetAverageVisualVertexHeight(this IMPD_Tile tile) {
            return (float) Math.Round(((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Average(x => tile.GetVisualVertexHeight(x) * 16f)) / 16f;
        }
    }
}
