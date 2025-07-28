using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using CommonLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Extensions;
using SF3.Models.Files.CHR;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace SF3.CHR {
    public class CHR_Def : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a CHR_Def.
        /// </summary>
        /// <param name="json">CHR_Def in JSON format as a string.</param>
        /// <returns>A new CHR_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHR_Def FromJSON(string json) {
            var chrDef = new CHR_Def();
            return chrDef.AssignFromJSON_String(json) ? chrDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a CHR_Def.
        /// </summary>
        /// <param name="jToken">CHR_Def as a JObject.</param>
        /// <returns>A new CHR_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHR_Def FromJToken(JToken jToken) {
            var chpDef = new CHR_Def();
            return chpDef.AssignFromJToken(jToken) ? chpDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return true;

            try {
                var jObj = (JObject) jToken;

                WriteFrameImagesBeforeTables = jObj.TryGetValue("WriteFrameImagesBeforeTables", out var wfi)     ? ((bool?) wfi)    : null;
                MaxSize                      = jObj.TryGetValue("MaxSize",                      out var maxSize) ? ((int?) maxSize) : null;
                JunkAfterFrameTables         = jObj.TryGetValue("JunkAfterFrameTables",         out var jaft)    ? ((byte[]) jaft)  : null;

                Sprites = jObj.TryGetValue("Sprites", out var sprites)
                    ? sprites.Select(x => SpriteDef.FromJToken(x)).ToArray()
                    : null;

                return true;
            }
            catch {
                return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() {
            var jObj = new JObject();
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            if (WriteFrameImagesBeforeTables.HasValue)
                jObj.Add("WriteFrameImagesBeforeTables", new JValue(WriteFrameImagesBeforeTables.Value));
            if (MaxSize.HasValue)
                jObj.Add("MaxSize", new JValue(MaxSize.Value));
            if (JunkAfterFrameTables != null)
                jObj.Add("JunkAfterFrameTables", JToken.FromObject(JunkAfterFrameTables, jsonSettings));
            if (Sprites != null)
                jObj.Add("Sprites", JArray.FromObject(Sprites.Select(x => x.ToJToken()).ToArray(), jsonSettings));

            return jObj;
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

            WriteCHR_Header(chrWriter);
            WriteCHR_Animations(chrWriter);
            WriteCHR_Frames(chrWriter);
            chrWriter.Finish();

            return chrWriter.BytesWritten;
        }

        private void WriteCHR_Header(CHR_Writer chrWriter) {
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
                    (byte) chr.PromotionLevel,
                    (int) Math.Round(chr.Scale * 65536.0f)
                );
            }
            chrWriter.WriteHeaderTerminator();
        }

        private void WriteCHR_Animations(CHR_Writer chrWriter) {
            // Write all individual animations.
            foreach (var (sprite, i) in Sprites.Select((x, i) => (CHR: x, Index: i))) {
                int animationIndex = 0;

                foreach (var animations in sprite.SpriteAnimations ?? new SpriteAnimationsDef[0]) {
                    var spriteName = animations.SpriteName ?? sprite.SpriteName;
                    var spriteDef = SpriteUtils.GetSpriteDef(spriteName);

                    var directions = animations.Directions ?? sprite.Directions;
                    var spritesheetKey = SpritesheetDef.DimensionsToKey(
                        animations.Width ?? sprite.Width, animations.Height ?? sprite.Height);
                    var frameKeyPrefix = $"{spriteName} ({spritesheetKey})";
                    var spritesheetDef = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
                    var spriteAnimsByDirection = (spritesheetDef?.AnimationByDirections?.TryGetValue(directions, out var sadOut) == true) ? sadOut : null;

                    foreach (var animationName in animations.Animations ?? new string[0]) {
                        var spriteAnimation = (animationName != null && spriteAnimsByDirection?.Animations?.TryGetValue(animationName, out var animOut) == true) ? animOut : null;
                        if (spriteAnimation != null) {
                            chrWriter.StartAnimationForCurrentSprite(animationIndex);

                            var currentDirections = directions;
                            foreach (var frame in spriteAnimation.AnimationFrames ?? new AnimationFrameDef[0]) {
                                string[] frameKeys = null;

                                // The 'SetDirectionCount' command (0xF1) updates the number of frames in our key from now on.
                                if (frame.Command == SpriteAnimationFrameCommandType.SetDirectionCount)
                                    currentDirections = frame.Parameter;
                                // If this is a frame, we need to generate a key that will be used to locate a FrameID later.
                                else if (frame.Command == SpriteAnimationFrameCommandType.Frame) {
                                    var frameDirections = CHR_Utils.GetCHR_FrameGroupDirections(currentDirections);

                                    // The 'FrameGroup' command assumes the standard directions for this frame.
                                    if (frame.FrameGroup != null) {
                                        frameKeys = frameDirections
                                            .Select(x => $"{frameKeyPrefix} {frame.FrameGroup} ({x.ToString()})")
                                            .ToArray();
                                    }
                                    // The 'Frames' command has manually specified FrameGroup + Direction pairs.
                                    else if (frame.Frames != null) {
                                        frameKeys = frameDirections
                                            .ToDictionary(x => x, x => frame.Frames.TryGetValue(x.ToString(), out var f) ? f : null)
                                            .Select(x => (x.Value == null) ? null : $"{frameKeyPrefix} {x.Value.Frame} ({x.Value.Direction.ToString()})")
                                            .ToArray();
                                    }
                                    // If nothing is there, something is wrong.
                                    else
                                        // TODO: what to do if we reach this point?
                                        ;
                                }

                                // We have enough info; write the frame.
                                chrWriter.WriteAnimationFrame(i, frame.Command, frame.Parameter, frameKeys);
                            }
                        }

                        animationIndex++;
                    }
                }

                // Now that all frame tables have been written, write the animation table.
                // The CHR_Writer knows the locations of all the frame tables we just wrote,
                // so we don't need to pass it any information.
                chrWriter.WriteAnimationTable(i);
            }
        }

        private class SpritesheetImageRef {
            public Bitmap Bitmap;
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        private void WriteCHR_Frames(CHR_Writer chrWriter) {
            var spritesheetImageDict = new Dictionary<string, Bitmap>();
            var imagesRefsByKey = new Dictionary<string, SpritesheetImageRef>();
            var framesToWriteBySpriteIndex = new Dictionary<int, List<(string FrameKey, string AniFrameKey)>>();

            // CHR files must have had their compressed frames written by sprite, because there's a forced alignment of 4
            // after every sprite's groups of frames. We don't bother to do that -- different sprites can share compressed
            // images -- but most cases are fixed if we record which image is the final image for each sprite.
            var finalSpriteFrames = new HashSet<string>();

            // Keep track of frame images already written if we're written frame images sprite-by-sprite.
            var frameImagesWritten = new HashSet<string>();

            // Write frame tables.
            void WriteFrameTable(int spriteIndex) {
                foreach (var frame in framesToWriteBySpriteIndex[spriteIndex])
                    chrWriter.WriteFrameTableFrame(spriteIndex, frame.FrameKey, frame.AniFrameKey);
                chrWriter.WriteFrameTableTerminator(spriteIndex);
            }

            void WriteFrameTables() {
                for (int i = 0; i < Sprites.Length; i++)
                    WriteFrameTable(i);
            }

            void WriteFrameImages() {
                // Write all images, updating pointers that reference each image by its key.
                foreach (var imageRefKv in imagesRefsByKey.Where(x => !frameImagesWritten.Contains(x.Key)).ToArray()) {
                    var imageKey = imageRefKv.Key;
                    var imageRef = imageRefKv.Value;

                    ushort[] data = null;

                    var bitmap = imageRef.Bitmap;
                    var x1 = imageRef.X;
                    var y1 = imageRef.Y;
                    var x2 = x1 + imageRef.Width;
                    var y2 = y1 + imageRef.Height;

                    // If the image is invalid, display a bright red square instead.
                    if (bitmap == null || x1 < 0 || y1 < 0 || x2 > bitmap.Width || y2 > bitmap.Height) {
                        data = new ushort[imageRef.Width * imageRef.Height];
                        for (int i = 0; i < data.Length; i++)
                            data[i] = 0x801F;
                    }
                    // It looks like a valid image. Copy it into the data.
                    else
                        data = bitmap.GetDataAt(x1, y1, imageRef.Width, imageRef.Height).To1DArrayTransposed();

                    // Write the compressed image, updating any pointers that reference it by its key.
                    chrWriter.WriteFrameImage(imageKey, Compression.CompressSpriteData(data, 0, data.Length));

                    // There's some padding after every sprite's group of frames to enforce an alignment of 4.
                    if (finalSpriteFrames.Contains(imageKey))
                        chrWriter.WriteToAlignTo(4);

                    // Don't write this frame to the CHR file again.
                    frameImagesWritten.Add(imageKey);
                }
            }

            // Build all frame tables and the images to be written.
            foreach (var (sprite, i) in Sprites.Select((x, i) => (CHR: x, Index: i))) {
                string lastFrameKey = null;
                framesToWriteBySpriteIndex.Add(i, new List<(string, string)>());

                foreach (var spriteFrames in sprite.SpriteFrames ?? new SpriteFramesDef[0]) {
                    var spriteName  = spriteFrames.SpriteName ?? sprite.SpriteName;
                    var frameWidth  = spriteFrames.Width      ?? sprite.Width;
                    var frameHeight = spriteFrames.Height     ?? sprite.Height;
                    var spriteDef   = SpriteUtils.GetSpriteDef(spriteName);

                    foreach (var frameGroup in spriteFrames.FrameGroups ?? new FrameGroupDef[0]) {
                        // Attempt to load the spritesheet referenced by the spritesheetDef.
                        // Don't bothe if the def couldn't be found.
                        var spritesheetKey = SpritesheetDef.DimensionsToKey(frameWidth, frameHeight);
                        var spritesheetImageKey = $"{spriteName} ({spritesheetKey})";
                        var spritesheetDef = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
                        var spriteFrameGroupDef = (spritesheetDef?.FrameGroups?.TryGetValue(frameGroup.Name, out var spriteFrameGroupOut) == true) ? spriteFrameGroupOut : null;

                        if (spritesheetDef != null && !spritesheetImageDict.ContainsKey(spritesheetImageKey)) {
                            Bitmap bitmap = null;
                            try {
                                var bitmapPath = SpriteUtils.SpritesheetImagePath($"{SpriteUtils.FilesystemName(spritesheetImageKey)}.BMP");
                                bitmap = (Bitmap) Image.FromFile(bitmapPath);
                            }
                            catch { }
                            spritesheetImageDict.Add(spritesheetImageKey, bitmap);
                        }

                        var frames = frameGroup.Frames
                            ?? CHR_Utils.GetCHR_FrameGroupDirections(sprite.Directions)
                                .Select(x => new FrameDef() { Direction = x })
                                .ToArray();

                        foreach (var frame in frames) {
                            var spriteFrameDef = (spriteFrameGroupDef?.Frames?.TryGetValue(frame.Direction.ToString(), out var spriteFrameOut) == true) ? spriteFrameOut : null;
                            var aniFrameKey = $"{spritesheetImageKey} {frameGroup.Name} ({frame.Direction})";
                            var frameKey = $"{spritesheetImageKey} {aniFrameKey}" + (frame.DuplicateKey == null ? "" : $" ({frame.DuplicateKey})");

                            // Add a reference to the image whether the spritesheet resources were found or not.
                            // If they're invalid, simply display a red image.
                            if (!imagesRefsByKey.ContainsKey(frameKey)) {
                                imagesRefsByKey.Add(frameKey, new SpritesheetImageRef() {
                                    Bitmap = spritesheetImageDict.TryGetValue(spritesheetImageKey, out var bmpOut) ? bmpOut : null,
                                    X = spriteFrameDef?.SpriteSheetX ?? -1,
                                    Y = spriteFrameDef?.SpriteSheetY ?? -1,
                                    Width  = frameWidth,
                                    Height = frameHeight
                                });
                            }

                            framesToWriteBySpriteIndex[i].Add((frameKey, aniFrameKey));
                            lastFrameKey = frameKey;
                        }
                    }
                }

                // Remember what the last image was in this sprite.
                if (lastFrameKey != null)
                    finalSpriteFrames.Add(lastFrameKey);

                // For what are likely older CHRs, images for each sprite are written before their own frame table.
                if (WriteFrameImagesBeforeTables == true) {
                    WriteFrameImages();
                    WriteFrameTable(i);
                }
            }

            // 99% of the time, the images are written after the frame tables.
            if (WriteFrameImagesBeforeTables != true) {
                WriteFrameTables();

                // XBTL127.CHR has junk data after the frame table. Write it here.
                if (JunkAfterFrameTables != null)
                    chrWriter.Write(JunkAfterFrameTables);
                WriteFrameImages();
            }
        }

        public bool? WriteFrameImagesBeforeTables;
        public int? MaxSize;
        public byte[] JunkAfterFrameTables;
        public SpriteDef[] Sprites;
    }
}
