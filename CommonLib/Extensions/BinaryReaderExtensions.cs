using System;
using System.IO;
using System.Linq;

namespace CommonLib.Extensions {
    public static class BinaryReaderExtensions {
        /// <summary>
        /// Reads a 32-bit little endian integer from a BinaryReader
        /// Credit to Agrathejagged for this code! https://github.com/Agrathejagged
        /// </summary>
        /// <param name="reader">The BinaryReader from which to read the data</param>
        /// <returns>An integer in native format</returns>
        public static int ReadLittleEndianInt32(this BinaryReader reader)
            => BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
    }
}
