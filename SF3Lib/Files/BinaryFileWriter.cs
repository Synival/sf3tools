using System;
using System.IO;

namespace SF3.Files {
    /// <summary>
    /// Performs the writing of binary data to an MPD file.
    /// </summary>
    public abstract class BinaryFileWriter {
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
                Write(new byte[alignment - pos % alignment]);
        }

        /// <summary>
        /// Ends the file, likely by writing some necessary padding.
        /// </summary>
        public abstract void Finish();

        /// <summary>
        /// Writes arbitrary data to the stream. Should only be used when the stream position is at the end.
        /// </summary>
        /// <param name="bytes">Bytes to write to the stream.</param>
        public void Write(byte[] bytes) {
            Stream.Write(bytes, 0, bytes.Length);
            BytesWritten += bytes.Length;
        }

        /// <summary>
        /// Sets the stream position to an offset, performs an action, and returns to the original offset.
        /// </summary>
        /// <param name="offset">Offset to jump to and perform 'action'.</param>
        /// <param name="action">Action to perform.</param>
        protected void AtOffset(long offset, Action<int> action) {
            var oldPosition = Stream.Position;
            Stream.Position = offset;
            action((int) (oldPosition - StreamStartPosition));
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
            foreach (var offset in offsets) {
                if (offset != 0) {
                    Stream.Position = currentPosition = offset;
                    action((int) (oldPosition - StreamStartPosition));
                }
            }
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
    }
}
