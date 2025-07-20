using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using CommonLib.Utils;
using Newtonsoft.Json;
using SF3.Models.Files.CHR;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace SF3.CHR {
    public class CHR_Def {
        public SpriteDef[] Sprites;

        /// <summary>
        /// Deserializes a JSON object of a CHR_Def.
        /// </summary>
        /// <param name="json">CHR_Def in JSON format as a string.</param>
        /// <returns>A new CHR_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHR_Def FromJSON(string json) {
            try {
                return JsonConvert.DeserializeObject<CHR_Def>(json);
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Converts the CHR_Def to a JSON object string.
        /// </summary>
        /// <returns>A string in JSON format.</returns>
        public string ToJSON_String() {
            var settings = new JsonSerializerSettings() {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.SerializeObject(this, settings);
        }

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

            // Write the header table with all sprite definitions and offsets for their own tables.
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

            // Write all animation tables.
            foreach (var (sprite, i) in Sprites.Select((x, i) => (CHR: x, Index: i)))
                chrWriter.WriteEmptyAnimationTable(i);

            // TODO: temporary, until we write the real images!!
            var imageKeys = new HashSet<string>();

            // Write all frame tables.
            foreach (var (sprite, i) in Sprites.Select((x, i) => (CHR: x, Index: i))) {
                var spritesheetKey = SpritesheetDef.DimensionsToKey(sprite.Width, sprite.Height);

                foreach (var spriteFrames in sprite.SpriteFrames ?? new SpriteFramesDef[0]) {
                    var spriteName = spriteFrames.SpriteName ?? sprite.SpriteName;
                    var spriteDef = SpriteUtils.GetSpriteDef(spriteName);
                    var spritesheet = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;

                    foreach (var frameGroup in spriteFrames.FrameGroups ?? new FrameGroupDef[0]) {
                        var directions =
                            frameGroup.Directions?.Select(x => (SpriteFrameDirection) Enum.Parse(typeof(SpriteFrameDirection), x))?.ToArray()
                            ?? CHR_Utils.GetCHR_FrameGroupDirections(sprite.Directions);

                        foreach (var direction in directions) {
                            // TODO: temporary, until we write the real images!!
                            imageKeys.Add(spritesheetKey);
                            chrWriter.WriteFrameTableFrame(i, spritesheetKey);
                        }
                    }
                }
                chrWriter.WriteFrameTableTerminator(i);
            }

            // TODO: temporary, until we write the real images!!
            foreach (var key in imageKeys) {
                var imageSize = SpritesheetDef.KeyToDimensions(key);
                var data = new ushort[imageSize.Width * imageSize.Height];
                for (int i = 0; i < data.Length; i++)
                    data[i] = 0xff00;

                chrWriter.WriteFrameImage(key, Compression.CompressSpriteData(data, 0, data.Length));
            }

            return chrWriter.BytesWritten;
        }
    }
}
