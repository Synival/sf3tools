using CommonLib.SGL;

namespace CommonLib.Rigging {
    public interface IBone {
        int? Tag { get; }
        IBone Parent { get; }
        int? BoneID { get; }

        int? ModelID { get; }
        VECTOR? Position { get; }

        QUATERNION? Rotation { get; }
        VECTOR? Scale { get; }

        IBone[] Children { get; }
    }
}
