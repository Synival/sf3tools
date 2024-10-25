using SF3;

namespace SF3LibTests {
    [TestClass]
    public class ProbablyValueSetTests {
        [TestMethod]
        public void RollNext_WithCoinFlip_ReturnsExpectedSet() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 1, 0.5 },
                { 2, 0.5 }
            };

            // Act
            var outputSet = inputSet.RollNext((int value) =>
            {
                return new ProbableValueSet()
                {
                    { value, 0.5 },
                    { value + 1, 0.5 }
                };
            });

            // Assert
            Assert.AreEqual(3, outputSet.Count);
            Assert.AreEqual(0.25, outputSet[1]);
            Assert.AreEqual(0.50, outputSet[2]);
            Assert.AreEqual(0.25, outputSet[3]);
        }

        [TestMethod]
        public void RollNext_WithWeightedCoinFlip_ReturnsExpectedSet() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 1, 0.5 },
                { 2, 0.5 }
            };

            // Act
            var outputSet = inputSet.RollNext((int value) =>
            {
                return new ProbableValueSet()
                {
                    { value, 0.25 },
                    { value + 1, 0.75 }
                };
            });

            // Assert
            Assert.AreEqual(3, outputSet.Count);
            Assert.AreEqual(0.125, outputSet[1], 0.001);
            Assert.AreEqual(0.50, outputSet[2], 0.001);
            Assert.AreEqual(0.375, outputSet[3], 0.001);
        }

        [TestMethod]
        public void GetWeightedAverage_WithWeightedAverage_ReturnsExpectedValue() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 1, 0.25 },
                { 2, 0.25 },
                { 3, 0.50 }
            };

            // Act + Assert
            Assert.AreEqual(2.25, inputSet.GetWeightedAverage(), 0.001);
        }

        [TestMethod]
        public void GetWeightedMedian_WithCoinFlip_ReturnsBetween() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 10, 0.50 },
                { 20, 0.50 }
            };

            // Act + Assert
            Assert.AreEqual(15.0, inputSet.GetWeightedMedian(), 0.001);
        }

        [TestMethod]
        public void GetWeightedMedianAt_WithCoinFlipAndZeroPercent_ReturnsTails() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 10, 0.50 },
                { 20, 0.50 }
            };

            // Act + Assert
            Assert.AreEqual(10.0, inputSet.GetWeightedMedianAt(0.00), 0.001);
        }

        [TestMethod]
        public void GetWeightedMedianAt_WithCoinFlipAndFiftyPercent_ReturnsBetweens() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 10, 0.50 },
                { 20, 0.50 }
            };

            // Act + Assert
            Assert.AreEqual(15.0, inputSet.GetWeightedMedianAt(0.50), 0.001);
        }

        [TestMethod]
        public void GetWeightedMedianAt_WithCoinFlipAndHundredPercent_ReturnsHeads() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 10, 0.50 },
                { 20, 0.50 }
            };

            // Act + Assert
            Assert.AreEqual(20.0, inputSet.GetWeightedMedianAt(1.00), 0.001);
        }


        [TestMethod]
        public void GetWeightedMedianAt_WithThreeSidesFiftyPercent_ReturnsMiddle() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 10, 0.33333 },
                { 20, 0.33333 },
                { 30, 0.33334 }
            };

            // Act + Assert
            Assert.AreEqual(20.0, inputSet.GetWeightedMedianAt(0.50), 0.001);
        }


        [TestMethod]
        public void GetWeightedMedianAt_WithOneOneTwoFiftyPercent_ReturnsBetweenSecondAndThird() {
            // Arrange
            var inputSet = new ProbableValueSet()
            {
                { 10, 0.25 },
                { 20, 0.25 },
                { 30, 0.50 }
            };

            // Act + Assert
            Assert.AreEqual(23.333, inputSet.GetWeightedMedianAt(0.50), 0.001);
        }
    }
}