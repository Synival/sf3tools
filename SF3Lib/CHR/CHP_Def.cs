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
        public int ExpectedSize;
        public Dictionary<int, CHR_Def> CHRsByOffset;

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
        /// Writes the CHP_Def to a stream in .CHP format.
        /// </summary>
        /// <param name="outputStream">Stream to export the .CHP-formatted data to.</param>
        /// <returns>The number of bytes written to outputStream.</returns>
        public int ToCHP_File(Stream outputStream) {
            var startPosition = outputStream.Position;

            foreach (var chrKv in CHRsByOffset.OrderBy(x => x.Key)) {
                var offset = chrKv.Key + startPosition;
                var chr = chrKv.Value;

                var padding = offset - outputStream.Position;
                if (padding > 0)
                    outputStream.Write(new byte[padding], 0, (int) padding);
                else if (padding < 0) {
                    // TODO: If this ever happens, something is seriously wrong!!
                    outputStream.Position = offset;
                }

                chr.ToCHR_File(outputStream);
            }

            var eofPadding = ExpectedSize - outputStream.Position - startPosition;
            if (eofPadding > 0)
                outputStream.Write(new byte[eofPadding], 0, (int) eofPadding);

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
