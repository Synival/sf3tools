using System;
using System.Collections.Generic;
using System.Text;

namespace DFRLib {
    /// <summary>
    /// A sequence of contiguous bytes changed at an address.
    /// </summary>
    public class ByteDiffChunk {
        /// <summary>
        /// Converts a sequence of bytes to a long string of hex values.
        /// </summary>
        /// <param name="bytes">A sequence of bytes.</param>
        /// <returns>A string.</returns>
        public static string MakeByteHexStr(IEnumerable<byte> bytes) {
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// Creates a ByteDiffChunk with an address, the old bytes, and the new bytes.
        /// </summary>
        /// <param name="address">Where the bytes where changed.</param>
        /// <param name="bytesFrom">What was originally present. Must be the same size as 'bytesTo'.</param>
        /// <param name="bytesTo">What the bytes have been changed to. Must be the same size as 'bytesFrom'.</param>
        /// <exception cref="ArgumentException">Thrown if 'bytesFrom' and 'bytesTo' have differing size.</exception>
        public ByteDiffChunk(ulong address, byte[] bytesFrom, byte[] bytesTo) {
            Address = address;
            if (bytesFrom.Length != bytesTo.Length)
                throw new ArgumentException(nameof(bytesFrom) + " and " + nameof(bytesTo) + " must be the same size");

            BytesFrom = bytesFrom;
            BytesTo = bytesTo;
        }

        /// <summary>
        /// Creates a ByteDiffChunk with an address, the old bytes, and the new bytes.
        /// The 'size' parameter is used to use the first sequence of bytes in the 'bytesFrom' and 'bytesTo' buffers.
        /// </summary>
        /// <param name="address">Where the bytes where changed.</param>
        /// <param name="bytesFrom">What was originally present. Only 'size' number of bytes will be used.</param>
        /// <param name="bytesTo">What the bytes have been changed to. Only 'size' number of bytes will be used.</param>
        /// <param name="fromSize">The size of the 'from' chunk.</param>
        /// <param name="toSize">The size of the 'to' chunk.</param>
        /// <exception cref="ArgumentException">Thrown if 'bytesFrom' or 'bytesTo' have fewer bytes than 'size'.</exception>
        public ByteDiffChunk(ulong address, byte[] bytesFrom, byte[] bytesTo, int fromSize, int toSize) {
            Address = address;
            if (bytesFrom.Length < fromSize)
                throw new ArgumentException(nameof(bytesFrom) + " is smaller than its requested size");
            if (bytesTo.Length < toSize)
                throw new ArgumentException(nameof(bytesTo) + " is smaller than its requested size");

            if (bytesFrom.Length != fromSize) {
                var newBytes = new byte[fromSize];
                Array.Copy(bytesFrom, newBytes, fromSize);
                bytesFrom = newBytes;
            }

            if (bytesTo.Length != toSize) {
                var newBytes = new byte[toSize];
                Array.Copy(bytesTo, newBytes, toSize);
                bytesTo = newBytes;
            }

            BytesFrom = bytesFrom;
            BytesTo = bytesTo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A string.</returns>
        public string ToDFR() => Address.ToString("x") + "," + MakeByteHexStr(BytesFrom) + "," + MakeByteHexStr(BytesTo);

        /// <summary>
        /// Where the diff chunk begins.
        /// </summary>
        public ulong Address { get; }

        /// <summary>
        /// The number of bytes affected.
        /// </summary>
        public int Size => BytesFrom.Length;

        /// <summary>
        /// The original bytes.
        /// </summary>
        public byte[] BytesFrom { get; }

        /// <summary>
        /// The new bytes.
        /// </summary>
        public byte[] BytesTo { get; }
    }
}
