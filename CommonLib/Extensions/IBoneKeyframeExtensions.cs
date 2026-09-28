using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Rigging;

namespace CommonLib.Extensions {
    public static class IBoneKeyframeExtensions {
        public static int GetEarliestKeyframe(this IReadOnlyList<IBoneKeyframe> keyframes) {
            return keyframes.Count > 0
                ? keyframes.Min(x => Math.Min(
                    x.PosTable.Count > 0 ? x.PosTable.Min(y => y.FrameNum) : 0,
                    Math.Min(
                        x.RotTable.Count   > 0 ? x.RotTable  .Min(y => y.FrameNum) : 0,
                        x.ScaleTable.Count > 0 ? x.ScaleTable.Min(y => y.FrameNum) : 0
                    )
                ))
                : 0;
        }

        public static int GetLatestKeyframe(this IReadOnlyList<IBoneKeyframe> keyframes) {
            return keyframes.Count > 0
                ? keyframes.Max(x => Math.Max(
                    x.PosTable.Count > 0 ? x.PosTable.Max(y => y.FrameNum) : 0,
                    Math.Max(
                        x.RotTable.Count   > 0 ? x.RotTable  .Max(y => y.FrameNum) : 0,
                        x.ScaleTable.Count > 0 ? x.ScaleTable.Max(y => y.FrameNum) : 0
                    )
                ))
                : 0;
        }
    }
}
