using System;
using System.Collections.Generic;
using CommonLib.SGL;

namespace SF3.X8PC {
    public class Bone {
        /// <summary>
        /// Constructor for a root bone.
        /// </summary>
        public Bone(Bone[] children) {
            Children = children;
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

        public string ToOutline(int indentation = 0) {
            string str = new string(' ', indentation * 2);

            if (BoneID.HasValue)
                str += $" bone=0x{BoneID:X2}";
            if (ModelID.HasValue)
                str += $" model=0x{ModelID.Value:X2}";
            if (Tag.HasValue)
                str += $" tag=0x{Tag.Value:X2}";
            if (Position != null)
                str += $" pos={Position.Value}";
            if (Rotation != null)
                str += $" rot={Rotation.Value}";
            if (Scale != null)
                str += $" scale={Scale.Value}";

            if (Children != null && Children.Length > 0) {
                str += " {\n";
                foreach (var bone in Children)
                    str += bone.ToOutline(indentation + 1);
                str += new string(' ', indentation * 2) + "}";
            }

            return str.Substring(str.Length > 0 ? 1 : 0) + "\n";
        }

        public Bone[] Flatten() {
            var bones = new List<Bone> { this };
            if (Children != null)
                foreach (var bone in Children)
                    bones.AddRange(bone.Flatten());
            return bones.ToArray();
        }

        public Bone FindTraverse(Func<Bone, bool> pred) {
            if (pred(this))
                return this;
            if (Children == null)
                return null;
            foreach (var bone in Children) {
                var found = bone.FindTraverse(pred);
                if (found != null)
                    return found;
            }
            return null;
        }

        public string GetBonePath() {
            var boneName = "";
            for (var bone = this; bone != null; bone = bone.Parent)
                boneName = bone.BoneID.HasValue ? ($"0x{bone.BoneID:X02}" + (boneName == "" ? "" : $".{boneName}")) : boneName;
            return boneName;
        }

        public int? Tag { get; }
        public Bone Parent { get; private set; }
        public int? BoneID { get; }

        public int? ModelID { get; }
        public VECTOR? Position { get; }

        public QUATERNION? Rotation { get; }
        public VECTOR? Scale { get; }

        public Bone[] Children { get; }
    }
}
