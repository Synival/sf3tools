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
        public void Indexer_ChangesData_TriggersModified() {
            var byteArray = new ByteArray(10);
            int modifiedCount = 0;
            byteArray.Modified += (s, a) => { modifiedCount++; };

            byteArray[0] = 1;
            Assert.AreEqual(1, modifiedCount);
        }

        [TestMethod]
        public void Indexer_DoesntChangeData_DoesntTriggerModified() {
            var byteArray = new ByteArray(10);
            int modifiedCount = 0;
            byteArray.Modified += (s, a) => { modifiedCount++; };

            byteArray[0] = 0;
            Assert.AreEqual(0, modifiedCount);
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
        public void ResizeAt_Contract_TriggersResized() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            int resizedCount = 0;
            ByteArrayResizedArgs? args = null;
            byteArray.Resized += (s, a) => {
                resizedCount++;
                args = a;
            };
            byteArray.ResizeAt(3, 5, 1);

            Assert.AreEqual(1, resizedCount);
            Assert.AreEqual(4, args?.Offset);
            Assert.AreEqual(-4, args?.BytesAddedOrRemoved);
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
        public void ResizeAt_Expand_TriggersResized() {
            var byteArray = new ByteArray(10);
            for (int i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            int resizedCount = 0;
            ByteArrayResizedArgs? args = null;
            byteArray.Resized += (s, a) => {
                resizedCount++;
                args = a;
            };
            byteArray.ResizeAt(3, 5, 10);

            Assert.AreEqual(1, resizedCount);
            Assert.AreEqual(8, args?.Offset);
            Assert.AreEqual(5, args?.BytesAddedOrRemoved);
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
        public void SetDataTo_WithSizeChanges_TriggersResizedAndModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5]);
            int resizedCount = 0, modifiedCount = 0;
            byteArray.Resized += (s, a) => { resizedCount++; };
            byteArray.Modified += (s, a) => { modifiedCount++; };
            byteArray.SetDataTo([0, 1, 2, 3, 4, 5, 6]);

            Assert.AreEqual(1, resizedCount);
            Assert.AreEqual(1, modifiedCount);
        }

        [TestMethod]
        public void SetDataTo_WithOnlyModifications_TriggersOnlyModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5]);
            int resizedCount = 0, modifiedCount = 0;
            byteArray.Resized += (s, a) => { resizedCount++; };
            byteArray.Modified += (s, a) => { modifiedCount++; };
            byteArray.SetDataTo([0, 1, 2, 3, 4, 50]);

            Assert.AreEqual(0, resizedCount);
            Assert.AreEqual(1, modifiedCount);
        }

        [TestMethod]
        public void SetDataTo_WithNoChanges_TriggersNothing() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5]);
            int resizedCount = 0, modifiedCount = 0;
            byteArray.Resized += (s, a) => { resizedCount++; };
            byteArray.Modified += (s, a) => { modifiedCount++; };
            byteArray.SetDataTo([0, 1, 2, 3, 4, 5]);

            Assert.AreEqual(0, resizedCount);
            Assert.AreEqual(0, modifiedCount);
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

        [TestMethod]
        public void SetDataAtTo_WithChanges_TriggersModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            int modifiedCount = 0;
            byteArray.Modified += (s, a) => { modifiedCount++; };
            byteArray.SetDataAtTo(4, [50, 100, 150]);

            Assert.AreEqual(1, modifiedCount);
        }

        [TestMethod]
        public void SetDataAtTo_WithoutChanges_DoesntTriggerModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            int modifiedCount = 0;
            byteArray.Modified += (s, a) => { modifiedCount++; };
            byteArray.SetDataAtTo(4, [4, 5, 6]);

            Assert.AreEqual(0, modifiedCount);
        }
    }
}
