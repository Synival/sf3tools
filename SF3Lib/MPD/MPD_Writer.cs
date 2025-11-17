using System;
using System.IO;
using SF3.Files;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    /// <summary>
    /// Performs the writing of binary data to an MPD file.
    /// </summary>
    public class MPD_Writer : BinaryFileWriter {
        public MPD_Writer(Stream stream, ScenarioType scenario) : base(stream) {
            Scenario = scenario;
        }

        public override void Finish() {
            // TODO: do what's necessary to finish.
        }

        /// <summary>
        /// Writes an entire MPD_File's contents to the stream.
        /// </summary>
        /// <param name="mpd">The MPD_File to write to the stream.</param>
        public void WriteMPD(MPD_File mpd) {
            // TODO: write data table by table.
            Write(mpd.Data.GetDataCopyOrReference());
            Finish();
        }

        public ScenarioType Scenario { get; }
    }
}
