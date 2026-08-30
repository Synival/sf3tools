using CommonLib.SGL;

namespace SF3.X8PC {
    public class Bone : IBone {
        /// <summary>
        /// Constructor for a root bone.
        /// </summary>
        public Bone(Bone[] children) {
            Children = children;
            foreach (var bone in children)
                bone.Parent = this;
        }

        /// <summary>
        /// Constructor for a child bone.
        /// </summary>
        public Bone(int boneId, Bone[] children) {
            BoneID   = boneId;
            Tag      = 0xFD;
            Children = children;
            foreach (var bone in children)
                bone.Parent = this;
        }

        /// <summary>
        /// Constructor for a model to render.
        /// </summary>
        public Bone(int tag, int modelId) {
            Tag     = tag;
            ModelID = modelId;
        }

        /// <summary>
        /// Constructor for a location tag.
        /// </summary>
        public Bone(int tag, VECTOR pos) {
            Tag      = tag;
            Position = pos;
        }

        /// <summary>
        /// Constructor for a weapon placement (tag 0x30 or 0x81).
        /// </summary>
        public Bone(int tag, VECTOR pos, QUATERNION rot, VECTOR scale) {
            Tag      = tag;
            Position = pos;
            Rotation = rot;
            Scale    = scale;
        }

        public int? Tag { get; }
        public IBone Parent { get; private set; }
        public int? BoneID { get; }

        public int? ModelID { get; }
        public VECTOR? Position { get; }

        public QUATERNION? Rotation { get; }
        public VECTOR? Scale { get; }

        public IBone[] Children { get; }
    }
}
