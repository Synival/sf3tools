using CommonLib.Arrays;

namespace CommonLib.Tests.Arrays {
    [TestClass]
    public class ByteArraySegmentTests {
        [TestMethod]
        public void IndexerSet_ModifiesParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            arraySegment[2] = 100;
            Assert.AreEqual(100, arraySegment[2]);
            Assert.AreEqual(100, parentArray[5]);
        }

        [TestMethod]
        public void IndexerSet_TriggersModifiedEvent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            int modifiedCount = 0;
            ByteArrayRangeModifiedArgs? args = null;
            arraySegment.RangeModified += (s, a) => {
                modifiedCount++;
                args = a;
            };

            arraySegment[2] = 100;

            Assert.AreEqual(1, modifiedCount);
            Assert.IsNotNull(args);

            Assert.AreEqual(2, args.Offset);
            Assert.AreEqual(1, args.Length);
            Assert.AreEqual(0, args.LengthChange);
            Assert.AreEqual(0, args.OffsetChange);
            Assert.IsFalse(args.Moved);
            Assert.IsFalse(args.Resized);
            Assert.IsTrue(args.Modified);
        }

        [TestMethod]
        public void IndexerSet_OutOfRange_ThrowsException() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => arraySegment[-1] = 0);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => arraySegment[4] = 0);
        }

        [TestMethod]
        public void IndexerGet_GetsParentData() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            parentArray[5] = 100;
            Assert.AreEqual(100, arraySegment[2]);
            Assert.AreEqual(100, parentArray[5]);
        }

        [TestMethod]
        public void IndexerGet_OutOfRange_ThrowsException() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => arraySegment[-1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => arraySegment[4]);
        }

        [TestMethod]
        public void ParentIndexerSet_WithinAndOutOfRange_TriggersExpectedModifiedEvents() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            for (int i = 0; i < parentArray.Length; i++)
                parentArray[i] = (byte) (100 + i);

            Assert.AreEqual(4, args.Count);

            for (int i = 0; i < args.Count; i++) {
                var a = args[i];
                Assert.AreEqual(i, a.Offset);
                Assert.AreEqual(1, a.Length);
                Assert.AreEqual(0, a.LengthChange);
                Assert.AreEqual(0, a.OffsetChange);
                Assert.IsFalse(a.Moved);
                Assert.IsFalse(a.Resized);
                Assert.IsTrue(a.Modified);
            }
        }

        [TestMethod]
        public void Resize_Contract_UpdatesLengthAndContractsParent() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Resize_Expand_UpdatesLengthAndExpandsParent() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_BeforeBeginning_MovesSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_BeforeBeginning_MovesSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_AtBeginning_MovesSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_AtBeginning_MovesSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_AtEnd_DoesNothingToSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_AtEnd_DoesNothingToSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_AfterEnd_DoesNothingToSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_AfterEnd_DoesNothingToSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_Inside_ResizesSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_Inside_ResizesSegment() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_SpanningBeginning_ThrowsExceptionAndAbortsResize() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_SpanningBeginning_ThrowsExceptionAndAbortsResize() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentContracted_SpanningEnd_ThrowsExceptionAndAbortsResize() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParentExpanded_SpanningEnd_ThrowsExceptionAndAbortsResize() {
            throw new NotImplementedException();
        }
    }
}
