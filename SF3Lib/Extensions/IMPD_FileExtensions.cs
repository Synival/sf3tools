using System;
using System.Linq;
using CommonLib.Types;
using SF3.MPD.Interfaces;

namespace SF3.Extensions {
    public static class IMPD_FileExtensions {
        public static float GetAverageVertexHeight(this IMPD_SurfaceTile tile) {
            return (float) Math.Round(((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Average(x => tile.GetVertexHeight(x) * 16f)) * 2f;
        }
    }
}
