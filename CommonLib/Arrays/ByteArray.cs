using System;
using System.Runtime.InteropServices;

namespace CommonLib.Arrays {
    /// <summary>
    /// Wrapper for a byte[] that can be resized. Use this if you need an unchanging reference to a byte[] that must be
    /// recreated due to size changes.
    /// </summary>
    public class ByteArray : IByteArray {
        /// <summary>
        /// Creates a new ByteArray with a new byte[] of size 'size'.
        /// </summary>
        /// <param name="size">The size of the new ByteArray.</param>
        public ByteArray(int size) {
            Bytes = new byte[size];
        }

        /// <summary>
        /// Creates a new ByteArray using an existing byte[]. The data is copied into a new byte array.
        /// </summary>
        /// <param name="bytes">The byte array to transfer into the new ByteArray.</param>
        public ByteArray(byte[] bytes) {
            Bytes = (byte[]) bytes.Clone();
        }

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(IntPtr lhs, IntPtr rhs, long count);

        private static bool ByteArraysAreEqual(byte[] lhs, byte[] rhs) {
            unsafe {
                fixed (byte* lhsPtr = lhs, rhsPtr = rhs)
                    return lhs.Length == rhs.Length && memcmp((IntPtr) lhsPtr, (IntPtr) rhsPtr, lhs.Length) == 0;
            }
        }

        public void Resize(int size)
            => ResizeReal(size, true);

        public bool ResizeReal(int size, bool invokeEvents)
            => ResizeAtReal(0, Bytes.Length, size, invokeEvents);

        public int ExpandOrContractAt(int offset, int bytesToAddOrRemove)
            => ExpandOrContractAtReal(offset, bytesToAddOrRemove, true);

        private int ExpandOrContractAtReal(int offset, int bytesToAddOrRemove, bool invokeEvents) {
            if (offset < 0 || offset > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (bytesToAddOrRemove == 0)
                return Bytes.Length;

            // Make sure we can't remove more bytes than are available.
            if (bytesToAddOrRemove < 0 && -bytesToAddOrRemove > Bytes.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(bytesToAddOrRemove));

            var oldBytes = Bytes;
            Bytes = new byte[oldBytes.Length + bytesToAddOrRemove];

            // Copy all old data before 'offset'.
            if (offset > 0) {
                unsafe {
                    fixed (byte* dest = Bytes, src = oldBytes)
                        _ = memcpy((IntPtr) dest, (IntPtr) src, offset);
                }
            }

            if (bytesToAddOrRemove > 0) {
                var bytesToAdd = bytesToAddOrRemove;
                if (invokeEvents)
                    PreRangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, 0, 0, bytesToAdd, false));

                // Copy data after the freshly-added padded 0's.
                var destPos = offset + bytesToAdd;
                var srcPos = offset;
                var copySize = oldBytes.Length - offset;
                unsafe {
                    fixed (byte* dest = Bytes, src = oldBytes)
                        _ = memcpy((IntPtr) dest + destPos, (IntPtr) src + srcPos, copySize);
                }
                if (invokeEvents)
                    RangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, 0, 0, bytesToAdd, false));
            }
            else {
                var bytesToRemove = -bytesToAddOrRemove;
                if (invokeEvents)
                    PreRangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, 0, 0, -bytesToRemove, false));

                // Copy data after the removed data.
                var destPos = offset;
                var srcPos = offset + bytesToRemove;
                var copySize = oldBytes.Length - srcPos;
                unsafe {
                    fixed (byte* dest = Bytes, src = oldBytes)
                        _ = memcpy((IntPtr) dest + destPos, (IntPtr) src + srcPos, copySize);
                }
                if (invokeEvents)
                    RangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, 0, 0, -bytesToRemove, false));
            }
            return Bytes.Length;
        }

        public void ResizeAt(int offset, int currentSize, int newSize)
            => ResizeAtReal(offset, currentSize, newSize, true);

        public bool ResizeAtReal(int offset, int currentSize, int newSize, bool invokeEvents) {
            if (offset < 0 || offset > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + currentSize > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(currentSize));
            if (newSize < 0)
                throw new ArgumentOutOfRangeException(nameof(newSize));
            if (currentSize == newSize)
                return false;

            var sizeDiff = newSize - currentSize;
            if (invokeEvents)
                PreRangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, currentSize, 0, sizeDiff, false));

            if (sizeDiff >= 0)
                _ = ExpandOrContractAtReal(offset + currentSize, sizeDiff, false);
            else
                _ = ExpandOrContractAtReal(offset + currentSize + sizeDiff, sizeDiff, false);

            if (invokeEvents)
                RangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, currentSize, 0, sizeDiff, false));

            return true;
        }

        public byte[] GetDataCopy() => (byte[]) Bytes.Clone();

        public byte[] GetDataCopyAt(int offset, int length) {
            if (offset < 0 || offset > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0 || offset + length > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            var bytes = new byte[length];
            unsafe {
                fixed (byte* dest = bytes, src = Bytes)
                    _ = memcpy((IntPtr) dest, (IntPtr) src + offset, length);
            }

            return bytes;
        }

        public void SetDataTo(byte[] data)
            => SetDataAtTo(0, Bytes.Length, data);

        public void SetDataAtTo(int offset, int length, byte[] data) {
            if (offset < 0 || offset > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0 || offset + length > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            bool needsResize = length != data.Length;
            bool needsModify = false;
            if (data.Length > 0) {
                unsafe {
                    fixed (byte* dest = Bytes, src = data) {
                        var destPtr = (IntPtr) dest + offset;
                        var srcPtr = (IntPtr) src;
                        needsModify = (memcmp(destPtr, srcPtr, data.Length) != 0);
                    }
                }
            }

            if (!needsResize && !needsModify)
                return;

            PreRangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, length, 0, data.Length - length, needsModify));

            _ = ResizeAtReal(offset, length, data.Length, false);
            if (data.Length > 0) {
                unsafe {
                    fixed (byte* dest = Bytes, src = data) {
                        var destPtr = (IntPtr) dest + offset;
                        var srcPtr = (IntPtr) src;
                        if (needsModify)
                            _ = memcpy(destPtr, srcPtr, data.Length);
                    }
                }
            }

            RangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(offset, length, 0, data.Length - length, needsModify));
        }

        public void Dispose() { }

        public int Length => Bytes.Length;

        public byte this[int index] {
            get => Bytes[index];
            set {
                if (Bytes[index] != value) {
                    PreRangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(index, 1, 0, 0, true));
                    Bytes[index] = value;
                    RangeModified?.Invoke(this, new ByteArrayRangeModifiedArgs(index, 1, 0, 0, true));
                }
            }
        }

        private byte[] Bytes { get; set; }

        public event ByteArrayRangeModifiedHandler PreRangeModified;
        public event ByteArrayRangeModifiedHandler RangeModified;
    }
}
