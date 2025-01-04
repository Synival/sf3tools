using CommonLib.SGL;
using CommonLib.Types;

namespace CommonLib.Tests.SGL {
    [TestClass]
    public class POLYGONTests {
        const float c_minDelta = 0.00025f;
        const POLYGON_NormalCalculationMethod c_calculationMethod = POLYGON_NormalCalculationMethod.TopLeftTriangle;

        public void AreVectorsEqual(VECTOR lhs, VECTOR rhs, float delta = 0.00f, string? name = null) {
            var diff = lhs - rhs;
            Assert.IsTrue(Math.Abs(diff.X.Float) <= delta && Math.Abs(diff.Y.Float) <= delta && Math.Abs(diff.Z.Float) <= delta,
                "Vectors " + ((name == null) ? "" : ("'" + name + "' ")) + "not equal: " + lhs + ", " + rhs);
        }

        [TestMethod]
        public void GetAbnormal_With000Slope_ReturnsExpectedAbnormal() {
            var quad = new POLYGON([
                new(10, 20, 30),
                new(11, 20, 30),
                new(11, 20, 31),
                new(10, 20, 31),
            ]);

            var abnormal = quad.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, 0.0000305f, 0.0000000f), abnormal, 0.00001f);
        }

        [TestMethod]
        public void GetAbnormal_With025Slope_ReturnsExpectedAbnormal() {
            var quad = new POLYGON([
                new(10, 20.25f, 30),
                new(11, 20.25f, 30),
                new(11, 20.00f, 31),
                new(10, 20.00f, 31),
            ]);

            var abnormal = quad.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, 0.0154724f, -0.2480469f), abnormal, c_minDelta);
        }

        [TestMethod]
        public void GetAbnormal_With050Slope_ReturnsExpectedAbnormal() {
            var quad = new POLYGON([
                new(10, 20.5f, 30),
                new(11, 20.5f, 30),
                new(11, 20.0f, 31),
                new(10, 20.0f, 31),
            ]);

            var abnormal = quad.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, 0.0597229f, -0.4850158f), abnormal, c_minDelta);
        }

        [TestMethod]
        public void GetAbnormal_With100Slope_ReturnsExpectedAbnormal() {
            var quad = new POLYGON([
                new(10, 21, 30),
                new(11, 21, 30),
                new(11, 20, 31),
                new(10, 20, 31),
            ]);

            var abnormal = quad.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, 0.2111511f, -0.8943786f), abnormal, c_minDelta);
        }

        [TestMethod]
        public void GetAbnormal_With150Slope_ReturnsExpectedAbnormal() {
            var quad = new POLYGON([
                new(10, 21.5f, 30),
                new(11, 21.5f, 30),
                new(11, 20.0f, 31),
                new(10, 20.0f, 31),
            ]);

            var abnormal = quad.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, 0.4000549f, -1.1999817f), abnormal, c_minDelta);
        }

        [TestMethod]
        public void GetAbnormal_With200Slope_ReturnsExpectedAbnormal() {
            var quad = new POLYGON([
                new(10, 22.0f, 30),
                new(11, 22.0f, 30),
                new(11, 20.0f, 31),
                new(10, 20.0f, 31),
            ]);

            var abnormal = quad.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, 0.5858460f, -1.4145400f), abnormal, c_minDelta);
        }

        [TestMethod]
        public void GetAbnormal_CheckNESW_ReturnsExpectedAbnormal() {
            var quadN = new POLYGON([
                new(10, 21, 30),
                new(11, 21, 30),
                new(11, 20, 31),
                new(10, 20, 31),
            ]);
            var quadE = new POLYGON([
                new(10, 20, 30),
                new(11, 21, 30),
                new(11, 21, 31),
                new(10, 20, 31),
            ]);
            var quadS = new POLYGON([
                new(10, 20, 30),
                new(11, 20, 30),
                new(11, 21, 31),
                new(10, 21, 31),
            ]);
            var quadW = new POLYGON([
                new(10, 21, 30),
                new(11, 20, 30),
                new(11, 20, 31),
                new(10, 21, 31),
            ]);

            var abnormalN = quadN.GetAbnormal(c_calculationMethod);
            var abnormalE = quadE.GetAbnormal(c_calculationMethod);
            var abnormalS = quadS.GetAbnormal(c_calculationMethod);
            var abnormalW = quadW.GetAbnormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR( 0.0000000f, 0.2111511f, -0.8943786f), abnormalN, c_minDelta, nameof(abnormalN));
            AreVectorsEqual(new VECTOR(-0.8943786f, 0.2111511f,  0.0000000f), abnormalE, c_minDelta, nameof(abnormalE));
            AreVectorsEqual(new VECTOR( 0.0000000f, 0.2111511f,  0.8943786f), abnormalS, c_minDelta, nameof(abnormalS));
            AreVectorsEqual(new VECTOR( 0.8943786f, 0.2111511f,  0.0000000f), abnormalW, c_minDelta, nameof(abnormalW));
        }

        [TestMethod]
        public void GetNormal_With000Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 20, 30),
                new(11, 20, 30),
                new(11, 20, 31),
                new(10, 20, 31),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0, 1, 0), normal);
        }

        [TestMethod]
        public void GetNormal_With100Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 21, 30),
                new(11, 21, 30),
                new(11, 20, 31),
                new(10, 20, 31),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0, 0.70710678118f, 0.70710678118f), normal, 0.0001f);
        }
    }
}
