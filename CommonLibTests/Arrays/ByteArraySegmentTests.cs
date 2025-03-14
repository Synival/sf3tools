﻿using CommonLib.Arrays;
using CommonLib.Exceptions;

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
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 100, 6], arraySegment.GetDataCopy()));
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
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.Resize(2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(2, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4], arraySegment.GetDataCopy()));
            Assert.AreEqual(8, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(-2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void Resize_Expand_UpdatesLengthAndExpandsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.Resize(6);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6, 0, 0], arraySegment.GetDataCopy()));
            Assert.AreEqual(12, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ResizeAt_Contract_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ResizeAt(1, 2, 0);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(2, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 6], arraySegment.GetDataCopy()));
            Assert.AreEqual(8, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(1, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(-2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ResizeAt_Expand_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ResizeAt(1, 2, 4);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 0, 0, 6], arraySegment.GetDataCopy()));
            Assert.AreEqual(12, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(1, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_AtBeginning_Contract_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ExpandOrContractAt(0, -2);
            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(2, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([5, 6], arraySegment.GetDataCopy()));
            Assert.AreEqual(8, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(-2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_AtBeginning_Expand_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ExpandOrContractAt(0, 2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([0, 0, 3, 4, 5, 6], arraySegment.GetDataCopy()));
            Assert.AreEqual(12, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(0, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_AtEnd_Contract_ThrowsException() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => arraySegment.ExpandOrContractAt(4, -2));
        }

        [TestMethod]
        public void ExpandOrContractAt_AtEnd_Expand_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ExpandOrContractAt(4, 2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6, 0, 0], arraySegment.GetDataCopy()));
            Assert.AreEqual(12, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(4, a.Offset);
            Assert.AreEqual(0, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_Inside_Contract_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ExpandOrContractAt(2, -2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(2, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4], arraySegment.GetDataCopy()));
            Assert.AreEqual(8, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(2, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(-2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ExpandOrContractAt_Inside_Expand_UpdatesLengthAndContractsParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.ExpandOrContractAt(2, 2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 0, 0, 5, 6], arraySegment.GetDataCopy()));
            Assert.AreEqual(12, parentArray.Length);

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(2, a.Offset);
            Assert.AreEqual(0, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_BeforeBeginning_Contracted_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(0, 2, 0);

            Assert.AreEqual(1, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(-2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_BeforeBeginning_Expanded_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(0, 2, 4);

            Assert.AreEqual(5, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResized_AtBeginning_WithResizeAt_Contracted_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(0, 3, 1);

            Assert.AreEqual(1, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(-2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResized_AtBeginning_WithExpandOrContractAt_Contracted_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ExpandOrContractAt(1, -2);

            Assert.AreEqual(1, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(-2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_AtBeginning_WithResizeAt_Expanded_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(0, 3, 5);

            Assert.AreEqual(5, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_AtBeginning_WithExpandOrContractAt_Expanded_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ExpandOrContractAt(3, 2);

            Assert.AreEqual(5, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_AtBeginning_WithResizeAt_ExpandedFromZeroSize_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(3, 0, 2);

            Assert.AreEqual(5, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_AtParentBeginning_WithResizeAt_ExpandedFromZeroSize_MovesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(0, 0, 2);

            Assert.AreEqual(5, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(arraySegment.Length, a.Length);
            Assert.AreEqual(2, a.OffsetChange);
            Assert.AreEqual(0, a.LengthChange);
            Assert.AreEqual(true, a.Moved);
            Assert.AreEqual(false, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_AtEnd_Contracted_WithResizeAt_DoesNothingToSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(7, 3, 1);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(0, args.Count);
        }

        [TestMethod]
        public void ParentResize_AtEnd_Contracted_WithExpandOrContractAt_DoesNothingToSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ExpandOrContractAt(7, -2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(0, args.Count);
        }

        [TestMethod]
        public void ParentResize_AtEnd_Expanded_WithResizeAt_DoesNothingToSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(7, 3, 5);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(0, args.Count);
        }

        [TestMethod]
        public void ParentResize_AtEnd_Expanded_WithExpandOrContractAt_DoesNothingToSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ExpandOrContractAt(7, 2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(0, args.Count);
        }

        [TestMethod]
        public void ParentResize_AfterEnd_Contracted_DoesNothingToSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(8, 2, 1);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(0, args.Count);
        }

        [TestMethod]
        public void ParentResize_AfterEnd_Expanded_DoesNothingToSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(8, 2, 5);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(4, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(0, args.Count);
        }

        [TestMethod]
        public void ParentResize_FullyInside_Contracted_ResizesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(3, 4, 2);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(2, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(-2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_Inside_Contracted_ResizesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(4, 2, 0);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(2, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(1, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(-2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_FullyInside_Expanded_ResizesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(3, 4, 6);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 6, 0, 0], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_Inside_Expanded_ResizesSegment() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            parentArray.ResizeAt(4, 2, 4);

            Assert.AreEqual(3, arraySegment.Offset);
            Assert.AreEqual(6, arraySegment.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([3, 4, 5, 0, 0, 6], arraySegment.GetDataCopy()));

            Assert.AreEqual(1, args.Count);
            var a = args[0];
            Assert.AreEqual(1, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(false, a.Moved);
            Assert.AreEqual(true, a.Resized);
            Assert.AreEqual(false, a.Modified);
        }

        [TestMethod]
        public void ParentResize_SpanningBeginning_Contracted_ThrowsExceptionAndAbortsResize() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var oldLength = parentArray.Length;

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(0, 4, 2));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(2, 4, 2));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(2, 8, 6));
            Assert.AreEqual(oldLength, parentArray.Length);
        }

        [TestMethod]
        public void ParentResize_SpanningBeginning_Expanded_ThrowsExceptionAndAbortsResize() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var oldLength = parentArray.Length;

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(0, 4, 6));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(2, 4, 6));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(2, 8, 10));
            Assert.AreEqual(oldLength, parentArray.Length);
        }

        [TestMethod]
        public void ParentResize_SpanningEnd_Contracted_ThrowsExceptionAndAbortsResize() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var oldLength = parentArray.Length;

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(4, 4, 2));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(6, 4, 2));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(4, 6, 2));
            Assert.AreEqual(oldLength, parentArray.Length);
        }

        [TestMethod]
        public void ParentResize_SpanningEnd_Expanded_ThrowsExceptionAndAbortsResize() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var oldLength = parentArray.Length;

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(4, 4, 6));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(6, 4, 6));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(4, 6, 10));
            Assert.AreEqual(oldLength, parentArray.Length);
        }

        [TestMethod]
        public void ParentResize_SpanningBeginningAndEnd_Contracted_ThrowsExceptionAndAbortsResize() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var oldLength = parentArray.Length;

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.Resize(8));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(0, 10, 8));
            Assert.AreEqual(oldLength, parentArray.Length);
        }

        [TestMethod]
        public void ParentResize_SpanningBeginningAndEnd_Expanded_ThrowsExceptionAndAbortsResize() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var oldLength = parentArray.Length;

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.Resize(12));
            Assert.AreEqual(oldLength, parentArray.Length);

            Assert.ThrowsException<InvalidByteArraySegmentRangeException>(() => parentArray.ResizeAt(0, 10, 12));
            Assert.AreEqual(oldLength, parentArray.Length);
        }

        [TestMethod]
        public void GetDataCopy_ReturnsExpectedData() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var dataCopy = arraySegment.GetDataCopy();

            Assert.AreEqual(4, dataCopy.Length);
            for (var i = 0; i < 4; i++)
                Assert.AreEqual(i + 3, dataCopy[i]);
        }

        [TestMethod]
        public void GetDataCopyAt_ReturnsExpectedData() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var dataCopy = arraySegment.GetDataCopyAt(1, 2);

            Assert.AreEqual(2, dataCopy.Length);
            for (var i = 0; i < 2; i++)
                Assert.AreEqual(i + 4, dataCopy[i]);
        }

        [TestMethod]
        public void SetDataTo_UpdatesDataAndResizesParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            arraySegment.SetDataTo([10, 20, 30, 40, 50, 60]);

            Assert.AreEqual(6, arraySegment.Length);
            Assert.AreEqual(12, parentArray.Length);

            for (var i = 0; i < arraySegment.Length; i++)
                Assert.AreEqual((i + 1) * 10, arraySegment[i]);
        }

        [TestMethod]
        public void SetDataTo_TriggersModified() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.SetDataTo([10, 20, 30, 40, 50, 60]);

            var a = args[0];
            Assert.AreEqual(0, a.Offset);
            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(2, a.LengthChange);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.IsFalse(a.Moved);
            Assert.IsTrue(a.Resized);
            Assert.IsTrue(a.Modified);
        }

        [TestMethod]
        public void SetDataAtTo_UpdatesDataAndResizesParent() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            arraySegment.SetDataAtTo(1, 2, [10, 20, 30, 40, 50, 60]);

            Assert.AreEqual(8, arraySegment.Length);
            Assert.AreEqual(14, parentArray.Length);

            Assert.AreEqual( 3, arraySegment[0]);
            Assert.AreEqual(10, arraySegment[1]);
            Assert.AreEqual(20, arraySegment[2]);
            Assert.AreEqual(30, arraySegment[3]);
            Assert.AreEqual(40, arraySegment[4]);
            Assert.AreEqual(50, arraySegment[5]);
            Assert.AreEqual(60, arraySegment[6]);
            Assert.AreEqual( 6, arraySegment[7]);
        }

        [TestMethod]
        public void SetDataAtTo_TriggersModified() {
            var parentArray = new ByteArray([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            var arraySegment = new ByteArraySegment(parentArray, 3, 4);

            var args = new List<ByteArrayRangeModifiedArgs>();
            arraySegment.RangeModified += (s, a) => args.Add(a);

            arraySegment.SetDataAtTo(1, 2, [10, 20, 30, 40, 50, 60]);

            var a = args[0];
            Assert.AreEqual(1, a.Offset);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(4, a.LengthChange);
            Assert.AreEqual(0, a.OffsetChange);
            Assert.IsFalse(a.Moved);
            Assert.IsTrue(a.Resized);
            Assert.IsTrue(a.Modified);
        }
    }
}
