using System;
using System.Runtime.InteropServices;

namespace CommonLib {
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

        public void Resize(int size)
            => ResizeAt(0, Bytes.Length, size);

        public int ExpandOrContractAt(int offset, int bytesToAddOrRemove) {
            if (bytesToAddOrRemove == 0)
                return Bytes.Length;
            if (offset < 0 || offset > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

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

                // Copy data after the freshly-added padded 0's.
                var destPos = offset + bytesToAdd;
                var srcPos = offset;
                var copySize = oldBytes.Length - offset;
                unsafe {
                    fixed (byte* dest = Bytes, src = oldBytes)
                        _ = memcpy((IntPtr) dest + destPos, (IntPtr) src + srcPos, copySize);
                }
            }
            else {
                var bytesToRemove = -bytesToAddOrRemove;

                // Copy data after the removed data.
                var destPos = offset;
                var srcPos = offset + bytesToRemove;
                var copySize = oldBytes.Length - srcPos;
                unsafe {
                    fixed (byte* dest = Bytes, src = oldBytes)
                        _ = memcpy((IntPtr) dest + destPos, (IntPtr) src + srcPos, copySize);
                }
            }

            return Bytes.Length;
        }

        public void ResizeAt(int offset, int currentSize, int newSize) {
            if (offset < 0 || offset + currentSize > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (newSize < 0)
                throw new ArgumentOutOfRangeException(nameof(newSize));

            var sizeDiff = newSize - currentSize;
            if (sizeDiff >= 0)
                ExpandOrContractAt(offset + currentSize, sizeDiff);
            else
                ExpandOrContractAt(offset + currentSize + sizeDiff, sizeDiff);
        }

        public byte[] GetDataCopy() => (byte[]) Bytes.Clone();

        public byte[] GetDataCopyAt(int offset, int length) {
            if (offset < 0 || offset >= Bytes.Length)
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

        public void SetDataTo(byte[] data) => Bytes = (byte[]) data.Clone();

        public void SetDataAtTo(int offset, byte[] data) {
            if (offset < 0 || offset >= Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + data.Length > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(data));

            unsafe {
                fixed (byte* dest = Bytes, src = data)
                    _ = memcpy((IntPtr) dest + offset, (IntPtr) src, data.Length);
            }

        }

        public int Length => Bytes.Length;

        public byte this[int index] {
            get => Bytes[index];
            set => Bytes[index] = value;
        }

        private byte[] Bytes { get; set; }
    }
}
