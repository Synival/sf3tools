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
                new(11, 20, 30),
                new(10, 20, 30),
                new(10, 20, 31),
                new(11, 20, 31),
            ]);

            var normal = quad.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR(0, -1, 0), normal);
        }

        [TestMethod]
        public void GetNormal_With025Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(11, 20.25f, 30),
                new(10, 20.25f, 30),
                new(10, 20.00f, 31),
                new(11, 20.00f, 31),
            ]);

            var normal = quad.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.97013855f, -0.2425232f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_With050Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(11, 20.5f, 30),
                new(10, 20.5f, 30),
                new(10, 20.0f, 31),
                new(11, 20.0f, 31),
            ]);

            var normal = quad.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.89442444f, -0.4472046f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_With100Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(11, 21, 30),
                new(10, 21, 30),
                new(10, 20, 31),
                new(11, 20, 31),
            ]);

            var normal = quad.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR(0, -0.70710678118f, -0.70710678118f), normal, 0.0001f);
        }

        [TestMethod]
        public void GetNormal_With150Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(11, 21.5f, 30),
                new(10, 21.5f, 30),
                new(10, 20.0f, 31),
                new(11, 20.0f, 31),
            ]);

            var normal = quad.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.5546875f, -0.8320465f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_With200Slope_ReturnsExpectedNormal() {
            var quad = new POLYGON([
                new(11, 22.0f, 30),
                new(10, 22.0f, 30),
                new(10, 20.0f, 31),
                new(11, 20.0f, 31),
            ]);

            var normal = quad.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR(0.0000000f, -0.4472046f, -0.89442444f), normal, c_minDelta);
        }

        [TestMethod]
        public void GetNormal_CheckNESW_ReturnsExpectedNormal() {
            var quadN = new POLYGON([
                new(11, 21, 30),
                new(10, 21, 30),
                new(10, 20, 31),
                new(11, 20, 31),
            ]);
            var quadE = new POLYGON([
                new(11, 20, 30),
                new(10, 21, 30),
                new(10, 21, 31),
                new(11, 20, 31),
            ]);
            var quadS = new POLYGON([
                new(11, 20, 30),
                new(10, 20, 30),
                new(10, 21, 31),
                new(11, 21, 31),
            ]);
            var quadW = new POLYGON([
                new(11, 21, 30),
                new(10, 20, 30),
                new(10, 20, 31),
                new(11, 21, 31),
            ]);

            var normalN = quadN.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);
            var normalE = quadE.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);
            var normalS = quadS.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);
            var normalW = quadW.GetMeshNormalComponent(CornerType.TopRight, c_calculationMethod);

            AreVectorsEqual(new VECTOR( 0.0000000f, -0.707093f, -0.7070923f), normalN, c_minDelta, nameof(normalN));
            AreVectorsEqual(new VECTOR(-0.7070923f, -0.707093f,  0.0000000f), normalE, c_minDelta, nameof(normalE));
            AreVectorsEqual(new VECTOR( 0.0000000f, -0.707093f,  0.7070923f), normalS, c_minDelta, nameof(normalS));
            AreVectorsEqual(new VECTOR( 0.7070923f, -0.707093f,  0.0000000f), normalW, c_minDelta, nameof(normalW));
        }

        [TestMethod]
        public void GetCornerNormal_ForAllCorners_WithHighBR_ReturnsExpectedNormals() {
            var quad = new POLYGON([
                new(11, 15, 30),
                new(10, 20, 30),
                new(10, 20, 31),
                new(11, 20, 31),
            ]);

            var normalTR = quad.GetCornerNormal(CornerType.TopRight);
            var normalTL = quad.GetCornerNormal(CornerType.TopLeft);
            var normalBL = quad.GetCornerNormal(CornerType.BottomLeft);
            var normalBR = quad.GetCornerNormal(CornerType.BottomRight);

            AreVectorsEqual(new VECTOR( 0.0000000f, -0.1961059f,  0.9805755f), normalTR, c_minDelta, nameof(normalTR));
            AreVectorsEqual(new VECTOR( 0.0000000f, -1.0000000f,  0.0000000f), normalTL, c_minDelta, nameof(normalTL));
            AreVectorsEqual(new VECTOR(-0.9805755f, -0.1961059f,  0.0000000f), normalBL, c_minDelta, nameof(normalBL));
            AreVectorsEqual(new VECTOR(-0.7001343f, -0.1400146f,  0.7001343f), normalBR, c_minDelta, nameof(normalBR));
        }

        [TestMethod]
        public void GetCornerNormal_ForAllCorners_WithHighBL_ReturnsExpectedNormals() {
            var quad = new POLYGON([
                new(11, 20, 30),
                new(10, 15, 30),
                new(10, 20, 31),
                new(11, 20, 31),
            ]);

            var normalTR = quad.GetCornerNormal(CornerType.TopRight);
            var normalTL = quad.GetCornerNormal(CornerType.TopLeft);
            var normalBL = quad.GetCornerNormal(CornerType.BottomLeft);
            var normalBR = quad.GetCornerNormal(CornerType.BottomRight);

            AreVectorsEqual(new VECTOR( 0.0000000f, -1.0000000f,  0.0000000f), normalTR, c_minDelta, nameof(normalTR));
            AreVectorsEqual(new VECTOR( 0.0000000f, -0.1961059f,  0.9805755f), normalTL, c_minDelta, nameof(normalTL));
            AreVectorsEqual(new VECTOR( 0.7001343f, -0.1400146f,  0.7001343f), normalBL, c_minDelta, nameof(normalBL));
            AreVectorsEqual(new VECTOR( 0.9805755f, -0.1961059f,  0.0000000f), normalBR, c_minDelta, nameof(normalBR));
        }
    }
}
