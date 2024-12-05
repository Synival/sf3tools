using System;
using System.Runtime.InteropServices;

namespace CommonLib {
    /// <summary>
    /// Wrapper for a byte[] that can be resized. Use this if you need an unchanging reference to a byte[] that must be
    /// recreated due to size changes.
    /// </summary>
    public class ByteArray {
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

        /// <summary>
        /// Resizes the byte[] array, adding 0's to the end for additional data or truncating data at the end.
        /// </summary>
        /// <param name="size">The new size of the byte[] array.</param>
        public void Resize(int size)
            => ResizeAt(0, Bytes.Length, size);

        /// <summary>
        /// Adds or removes bytes at a specific offset. Use this to expand or contract the byte[] array at specific points.
        /// Bytes are added or removed at the 'offset' position, not before.
        /// </summary>
        /// <param name="offset">The offset at which to add or remove data.</param>
        /// <param name="bytesToAddOrRemove">The number of bytes to add (positive value) or remove (negative value). A value of 0 will do nothing.</param>
        /// <returns>The new size of the byte[] array.</returns>
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

        /// <summary>
        /// Resizes a specific region 
        /// </summary>
        /// <param name="offset">The starting offset of the range to resize.</param>
        /// <param name="currentSize">The size of the region of resize.</param>
        /// <param name="newSize">The new size of the region.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Gets a copy of all data.
        /// </summary>
        /// <returns>A byte[] containing a copy of all data.</returns>
        public byte[] GetDataCopy() => (byte[]) Bytes.Clone();

        /// <summary>
        /// Gets a copy of all data in a specific range.
        /// </summary>
        /// <param name="offset">The start of the data to get.</param>
        /// <param name="length">The length of the data to get.</param>
        /// <returns>A byte[] containing a copy of data from 'offset' of size 'length'.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the requested data is out of range of 'length' is less than 0.</exception>
        public byte[] GetDataCopyAt(int offset, int length) {
            if (offset < 0 || offset >= Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0 || offset + length >= Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            var bytes = new byte[length];
            unsafe {
                fixed (byte* dest = bytes, src = Bytes)
                    _ = memcpy((IntPtr) dest, (IntPtr) src + offset, length);
            }

            return bytes;
        }

        /// <summary>
        /// Sets all data to 'data', updating size.
        /// </summary>
        /// <param name="data"></param>
        public void SetDataTo(byte[] data) => Bytes = (byte[]) data.Clone();

        /// <summary>
        /// Replaces data at 'offset' to data supplied by 'data'.
        /// </summary>
        /// <param name="offset">The start position of the data to be replaced.</param>
        /// <param name="data">The data that the byte array at point 'offset' should be replaced with.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the offset is out of range or 'offset + data.Length'
        /// is greater than the length of the byte array.</exception>
        public void SetDataAtTo(int offset, byte[] data) {
            if (offset < 0 || offset >= Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + data.Length >= Bytes.Length)
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
