namespace CommonLib.Tests {
    [TestClass]
    public class ByteArrayTests {
        [TestMethod]
        public void Length_AfterConstruction_HasExpectedValue() {
            var byteArray = new ByteArray(100);
            Assert.AreEqual(100, byteArray.Length);
        }

        [TestMethod]
        public void Indexer_AfterConstruction_HasExpectedBytes() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            Assert.AreEqual(10, byteArray.Length);
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void Indexer_AfterSettingData_HasExpectedBytes() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void Resize_Shrink_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.Resize(5);
            Assert.AreEqual(5, byteArray.Length);

            for (int i = 0; i < 5; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void Resize_Grow_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.Resize(15);
            Assert.AreEqual(15, byteArray.Length);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
            for (int i = 10; i < 15; i++)
                Assert.AreEqual(0, byteArray[i]);
        }

        [TestMethod]
        public void ResizeAt_Contract_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ResizeAt(3, 5, 1);
            Assert.AreEqual(6, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 3, 8, 9};
            for (int i = 0; i < 6; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ResizeAt_Expand_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ResizeAt(3, 5, 10);
            Assert.AreEqual(15, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 8, 9};
            for (int i = 0; i < 15; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ExpandOrContractAt_Contract_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ExpandOrContractAt(3, -4);
            Assert.AreEqual(6, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 7, 8, 9};
            for (int i = 0; i < 6; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ExpandOrContractAt_Expand_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ExpandOrContractAt(3, 5);
            Assert.AreEqual(15, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 0, 0, 0, 0, 0, 3, 4, 5, 6, 7, 8, 9};
            for (int i = 0; i < 15; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void GetDataCopy_ReturnsCloneOfData() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var dataCopy = byteArray.GetDataCopy();
            Assert.AreEqual(10, dataCopy.Length);
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(i, dataCopy[i]);
        }

        [TestMethod]
        public void GetDataCopyAt_ReturnsCloneOfData() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var dataCopy = byteArray.GetDataCopyAt(3, 5);
            Assert.AreEqual(5, dataCopy.Length);
            for (int i = 0; i < 5; i++)
                Assert.AreEqual(i + 3, dataCopy[i]);
        }

        [TestMethod]
        public void SetDataTo_SetsData() {
            var byteArray = new ByteArray(0);
            byteArray.SetDataTo([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            Assert.AreEqual(10, byteArray.Length);
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void SetDataAtTo_SetsData() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            byteArray.SetDataAtTo(4, [50, 100, 150]);
            Assert.AreEqual(10, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 3, 50, 100, 150, 7, 8, 9};
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }
    }
}
