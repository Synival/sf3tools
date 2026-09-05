using System;
using System.Collections.Generic;
using System.Numerics;
using CommonLib.SGL;

namespace SF3.X8PC {
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

    public static class IBoneExtensions {
        public static string ToOutline(this IBone thisBone, int indentation = 0) {
            string str = new string(' ', indentation * 2);

            if (thisBone.BoneID.HasValue)
                str += $" bone=0x{thisBone.BoneID:X2}";
            if (thisBone.ModelID.HasValue)
                str += $" model=0x{thisBone.ModelID.Value:X2}";
            if (thisBone.Tag.HasValue)
                str += $" tag=0x{thisBone.Tag.Value:X2}";
            if (thisBone.Position != null)
                str += $" pos={thisBone.Position.Value}";
            if (thisBone.Rotation != null)
                str += $" rot={thisBone.Rotation.Value}";
            if (thisBone.Scale != null)
                str += $" scale={thisBone.Scale.Value}";

            if (thisBone.Children != null && thisBone.Children.Length > 0) {
                str += " {\n";
                foreach (var bone in thisBone.Children)
                    str += bone.ToOutline(indentation + 1);
                str += new string(' ', indentation * 2) + "}";
            }

            return str.Substring(str.Length > 0 ? 1 : 0) + "\n";
        }

        public static IBone[] Flatten(this IBone thisBone) {
            var bones = new List<IBone> { thisBone };
            if (thisBone.Children != null)
                foreach (var bone in thisBone.Children)
                    bones.AddRange(bone.Flatten());
            return bones.ToArray();
        }

        public static IBone FindTraverse(this IBone thisBone, Func<IBone, bool> pred) {
            if (pred(thisBone))
                return thisBone;
            if (thisBone.Children == null)
                return null;
            foreach (var bone in thisBone.Children) {
                var found = bone.FindTraverse(pred);
                if (found != null)
                    return found;
            }
            return null;
        }

        public static string GetBonePath(this IBone thisBone) {
            var boneName = "";
            for (var bone = thisBone; bone != null; bone = bone.Parent)
                boneName = bone.BoneID.HasValue ? ($"{bone.BoneID:D2}" + (boneName == "" ? "" : $".{boneName}")) : boneName;
            return boneName;
        }

        public static Matrix4x4 CreateMatrix(this IBone bone) {
            var pos  = bone.Position.Value;
            var rotQ1 = bone.Rotation.Value;
            var rotQ2 = new Quaternion(rotQ1.X.Float, rotQ1.Y.Float, rotQ1.Z.Float, rotQ1.W.Float);
            var scale = bone.Scale.Value;

            return Matrix4x4.CreateScale(new Vector3(scale.X.Float, scale.Y.Float, scale.Z.Float))
                 * Matrix4x4.CreateFromQuaternion(rotQ2)
                 * Matrix4x4.CreateTranslation(pos.X.Float / 32.0f, pos.Y.Float / -32.0f, pos.Z.Float / -32.0f);
        }
    }
}
