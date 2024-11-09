using System;
using System.IO;
using System.Linq;

namespace MPDLib.Extensions {
    public static class BinaryReaderExtensions {
        public static int ReadLittleEndianInt32(this BinaryReader reader)
            => BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
    }
}
