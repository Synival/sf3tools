using System;

namespace CommonLib.Arrays {
    public class ByteArrayResizedArgs {
        public ByteArrayResizedArgs(int offset, int bytesAddedOrRemoved) {
            Offset = offset;
            BytesAddedOrRemoved = bytesAddedOrRemoved;
        }

        /// <summary>
        /// The position at which bytes were added or removed.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// The number of bytes added (positive) or removed (negative).
        /// </summary>
        public int BytesAddedOrRemoved { get; }
    }

    public delegate void ByteArrayResizedHandler(object sender, ByteArrayResizedArgs args);

    public interface IByteArray {
        /// <summary>
        /// Resizes the byte[] array, adding 0's to the end for additional data or truncating data at the end.
        /// </summary>
        /// <param name="size">The new size of the byte[] array.</param>
        void Resize(int size);

        /// <summary>
        /// Adds or removes bytes at a specific offset. Use this to expand or contract the byte[] array at specific points.
        /// Bytes are added or removed at the 'offset' position, not before.
        /// </summary>
        /// <param name="offset">The offset at which to add or remove data.</param>
        /// <param name="bytesToAddOrRemove">The number of bytes to add (positive value) or remove (negative value). A value of 0 will do nothing.</param>
        /// <returns>The new size of the byte[] array.</returns>
        int ExpandOrContractAt(int offset, int bytesToAddOrRemove);

        /// <summary>
        /// Resizes a specific region 
        /// </summary>
        /// <param name="offset">The starting offset of the range to resize.</param>
        /// <param name="currentSize">The size of the region of resize.</param>
        /// <param name="newSize">The new size of the region.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void ResizeAt(int offset, int currentSize, int newSize);

        /// <summary>
        /// Gets a copy of all data.
        /// </summary>
        /// <returns>A byte[] containing a copy of all data.</returns>
        byte[] GetDataCopy();

        /// <summary>
        /// Gets a copy of all data in a specific range.
        /// </summary>
        /// <param name="offset">The start of the data to get.</param>
        /// <param name="length">The length of the data to get.</param>
        /// <returns>A byte[] containing a copy of data from 'offset' of size 'length'.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the requested data is out of range of 'length' is less than 0.</exception>
        byte[] GetDataCopyAt(int offset, int length);

        /// <summary>
        /// Sets all data to 'data', updating size.
        /// </summary>
        /// <param name="data">Incoming data to completely replace existing data.</param>
        void SetDataTo(byte[] data);

        /// <summary>
        /// Replaces data at 'offset' to data supplied by 'data'.
        /// </summary>
        /// <param name="offset">The start position of the data to be replaced.</param>
        /// <param name="data">The data that the byte array at point 'offset' should be replaced with.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the offset is out of range or 'offset + data.Length'
        /// is greater than the length of the byte array.</exception>
        void SetDataAtTo(int offset, byte[] data);

        int Length { get; }

        byte this[int index] { get; set; }

        event EventHandler Modified;
        event ByteArrayResizedHandler Resized;
    }
}