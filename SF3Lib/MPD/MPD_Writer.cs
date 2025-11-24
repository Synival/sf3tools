using System.IO;
using SF3.Files;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    /// <summary>
    /// Performs the writing of binary data to an MPD file.
    /// </summary>
    public partial class MPD_Writer : BinaryFileWriter {
        public MPD_Writer(Stream stream, ScenarioType scenario) : base(stream) {
            Scenario = scenario;
        }

        /// <summary>
        /// Writes an entire MPD_File's contents to the stream.
        /// </summary>
        /// <param name="mpd">The MPD_File to write to the stream.</param>
        public void WriteMPD(IMPD_File mpd) {
            // Write the main section of the MPD (0x0000 - 0x2000).
            WriteMain(mpd);

            // Write zeroes up until 0x2000 (empty space before the chunk table) and all the way through 0x2100,
            // which writes all zeroes for the chunk table. The chunk table's actual entries will be written as
            // chunks are written.
            WriteBytes(new byte[0x2100 - CurrentOffset]);

            // Write all chunks in the file.
            WriteChunks(mpd);
        }

        private void WriteMPDPointer(int? offset)
            => WriteInt(offset.HasValue ? (offset.Value + 0x290000) : 0);

        private void WriteMPDPointer(uint? offset)
            => WriteUInt(offset.HasValue ? (offset.Value + 0x290000) : 0);

        public ScenarioType Scenario { get; }
    }
}
