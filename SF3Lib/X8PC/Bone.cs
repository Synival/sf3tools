using System.Collections.Generic;
using CommonLib.SGL;

namespace SF3.X8PC {
    public class Bone {
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

        public int? BoneID { get; set; }

        public int? ModelID { get; set; }
        public int? Tag { get; set; }
        public VECTOR? Position { get; set; }
        public QUATERNION? Rotation { get; set; }
        public VECTOR? Scale { get; set; }

        public Bone[] Children { get; set; }
    }
}
