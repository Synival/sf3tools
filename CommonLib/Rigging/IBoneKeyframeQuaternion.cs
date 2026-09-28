using CommonLib.SGL;

namespace CommonLib.Rigging {
    public interface IBoneKeyframeQuaternion : IBoneKeyframeValueBase {
        QUATERNION Quaternion { get; }
    }
}
