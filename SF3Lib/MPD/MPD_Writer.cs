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
            // VOID.MPD tables:
            // X 0x0000: Offset of pointer to header (int), followed by 8 bytes of 0x00
            // X 0x000C: Light palette
            // X 0x004C: Light positions
            // X 0x0050: Unknown1
            // X 0x0090: Model switch groups
            // X 0x0094: Texture animations
            // X 0x0096: Unknown2
            // X 0x0098: Ground animation
            // X 0x009A: Boundaries
            // X 0x00AA: Texture anim alt
            // X 0x00AC: Palette 1 (actually header)
            // X 0x00AC: Palette 2 (also actually header)
            // X 0x00AC: Header
            // X 0x0104: Pointer to header
            // X 0x2000: Chunk table
            // X 0x2100: Model chunk
            // - 0x2898: Surface (compressed)
            // - 0x2C3C: Textures 1
            // - 0x32B0: Textures 2
            // - 0x32B8: Textures 3
            // - 0x32C0: Textures 4
            // - 0x32C8: Textures 5
            // - 0x32D0: Chest 1 Textures
            // - 0x3DB0: Chest 2 Textures
            // - 0x46F4: Barrel Textures

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
