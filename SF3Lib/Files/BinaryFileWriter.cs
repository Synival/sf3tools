using System;
using System.IO;
using CommonLib.Utils;

namespace SF3.Files {
    /// <summary>
    /// Performs the writing of binary data to an MPD file.
    /// </summary>
    public class BinaryFileWriter {
        public BinaryFileWriter(Stream stream) {
            StreamStartPosition = stream.Position;
            Stream = stream;
        }

        /// <summary>
        /// Writes enough bytes so the position is divisible by 'alignment'.
        /// </summary>
        /// <param name="alignment">The number of bytes the stream's length should be visible by.</param>
        public void WriteToAlignTo(int alignment) {
            var pos = CurrentOffset;
            if (pos % alignment != 0)
                WriteBytes(new byte[alignment - pos % alignment]);
        }

        /// <summary>
        /// Writes an arbitrary number of bytes to the stream.
        /// </summary>
        /// <param name="bytes">Bytes to write to the stream.</param>
        public void WriteBytes(byte[] bytes) {
            Stream.Write(bytes, 0, bytes.Length);
            if (AtEndOfStream)
                BytesWritten += bytes.Length;
        }

        /// <summary>
        /// Writes an arbitrary number of bytes to the stream.
        /// </summary>
        /// <param name="bytes">Bytes to write to the stream.</param>
        /// <param name="offset">Start position of data in 'bytes' to write.</param>
        /// <param name="length">Number of bytes to write.</param>
        public void WriteBytes(byte[] bytes, int offset, int length) {
            Stream.Write(bytes, offset, length);
            if (AtEndOfStream)
                BytesWritten += length;
        }

        public void WriteUShorts(ushort[] values)
            => WriteBytes(values.ToBytes());

        public void WriteShorts(short[] values)
            => WriteBytes(values.ToBytes());

        public void WriteUInts(uint[] values)
            => WriteBytes(values.ToBytes());

        public void WriteInts(int[] values)
            => WriteBytes(values.ToBytes());

        public void WriteByte(byte value) {
            Stream.WriteByte(value);
            if (AtEndOfStream)
                BytesWritten++;
        }

        public void WriteUShort(ushort value) {
            WriteToAlignTo(2);
            Stream.WriteByte((byte) (value >> 8));
            Stream.WriteByte((byte) value);
            if (AtEndOfStream)
                BytesWritten += 2;
        }

        public void WriteShort(short value)
            => WriteUShort((ushort) value);

        public void WriteUInt(uint value) {
            WriteToAlignTo(4);
            Stream.WriteByte((byte) (value >> 24));
            Stream.WriteByte((byte) (value >> 16));
            Stream.WriteByte((byte) (value >> 8));
            Stream.WriteByte((byte) value);
            if (AtEndOfStream)
                BytesWritten += 4;
        }

        public void WriteInt(int value)
            => WriteUInt((uint) value);

        /// <summary>
        /// Sets the stream position to an offset, performs an action, and returns to the original offset.
        /// </summary>
        /// <param name="offset">Offset to jump to and perform 'action'.</param>
        /// <param name="action">Action to perform.</param>
        protected void AtOffset(long offset, Action<int> action) {
            var oldPosition = Stream.Position;
            Stream.Position = offset;
            SeekCount++;
            action((int) (oldPosition - StreamStartPosition));
            SeekCount--;
            Stream.Position = oldPosition;
        }

        /// <summary>
        /// For each offset, sets the stream position to the offset and performs the action. Once finished, returns to the original offset.
        /// </summary>
        /// <param name="offsets">Offsets to jump to and perform 'action'.</param>
        /// <param name="action">Action to perform.</param>
        protected void AtOffsets(long[] offsets, Action<int> action) {
            var oldPosition = Stream.Position;
            var currentPosition = oldPosition;
            SeekCount++;
            foreach (var offset in offsets) {
                if (offset != 0) {
                    Stream.Position = currentPosition = offset;
                    action((int) (oldPosition - StreamStartPosition));
                }
            }
            SeekCount--;
            if (currentPosition != oldPosition)
                Stream.Position = oldPosition;
        }

        public long StreamStartPosition { get; }
        public Stream Stream { get; }
        public int BytesWritten { get; private set; } = 0;

        public long CurrentOffset {
            get => Stream.Position - StreamStartPosition;
            set => Stream.Position = value + StreamStartPosition;
        }

        private int SeekCount = 0;
        private bool AtEndOfStream => SeekCount == 0;
    }
}
