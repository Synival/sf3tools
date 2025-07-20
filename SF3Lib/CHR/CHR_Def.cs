using System;
using System.IO;
using System.Linq;
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

            foreach (var chr in Sprites) {
                chrWriter.WriteHeaderEntry(
                    (ushort) chr.SpriteID,
                    (ushort) chr.Width,
                    (ushort) chr.Height,
                    (byte) chr.Directions,
                    (byte) chr.VerticalOffset,
                    (byte) chr.Unknown0x08,
                    (byte) chr.CollisionSize,
                    (byte) (chr.PromotionLevel ?? 0),
                    (int) Math.Round(chr.Scale.Value * 65536.0f)
                );
            }
            chrWriter.WriteHeaderTerminator();

            foreach (var (sprite, i) in Sprites.Select((x, i) => (CHR: x, Index: i)))
                chrWriter.WriteEmptyAnimationTable(i);

            foreach (var (sprite, i) in Sprites.Select((x, i) => (CHR: x, Index: i)))
                chrWriter.WriteEmptyFrameTable(i);

            return chrWriter.BytesWritten;
        }
    }
}
