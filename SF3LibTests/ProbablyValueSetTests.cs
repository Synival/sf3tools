using SF3;

namespace SF3LibTests
{
    [TestClass]
    public class ProbablyValueSetTests
    {
        [TestMethod]
        public void RollNext_WithCoinFlip_ReturnsExpectedSet()
        {
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
        public void RollNext_WithWeightedCoinFlip_ReturnsExpectedSet()
        {
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
        public void GetWeightedAverage_WithWeightedAverage_ReturnsExpectedValue()
        {
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
    }
}