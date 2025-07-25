using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using Newtonsoft.Json;
using SF3.Models.Files.CHP;
using SF3.Types;

namespace SF3.CHR {
    public class CHP_Def {
        public int TotalSectors;
        public Dictionary<int, CHR_Def> CHRsBySector;

        /// <summary>
        /// Converts the CHP_Def to a CHP_File.
        /// </summary>
        /// <param name="nameGetterContext">Context for getting named values.</param>
        /// <param name="scenario">Scenario to which this CHP belongs.</param>
        /// <returns></returns>
        public ICHP_File ToCHP_File(INameGetterContext nameGetterContext, ScenarioType scenario) {
            byte[] data;
            using (var stream = new MemoryStream()) {
                ToCHP_File(stream);
                data = stream.ToArray();
            }
            return CHP_File.Create(new ByteData.ByteData(new ByteArray(data)), nameGetterContext, scenario);
        }

        /// <summary>
        /// Converts the CHP_Def to a CHP_File using a pre-existing buffer.
        /// </summary>
        /// <param name="nameGetterContext">Context for getting named values.</param>
        /// <param name="scenario">Scenario to which this CHP belongs.</param>
        /// <param name="buffer">A pre-existing buffer to which the CHP will be written. Empty space between
        /// CHR files will be skipped rather than written with zeroes. This buffer must be large enough to
        /// contain the entire CHP file.</param>
        /// <returns></returns>
        public ICHP_File ToCHP_File(INameGetterContext nameGetterContext, ScenarioType scenario, byte[] buffer) {
            ToCHP_File(buffer);
            return CHP_File.Create(new ByteData.ByteData(new ByteArray(buffer)), nameGetterContext, scenario);
        }

        /// <summary>
        /// Writes the CHP_Def to a stream in .CHP format.
        /// </summary>
        /// <param name="outputStream">Stream to export the .CHP-formatted data to.</param>
        /// <returns>The number of bytes written to outputStream.</returns>
        public int ToCHP_File(Stream outputStream)
            => ToCHP_FileInternal(outputStream, false);

        /// <summary>
        /// Writes the CHP_Def to a stream in .CHP format.
        /// </summary>
        /// <param name="outputStream">Stream to export the .CHP-formatted data to.</param>
        /// <param name="buffer">A pre-existing buffer to which the CHP will be written. Empty space between
        /// CHR files will be skipped rather than written with zeroes. This buffer must be large enough to
        /// contain the entire CHP file.</param>
        /// <returns>The number of bytes written to outputStream.</returns>
        public int ToCHP_File(byte[] buffer) {
            using (var stream = new MemoryStream(buffer))
                return ToCHP_FileInternal(stream, true);
        }

        private int ToCHP_FileInternal(Stream outputStream, bool seekInsteadOfWrite) {
            var startPosition = outputStream.Position;

            foreach (var chrKv in CHRsBySector.OrderBy(x => x.Key)) {
                var offset = (chrKv.Key * 0x800) + startPosition;
                var chr = chrKv.Value;

                var padding = offset - outputStream.Position;
                if (padding > 0) {
                    if (seekInsteadOfWrite)
                        outputStream.Position += padding;
                    else
                        outputStream.Write(new byte[padding], 0, (int) padding);
                }
                else if (padding < 0) {
                    // TODO: this is a serious error; how should we handle it?
                    outputStream.Position = offset;
                }

                var bytesWritten = chr.ToCHR_File(outputStream);
                if (chr.MaxSize.HasValue && bytesWritten > chr.MaxSize) {
                    // TODO: this is a serious error; how should we handle it?
                    ;
                }
            }

            var eofPadding = (TotalSectors * 0x800) - outputStream.Position - startPosition;
            if (eofPadding > 0) {
                if (seekInsteadOfWrite)
                    outputStream.Position += eofPadding;
                else
                    outputStream.Write(new byte[eofPadding], 0, (int) eofPadding);
            }

            return (int) (outputStream.Position - startPosition);
        }

        /// <summary>
        /// Converts the CHP_Def to a JSON object string.
        /// </summary>
        /// <returns>A string in JSON format.</returns>
        public string ToJSON_String() {
            var settings = new JsonSerializerSettings() {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
