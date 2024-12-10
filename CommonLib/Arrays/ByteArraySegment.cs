using System;

namespace CommonLib.Arrays {
    public class ByteArraySegment : IByteArray {
        public ByteArraySegment(IByteArray parentArray, int offset, int length) {
            if (parentArray == null)
                throw new ArgumentNullException(nameof(parentArray));
            if (offset < 0 || offset > parentArray.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0 || offset + length > parentArray.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            ParentArray = parentArray;
            Offset = offset;
            Length = length;
        }

        private IByteArray ParentArray { get; }

        public int Offset { get; private set; }

        public int Length { get; private set; }

        public byte this[int index] {
            get => (index < 0 || index >= Length) ? throw new ArgumentOutOfRangeException(nameof(index)) : ParentArray[Offset + index];
            set {
                if (index < 0 || index >= Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                ParentArray[Offset + index] = value;
            }
        }

        public int ExpandOrContractAt(int offset, int bytesToAddOrRemove) => throw new NotImplementedException();
        public byte[] GetDataCopy() => throw new NotImplementedException();
        public byte[] GetDataCopyAt(int offset, int length) => throw new NotImplementedException();
        public void Resize(int size) => throw new NotImplementedException();
        public void ResizeAt(int offset, int currentSize, int newSize) => throw new NotImplementedException();
        public void SetDataAtTo(int offset, byte[] data) => throw new NotImplementedException();
        public void SetDataTo(byte[] data) => throw new NotImplementedException();

        public event ByteArrayModifiedHandler Modified;
        public event ByteArrayResizedHandler Resized;
    }
}
