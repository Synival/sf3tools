using CommonLib.Arrays;

namespace CommonLib.Tests.Arrays {
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
            for (var i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void Indexer_AfterSettingData_HasExpectedBytes() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;
            for (var i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void Indexer_ChangesData_TriggersModified() {
            var byteArray = new ByteArray(10);
            var modifiedCount = 0;

            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };

            byteArray[5] = 1;
            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(5, args.Offset);
            Assert.AreEqual(1, args.Length);
            Assert.AreEqual(0, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsFalse(args.Resized);
            Assert.IsTrue(args.Modified);
        }

        [TestMethod]
        public void Indexer_DoesntChangeData_DoesntTriggerModified() {
            var byteArray = new ByteArray(10);
            var modifiedCount = 0;
            byteArray.RangeModified += (s, a) => { modifiedCount++; };

            byteArray[0] = 0;
            Assert.AreEqual(0, modifiedCount);
        }

        [TestMethod]
        public void Resize_Shrink_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.Resize(5);
            Assert.AreEqual(5, byteArray.Length);

            for (var i = 0; i < 5; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void Resize_Grow_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.Resize(15);
            Assert.AreEqual(15, byteArray.Length);

            for (var i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
            for (var i = 10; i < 15; i++)
                Assert.AreEqual(0, byteArray[i]);
        }

        [TestMethod]
        public void ResizeAt_Contract_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ResizeAt(3, 5, 1);
            Assert.AreEqual(6, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 3, 8, 9};
            for (var i = 0; i < 6; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ResizeAt_Contract_TriggersResized() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            var modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.ResizeAt(3, 5, 1);

            Assert.IsNotNull(args);
            Assert.AreEqual(1, modifiedCount);

            Assert.AreEqual(3, args.Offset);
            Assert.AreEqual(5, args.Length);
            Assert.AreEqual(-4, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsFalse(args.Modified);
        }

        [TestMethod]
        public void ResizeAt_Expand_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ResizeAt(3, 5, 10);
            Assert.AreEqual(15, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 8, 9};
            for (var i = 0; i < 15; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ResizeAt_Expand_TriggersResized() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            var modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.ResizeAt(3, 5, 10);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(3, args.Offset);
            Assert.AreEqual(5, args.Length);
            Assert.AreEqual(5, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsFalse(args.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_Contract_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ExpandOrContractAt(3, -4);
            Assert.AreEqual(6, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 7, 8, 9};
            for (var i = 0; i < 6; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ExpandOrContractAt_Contract_TriggersModified() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            var modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.ExpandOrContractAt(3, -4);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(3, args.Offset);
            Assert.AreEqual(0, args.Length);
            Assert.AreEqual(-4, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsFalse(args.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_Expand_HasExpectedSizeAndData() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            byteArray.ExpandOrContractAt(3, 5);
            Assert.AreEqual(15, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 0, 0, 0, 0, 0, 3, 4, 5, 6, 7, 8, 9};
            for (var i = 0; i < 15; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void ExpandOrContractAt_Expand_TriggersModified() {
            var byteArray = new ByteArray(10);
            for (var i = 0; i < 10; i++)
                byteArray[i] = (byte) i;

            var modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.ExpandOrContractAt(3, 5);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(3, args.Offset);
            Assert.AreEqual(0, args.Length);
            Assert.AreEqual(5, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsFalse(args.Modified);
        }

        [TestMethod]
        public void GetDataCopy_ReturnsCloneOfData() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var dataCopy = byteArray.GetDataCopy();
            Assert.AreEqual(10, dataCopy.Length);
            for (var i = 0; i < 10; i++)
                Assert.AreEqual(i, dataCopy[i]);
        }

        [TestMethod]
        public void GetDataCopyAt_ReturnsCloneOfData() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var dataCopy = byteArray.GetDataCopyAt(3, 5);
            Assert.AreEqual(5, dataCopy.Length);
            for (var i = 0; i < 5; i++)
                Assert.AreEqual(i + 3, dataCopy[i]);
        }

        [TestMethod]
        public void SetDataTo_SetsData() {
            var byteArray = new ByteArray(0);
            byteArray.SetDataTo([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            Assert.AreEqual(10, byteArray.Length);
            for (var i = 0; i < 10; i++)
                Assert.AreEqual(i, byteArray[i]);
        }

        [TestMethod]
        public void SetDataTo_WithSizeChanges_TriggersResizedAndModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataTo([0, 1, 2, 3, 4, 5, 6]);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(0, args.Offset);
            Assert.AreEqual(6, args.Length);
            Assert.AreEqual(1, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsTrue(args.Modified);
        }

        [TestMethod]
        public void SetDataTo_WithOnlyModifications_TriggersOnlyModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataTo([0, 1, 2, 3, 4, 50]);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(0, args.Offset);
            Assert.AreEqual(6, args.Length);
            Assert.AreEqual(0, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsFalse(args.Resized);
            Assert.IsTrue(args.Modified);
        }

        [TestMethod]
        public void SetDataTo_WithNoChanges_TriggersNothing() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataTo([0, 1, 2, 3, 4, 5]);

            Assert.AreEqual(0, modifiedCount);
        }

        [TestMethod]
        public void SetDataAtTo_WithChanges_ModifiesData() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            byteArray.SetDataAtTo(4, 3, [50, 100, 150]);
            Assert.AreEqual(10, byteArray.Length);

            var expectedData = new byte[]{0, 1, 2, 3, 50, 100, 150, 7, 8, 9};
            for (var i = 0; i < 10; i++)
                Assert.AreEqual(expectedData[i], byteArray[i]);
        }

        [TestMethod]
        public void SetDataAtTo_WithChanges_TriggersModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataAtTo(4, 3, [50, 100, 150]);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(4, args.Offset);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual(0, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsFalse(args.Resized);
            Assert.IsTrue(args.Modified);
        }

        [TestMethod]
        public void SetDataAtTo_WithoutChanges_DoesntTriggerModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataAtTo(4, 3, [4, 5, 6]);

            Assert.AreEqual(0, modifiedCount);
        }

        [TestMethod]
        public void SetDataAtTo_WithNoChangesAndExpanded_TriggersModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataAtTo(4, 3, [4, 5, 6, 0, 0]);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(4, args.Offset);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual(2, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsFalse(args.Modified);
        }

        [TestMethod]
        public void SetDataAtTo_WithNoChangesAndContracted_TriggersModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataAtTo(4, 3, [4, 5]);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(4, args.Offset);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual(-1, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsFalse(args.Modified);
        }

        [TestMethod]
        public void SetDataAtTo_WithChangesAndResize_TriggersModified() {
            var byteArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            byteArray.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };
            byteArray.SetDataAtTo(4, 3, [40, 50, 60, 70]);

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(4, args.Offset);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual(1, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsTrue(args.Resized);
            Assert.IsTrue(args.Modified);
        }
    }
}
