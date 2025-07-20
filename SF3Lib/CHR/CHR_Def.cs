using System.IO;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.Models.Files.CHR;
using SF3.Types;

namespace SF3.CHR {
    public class CHR_Def {
        public SpriteDef[] Sprites;

        /// <summary>
        /// Converts the CHR_Def to a CHR_File.
        /// </summary>
        /// <param name="nameGetterContext">Context for getting named values.</param>
        /// <param name="scenario">Scenario to which this CHR belongs.</param>
        /// <returns></returns>
        public CHR_File ToCHR_File(INameGetterContext nameGetterContext, ScenarioType scenario) {
            byte[] data;
            using (var stream = new MemoryStream()) {
                ToCHR_File(stream);
                data = stream.ToArray();
            }
            return CHR_File.Create(new ByteData.ByteData(new ByteArray(data)), nameGetterContext, scenario);
        }

        /// <summary>
        /// Writes the CHR_Def to a stream in .CHR format.
        /// </summary>
        /// <param name="outputStream">Stream to export the .CHR-formatted data to.</param>
        /// <returns>The number of bytes written to outputStream.</returns>
        public int ToCHR_File(Stream outputStream) {
            var chrWriter = new CHR_Writer(outputStream);
            chrWriter.AddHeaderTerminator();
            return chrWriter.BytesWritten;
        }
    }
}
