using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                _ = sb.Append(b.ToString("x2"));
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
        /// Creates a byte chunk based on a row in a DFR File.
        /// Comments are pre-processed out. The row must have valid data in the proper format, or it will throw.
        /// </summary>
        /// <param name="dfrRow">A string representation of the DFR row.</param>
        /// <exception cref="ArgumentNullException">Thrown if 'dfrRow' is null.</exception>
        /// <exception cref="ArgumentException">Thrown if 'dfrRow' has no data or data with an invalid format.</exception>
        public ByteDiffChunk(string dfrRow) {
            if (dfrRow == null)
                throw new ArgumentNullException(nameof(dfrRow));

            // Ignore comments.
            var index = dfrRow.IndexOf(';');
            if (index >= 0)
                dfrRow = dfrRow.Substring(0, index).Trim();

            // Get sections:
            //     <address>,<in-data>,<out-data>
            var sections = dfrRow.Split(',').Select(x => x.Trim()).ToArray();
            if (sections.Length == 0 || sections[0] == "")
                throw new ArgumentException("No data");
            if (sections.Length != 3)
                throw new ArgumentException("Expected 3 sections, got " + sections.Length + ": " + dfrRow);

            // Get the address.
            Address = ulong.TryParse(sections[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var addr)
                ? addr
                : throw new ArgumentException("Invalid address: " + sections[0]);

            byte[] GetByteList(string argName, string argValue) {
                byte chToHexDigit(char ch) {
                    return ch >= '0' && ch <= '9'
                        ? (byte) (ch - '0')
                        : ch >= 'a' && ch <= 'f'
                        ? (byte) (ch - 'a' + 0x0a)
                        : ch >= 'A' && ch <= 'F' ? (byte) (ch - 'A' + 0x0a) : throw new ArgumentException("Not a hex char: " + ch);
                }

                if (argValue.Length % 2 != 0)
                    throw new ArgumentException(argName + " has wrong length: " + argValue);
                var bytes = new byte[argValue.Length / 2];
                var b = 0;
                for (var i = 0; i < argValue.Length; i += 2) {
                    var b1 = chToHexDigit(argValue[i + 0]);
                    var b2 = chToHexDigit(argValue[i + 1]);
                    bytes[b++] = (byte) ((b1 << 4) + b2);
                }

                return bytes;
            }

            // Get bytes from and to.
            var bytesFrom = GetByteList(nameof(sections) + "[1]", sections[1]);
            var bytesTo = GetByteList(nameof(sections) + "[2]", sections[2]);
            if (bytesFrom.Length != 0 && bytesFrom.Length != bytesTo.Length)
                throw new ArgumentException("Doesn't match format of alter or append chunk: " + dfrRow);

            BytesFrom = bytesFrom;
            BytesTo = bytesTo;
        }

        /// <summary>
        /// Converts the byte chunk into a row for a DFR file.
        /// </summary>
        /// <returns>A string.</returns>
        public string ToDFR() => Address.ToString("x") + "," + MakeByteHexStr(BytesFrom) + "," + MakeByteHexStr(BytesTo);

        /// <summary>
        /// Applies the changes in this chunk to data. The data must be large enough to apply the change,
        /// and must have the expected data if 'BytesFrom' is set.
        /// </summary>
        /// <param name="data">Data to modify.</param>
        /// <exception cref="ArgumentException">Thrown if the data is too small to apply the change or the expected data is incorrect.</exception>
        public void ApplyTo(byte[] data) {
            // Ensure that the expected data is correct.
            if (data.Length < (int) Address + BytesTo.Length) {
                throw new ArgumentException("Data is too short; must be at least " + ((int) Address + BytesTo.Length) + " bytes, " +
                    "but is only " + data.Length + " bytes.");
            }

            for (var i = 0; i < BytesFrom.Length; i++) {
                if (data[i + (int) Address] != BytesFrom[i]) {
                    throw new ArgumentException(
                        "Expected " + BytesFrom[i].ToString("X2") + " at address " + Address.ToString("X") +
                        ", instead got " + data[i + (int) Address]);
                }
            }

            // Set data.
            BytesTo.CopyTo(data, (int) Address);
        }

        /// <summary>
        /// Where the diff chunk begins.
        /// </summary>
        public ulong Address { get; }

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
