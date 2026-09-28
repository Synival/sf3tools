using System.Collections.Generic;

namespace CommonLib.Rigging {
    public interface IBoneKeyframe {
        IReadOnlyList<IBoneKeyframeVector> PosTable { get; }
        IReadOnlyList<IBoneKeyframeQuaternion> RotTable { get; }
        IReadOnlyList<IBoneKeyframeVector> ScaleTable { get; }
    }
}
