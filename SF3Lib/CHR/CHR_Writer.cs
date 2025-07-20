using System;
using System.IO;

namespace SF3.CHR {
    public class CHR_Writer {
        public CHR_Writer(Stream stream) {
            Stream = stream;
        }

        /// <summary>
        /// Writes the final 24-byte entry indicating the end of the header.
        /// </summary>
        public void AddHeaderTerminator() {
            Stream.Write(new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                }, 0, 0x18);
            BytesWritten += 0x18;
        }

        public Stream Stream { get; }
        public int BytesWritten { get; private set; } = 0;
    }
}
