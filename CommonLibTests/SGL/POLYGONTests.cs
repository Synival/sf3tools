using CommonLib.SGL;
using CommonLib.Types;

namespace CommonLib.Tests.SGL {
    [TestClass]
    public class POLYGONTests {
        const float c_minDelta = 0.00025f;
        const POLYGON_NormalCalculationMethod c_calculationMethod = POLYGON_NormalCalculationMethod.TopRightTriangle;

        public void AreVectorsEqual(VECTOR lhs, VECTOR rhs, float delta = 0.00f, string? name = null) {
            var diff = lhs - rhs;
            Assert.IsTrue(Math.Abs(diff.X.Float) <= delta && Math.Abs(diff.Y.Float) <= delta && Math.Abs(diff.Z.Float) <= delta,
                "Vectors " + ((name == null) ? "" : ("'" + name + "' ")) + "not equal: " + lhs + ", " + rhs);
        }

        [TestMethod]
        public void GetNormal_With000Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 20, 31),
                new(11, 20, 31),
                new(11, 20, 30),
                new(10, 20, 30),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0, -1, 0), normal);
        }

        [TestMethod]
        public void GetNormal_With025Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 20.25f, 31),
                new(11, 20.25f, 31),
                new(11, 20.00f, 30),
                new(10, 20.00f, 30),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.97013855f, 0.2425232f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_With050Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 20.5f, 31),
                new(11, 20.5f, 31),
                new(11, 20.0f, 30),
                new(10, 20.0f, 30),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.89442444f, 0.4472046f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_With100Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 21, 31),
                new(11, 21, 31),
                new(11, 20, 30),
                new(10, 20, 30),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0, -0.70710678118f, 0.70710678118f), normal, 0.0001f);
        }

        [TestMethod]
        public void GetNormal_With150Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 21.5f, 31),
                new(11, 21.5f, 31),
                new(11, 20.0f, 30),
                new(10, 20.0f, 30),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.5546875f, 0.8320465f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_With200Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(10, 22.0f, 31),
                new(11, 22.0f, 31),
                new(11, 20.0f, 30),
                new(10, 20.0f, 30),
            ]);

            var normal = quad.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.4472046f, 0.89442444f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_CheckNESW_ReturnsExpectedNormal() {
            var quadN = new POLYGON([
                new(10, 21, 31),
                new(11, 21, 31),
                new(11, 20, 30),
                new(10, 20, 30),
            ]);
            var quadE = new POLYGON([
                new(10, 20, 31),
                new(11, 21, 31),
                new(11, 21, 30),
                new(10, 20, 30),
            ]);
            var quadS = new POLYGON([
                new(10, 20, 31),
                new(11, 20, 31),
                new(11, 21, 30),
                new(10, 21, 30),
            ]);
            var quadW = new POLYGON([
                new(10, 21, 31),
                new(11, 20, 31),
                new(11, 20, 30),
                new(10, 21, 30),
            ]);

            var normalN = quadN.GetNormal(c_calculationMethod);
            var normalE = quadE.GetNormal(c_calculationMethod);
            var normalS = quadS.GetNormal(c_calculationMethod);
            var normalW = quadW.GetNormal(c_calculationMethod);

            AreVectorsEqual(new VECTOR( 0.0000000f, -0.707093f,  0.7070923f), normalN, c_minDelta, nameof(normalN));
            AreVectorsEqual(new VECTOR(-0.7070923f, -0.707093f,  0.0000000f), normalE, c_minDelta, nameof(normalE));
            AreVectorsEqual(new VECTOR( 0.0000000f, -0.707093f, -0.7070923f), normalS, c_minDelta, nameof(normalS));
            AreVectorsEqual(new VECTOR( 0.7070923f, -0.707093f,  0.0000000f), normalW, c_minDelta, nameof(normalW));
        }
    }
}
