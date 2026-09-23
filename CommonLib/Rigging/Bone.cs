using System.Linq;
using CommonLib.SGL;

namespace CommonLib.Rigging {
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
        /// Copy constructor for a bone.
        /// </summary>
        /// <param name="bone"></param>
        public Bone(IBone bone, IBone parent = null) {
            Parent   = parent;
            Tag      = bone.Tag;
            BoneID   = bone.BoneID;

            ModelID  = bone.ModelID;
            Position = bone.Position;
            Rotation = bone.Rotation;
            Scale    = bone.Scale;

            if (bone.Children != null)
                Children = bone.Children.Select(x => new Bone(x, this)).ToArray();
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
        public Bone(int tag, VECTOR? pos) {
            Tag      = tag;
            Position = pos;
        }

        /// <summary>
        /// Constructor for a weapon placement (tag 0x30 or 0x81).
        /// </summary>
        public Bone(int tag, VECTOR? pos, QUATERNION? rot, VECTOR? scale) {
            Tag      = tag;
            Position = pos;
            Rotation = rot;
            Scale    = scale;
        }

        public int? Tag { get; set; }
        public IBone Parent { get; set; }
        public int? BoneID { get; set; }

        public int? ModelID { get; set; }
        public VECTOR? Position { get; set; }
        public QUATERNION? Rotation { get; set; }
        public VECTOR? Scale { get; set; }

        public IBone[] Children { get; set; }
    }
}
