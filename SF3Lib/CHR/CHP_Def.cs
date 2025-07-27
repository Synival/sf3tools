using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Models.Files.CHP;
using SF3.Types;

namespace SF3.CHR {
    public class CHP_Def : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a CHP_Def.
        /// </summary>
        /// <param name="json">CHP_Def in JSON format as a string.</param>
        /// <returns>A new CHP_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHP_Def FromJSON(string json) {
            var chpDef = new CHP_Def();
            return chpDef.AssignFromJSON_String(json) ? chpDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a CHP_Def.
        /// </summary>
        /// <param name="jToken">CHP_Def as a JToken.</param>
        /// <returns>A new CHP_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHP_Def FromJToken(JToken jToken) {
            var chpDef = new CHP_Def();
            return chpDef.AssignFromJToken(jToken) ? chpDef : null;
        }

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
                    try { throw new InvalidDataException(); } catch { }
                }

                var bytesWritten = chr.ToCHR_File(outputStream);
                if (chr.MaxSize.HasValue && bytesWritten > chr.MaxSize) {
                    // TODO: this is a serious error; how should we handle it?
                    try { throw new InvalidDataException(); } catch { }
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

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return false;

            try {
                var jObj = (JObject) jToken;
                TotalSectors = jObj.TryGetValue("TotalSectors", out var totalSectors) ? ((int?) totalSectors ?? 0) : 0;

                if (jObj.TryGetValue("CHRsBySector", out var chrsBySectorOut) && chrsBySectorOut.Type == JTokenType.Object) {
                    CHRsBySector = ((IDictionary<string, JToken>) ((JObject) chrsBySectorOut))
                        .Where(x => int.TryParse(x.Key, out _))
                        .ToDictionary(x => int.Parse(x.Key), x => CHR_Def.FromJToken(x.Value));
                }

                return true;
            }
            catch {
                return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() {
            return new JObject {
                { "TotalSectors", new JValue(TotalSectors) },
                { "CHRsBySector", JObject.FromObject(CHRsBySector.ToDictionary(x => x.Key, x => x.Value.ToJToken())) },
            };
        }

        public int TotalSectors;
        public Dictionary<int, CHR_Def> CHRsBySector;
    }
}
