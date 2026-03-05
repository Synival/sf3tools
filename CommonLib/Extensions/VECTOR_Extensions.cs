using System;
using CommonLib.SGL;

namespace CommonLib.Extensions {
    public static class VECTOR_Extensions {
        public struct BoundingBox {
            public VECTOR[] ToVECTORs() {
                return new VECTOR[] {
                    new VECTOR(LeftTopFront.X,    LeftTopFront.Y,    LeftTopFront.Z),
                    new VECTOR(RightBottomBack.X, LeftTopFront.Y,    LeftTopFront.Z),
                    new VECTOR(LeftTopFront.X,    RightBottomBack.Y, LeftTopFront.Z),
                    new VECTOR(RightBottomBack.X, RightBottomBack.Y, LeftTopFront.Z),
                    new VECTOR(LeftTopFront.X,    LeftTopFront.Y,    RightBottomBack.Z),
                    new VECTOR(RightBottomBack.X, LeftTopFront.Y,    RightBottomBack.Z),
                    new VECTOR(LeftTopFront.X,    RightBottomBack.Y, RightBottomBack.Z),
                    new VECTOR(RightBottomBack.X, RightBottomBack.Y, RightBottomBack.Z),
                };
            }

            public VECTOR LeftTopFront;
            public VECTOR RightBottomBack;

            public float Width {
                get => RightBottomBack.X.Float - LeftTopFront.X.Float;
                set {
                    var percent = value / Width;
                    RightBottomBack.X.Float *= percent;
                    LeftTopFront.X.Float    *= percent;
                }
            }

            public float Height {
                get => RightBottomBack.Y.Float - LeftTopFront.Y.Float;
                set {
                    var percent = value / Height;
                    RightBottomBack.Y.Float *= percent;
                    LeftTopFront.Y.Float    *= percent;
                }
            }

            public float Depth {
                get => RightBottomBack.Z.Float - LeftTopFront.Z.Float;
                set {
                    var percent = value / Depth;
                    RightBottomBack.Z.Float *= percent;
                    LeftTopFront.Z.Float    *= percent;
                }
            }
        }

        public static BoundingBox CreateBoundingBox(this VECTOR[] vectors) {
            if (vectors == null || vectors.Length == 0)
                return new BoundingBox { LeftTopFront = new VECTOR(), RightBottomBack = new VECTOR()};

            var leftTopFront    = new VECTOR(vectors[0].X, vectors[0].Y, vectors[0].Z);
            var rightBottomBack = leftTopFront;

            for (int i = 1; i < vectors.Length; i++) {
                var vector = vectors[i];
                if (vector.X < leftTopFront.X)    leftTopFront.X    = vector.X;
                if (vector.Y < leftTopFront.Y)    leftTopFront.Y    = vector.Y;
                if (vector.Z < leftTopFront.Z)    leftTopFront.Z    = vector.Z;
                if (vector.X > rightBottomBack.X) rightBottomBack.X = vector.X;
                if (vector.Y > rightBottomBack.Y) rightBottomBack.Y = vector.Y;
                if (vector.Z > rightBottomBack.Z) rightBottomBack.Z = vector.Z;
            }

            return new BoundingBox { LeftTopFront = leftTopFront, RightBottomBack = rightBottomBack };
        }

        public static VECTOR[] RotateXYZ(this VECTOR[] vectors, float rotateXInDegrees, float rotateYInDegrees, float rotateZInDegrees) {
            // Rotation always happens in this order in SF3.
            return vectors
                .RotateX(rotateXInDegrees)
                .RotateY(rotateYInDegrees)
                .RotateZ(rotateZInDegrees);
        }

        public static VECTOR[] UnrotateXYZ(this VECTOR[] vectors, float rotateXInDegrees, float rotateYInDegrees, float rotateZInDegrees) {
            // Opposite rotations to RotateXYZ().
            return vectors
                .RotateZ(-rotateZInDegrees)
                .RotateY(-rotateYInDegrees)
                .RotateX(-rotateXInDegrees);
        }

        public static VECTOR[] RotateX(this VECTOR[] vectors, float rotateXInDegrees) {
            var sin = (float) Math.Sin(rotateXInDegrees / 180.0f * Math.PI);
            var cos = (float) Math.Cos(rotateXInDegrees / 180.0f * Math.PI);

            var newVectors = new VECTOR[vectors.Length];
            for (int i = 0; i < vectors.Length; i++) {
                var vec = vectors[i];
                var x = vec.X.Float;
                var y = vec.Y.Float;
                var z = vec.Z.Float;
                newVectors[i] = new VECTOR(x, y * cos + z * -sin, sin * y + z * cos);
            }

            return newVectors;
        }

        public static VECTOR[] RotateY(this VECTOR[] vectors, float rotateYInDegrees) {
            var sin = (float) Math.Sin(rotateYInDegrees / 180.0f * Math.PI);
            var cos = (float) Math.Cos(rotateYInDegrees / 180.0f * Math.PI);

            var newVectors = new VECTOR[vectors.Length];
            for (int i = 0; i < vectors.Length; i++) {
                var vec = vectors[i];
                var x = vec.X.Float;
                var y = vec.Y.Float;
                var z = vec.Z.Float;
                newVectors[i] = new VECTOR(x * cos + z * sin, y, x * -sin + z * cos);
            }

            return newVectors;
        }

        public static VECTOR[] RotateZ(this VECTOR[] vectors, float rotateZInDegrees) {
            var sin = (float) Math.Sin(rotateZInDegrees / 180.0f * Math.PI);
            var cos = (float) Math.Cos(rotateZInDegrees / 180.0f * Math.PI);

            var newVectors = new VECTOR[vectors.Length];
            for (int i = 0; i < vectors.Length; i++) {
                var vec = vectors[i];
                var x = vec.X.Float;
                var y = vec.Y.Float;
                var z = vec.Z.Float;
                newVectors[i] = new VECTOR(x * cos + y * -sin, x * sin + y * cos, z);
            }

            return newVectors;
        }

        public static VECTOR[] Scale(this VECTOR[] vectors, float scaleX, float scaleY, float scaleZ) {
            var newVectors = new VECTOR[vectors.Length];
            for (int i = 0; i < vectors.Length; i++) {
                var vec = vectors[i];
                var x = vec.X.Float;
                var y = vec.Y.Float;
                var z = vec.Z.Float;
                newVectors[i] = new VECTOR(x * scaleX, y * scaleY, z * scaleZ);
            }

            return newVectors;
        }
    }
}
