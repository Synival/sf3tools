using System;
using CommonLib.Exceptions;

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

            parentArray.PreRangeModified += OnParentPreRangeModified;
            parentArray.RangeModified    += OnParentRangeModified;
        }

        /// <summary>
        /// When incremented, this indicates that a resize has been performed from the ByteArraySegment.
        /// This affects the logic when performing a resize at the beginning or end of a segment:
        /// When inside, the resize affects the ByteArraySegment. When not inside (i.e, the resize was
        /// performed directly from the parent), the ByteArraySegment is unaffected.
        /// </summary>
        private int _resizingInside = 0;

        public void OnParentPreRangeModified(object sender, ByteArrayRangeModifiedArgs args)
            => OnParentRangeModifiedReal(sender, args, isPre: true);

        public void OnParentRangeModified(object sender, ByteArrayRangeModifiedArgs args)
            => OnParentRangeModifiedReal(sender, args, isPre: false);

        public void OnParentRangeModifiedReal(object sender, ByteArrayRangeModifiedArgs args, bool isPre) {
            // TODO: moving!
            if (args.Moved)
                throw new NotImplementedException();

            var end       = Length;
            var argOffset = args.Offset - Offset;
            var argLength = args.Length;
            var argEnd    = argOffset + argLength;

            int offsetChange = 0;
            int lengthChange = 0;
            bool wasModified = false;

            int? modifiedStart = null;
            int? modifiedEnd = null;

            void UpdateModifiedRange(int start, int end2) {
                modifiedStart = Math.Min(modifiedStart ?? start, start);
                modifiedEnd   = Math.Max(modifiedEnd   ?? end2,  end2);
            }

            void HandleResize() {
                // If the change was inside the ByteArraySegment but for some reason beyond
                // the applicable range, throw an exception. This should never happen!!
                if (_resizingInside != 0 && (argOffset < 0 || argEnd > end))
                    throw new InvalidByteArraySegmentRangeException();

                // Ignore changes after the ByteArraySegment.
                if (argOffset > end || (argOffset == end && _resizingInside == 0))
                    return;

                // If the change was before the ByteArraySegment start, just adjust the offset.
                if (argEnd < 0 || (argEnd == 0 && _resizingInside == 0)) {
                    UpdateModifiedRange(0, Length);
                    offsetChange = args.LengthChange;
                    return;
                }

                // Throw an exception if the resize is both inside and outside the ByteArraySegment.
                // This is invalid because the new start/end of the segment can't be determined.
                if ((argOffset < 0 && argEnd > 0) || (argOffset < end && argEnd > end))
                    throw new InvalidByteArraySegmentRangeException();

                // If the resized range was inside, modify the ByteArraySegment length.
                if (argOffset >= 0 && argEnd <= end) {
                    UpdateModifiedRange(argOffset, argEnd);
                    lengthChange = args.LengthChange;
                    return;
                }

                throw new InvalidOperationException("State not handled");
            };

            if (args.Resized)
                HandleResize();

            void HandleModified() {
                var overlapStart = Math.Max(0,   argOffset);
                var overlapEnd   = Math.Min(end, argEnd);
                if (overlapStart < overlapEnd) {
                    UpdateModifiedRange(overlapStart, overlapEnd);
                    wasModified = true;
                }
            };

            if (args.Modified)
                HandleModified();

            if (modifiedStart.HasValue && modifiedEnd.HasValue) {
                var modifiedLength = modifiedEnd.Value - modifiedStart.Value;

                if (isPre)
                    PreRangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(modifiedStart.Value, modifiedLength, offsetChange, lengthChange, wasModified));
                else {
                    Offset += offsetChange;
                    Length += lengthChange;
                    RangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(modifiedStart.Value, modifiedLength, offsetChange, lengthChange, wasModified));
                }
            }
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
        public void SetDataAtTo(int offset, int length, byte[] data) => throw new NotImplementedException();
        public void SetDataTo(byte[] data) => throw new NotImplementedException();

        public void Dispose() {
            ParentArray.PreRangeModified -= OnParentPreRangeModified;
            ParentArray.RangeModified    -= OnParentRangeModified;
        }

        public event ByteArrayRangeModifiedHandler PreRangeModified;
        public event ByteArrayRangeModifiedHandler RangeModified;
    }
}
