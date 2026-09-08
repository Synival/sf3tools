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
        public static string ToOutline(this IBone thisBone)
            => ToOutlineSub(thisBone, 0);

        private static string ToOutlineSub(this IBone thisBone, int indentation = 0) {
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
                str += " {\r\n";
                foreach (var bone in thisBone.Children)
                    str += bone.ToOutlineSub(indentation + 1);
                str += new string(' ', indentation * 2) + "}";
            }

            return str.Substring(str.Length > 0 ? 1 : 0) + "\r\n";
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

        public static Matrix4x4 CreateMatrix(this IBone bone)
            => CreateMatrix(bone.Position, bone.Rotation, bone.Scale);

        public static Matrix4x4 CreateMatrix(VECTOR? posIn, QUATERNION? rotIn, VECTOR? scaleIn) {
            var matrix = Matrix4x4.Identity;

            if (scaleIn.HasValue) {
                var scale = scaleIn.Value;
                matrix *= Matrix4x4.CreateScale(new Vector3(scale.X.Float, scale.Y.Float, scale.Z.Float));
            }

            if (rotIn.HasValue) {
                var rotQ1 = rotIn.Value;
                var rotQ2 = new Quaternion(rotQ1.X.Float, -rotQ1.Y.Float, -rotQ1.Z.Float, rotQ1.W.Float);
                matrix *= Matrix4x4.CreateFromQuaternion(rotQ2);
            }

            if (posIn.HasValue) {
                var pos = posIn.Value;
                matrix *= Matrix4x4.CreateTranslation(pos.X.Float, -pos.Y.Float, -pos.Z.Float);
            }

            return matrix;
        }

        public static Matrix4x4 CreateMatrix(
            VECTOR pos1, VECTOR pos2, float posMix,
            QUATERNION rot1, QUATERNION rot2, float rotMix,
            VECTOR scale1, VECTOR scale2, float scaleMix
        ) {
            var scale = Vector3.Lerp(
                new Vector3(scale1.X.Float, scale1.Y.Float, scale1.Z.Float),
                new Vector3(scale2.X.Float, scale2.Y.Float, scale2.Z.Float),
                scaleMix
            );

            var rot = Quaternion.Lerp(
                new Quaternion(rot1.X.Float, -rot1.Y.Float, -rot1.Z.Float, rot1.W.Float),
                new Quaternion(rot2.X.Float, -rot2.Y.Float, -rot2.Z.Float, rot2.W.Float),
                rotMix
            );

            var pos = Vector3.Lerp(
                new Vector3(pos1.X.Float, -pos1.Y.Float, -pos1.Z.Float),
                new Vector3(pos2.X.Float, -pos2.Y.Float, -pos2.Z.Float),
                posMix
            );

            return Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rot) * Matrix4x4.CreateTranslation(pos);
        }
    }
}
