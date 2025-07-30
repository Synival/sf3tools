using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.Extensions;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace SF3.CHR {
    public class CHR_CompilationUnit {
        /// <summary>
        /// Creates a compilation unit/context for a single CHR_Def.
        /// </summary>
        /// <param name="chrDef">The CHR_Def to be compiled.</param>
        public CHR_CompilationUnit(CHR_Def chrDef) {
            _chrDef = chrDef;

            // Build the frame table with image data and other information necessary for writing.
            BuildFrameTableAndImages();
        }

        /// <summary>
        /// Writes the CHR_Def to an output stream.
        /// </summary>
        /// <param name="outputStream">The stream to write the CHR_Def to.</param>
        /// <returns>The number of bytes written to 'outputStream'.</returns>
        public int Write(Stream outputStream) {
            // If the output stream can't seek, we're need to write to an intermediate in-memory stream.
            var targetOutputStream = outputStream.CanSeek ? outputStream : new MemoryStream();
            var chrWriter = new CHR_Writer(targetOutputStream);

            // Write data.
            WriteHeader(chrWriter);
            WriteAnimations(chrWriter);
            WriteFrames(chrWriter);
            chrWriter.Finish();
            var bytesWritten = chrWriter.BytesWritten;

            // If we wrote to an intermediate stream, output the contents to 'outputStream'.
            if (outputStream != targetOutputStream) {
                var compiledChr = ((MemoryStream) targetOutputStream).ToArray();
                outputStream.Write(compiledChr, 0, compiledChr.Length);
            }

            return bytesWritten;
        }

        private void WriteHeader(CHR_Writer chrWriter) {
            // Write the header table with all sprite definitions and offsets for their own tables.
            foreach (var sprite in _chrDef.Sprites) {
                chrWriter.WriteHeaderEntry(
                    (ushort) sprite.SpriteID,
                    (ushort) sprite.Width,
                    (ushort) sprite.Height,
                    (byte) sprite.Directions,
                    (byte) sprite.VerticalOffset,
                    (byte) sprite.Unknown0x08,
                    (byte) sprite.CollisionSize,
                    (byte) sprite.PromotionLevel,
                    (int) Math.Round(sprite.Scale * 65536.0f)
                );
            }
            chrWriter.WriteHeaderTerminator();
        }

        private void WriteAnimations(CHR_Writer chrWriter) {
            // Write all individual animations.
            foreach (var (sprite, spriteIndex) in _chrDef.Sprites.Select((x, i) => (CHR: x, Index: i))) {
                int animationIndex = 0;

                foreach (var animations in sprite.AnimationsForSpritesheetAndDirections ?? new AnimationsForSpritesheetAndDirection[0]) {
                    var spriteName = animations.SpriteName ?? sprite.SpriteName;
                    var spriteDef = SpriteUtils.GetSpriteDef(spriteName);

                    var directions = animations.Directions ?? sprite.Directions;
                    var spritesheetKey = Spritesheet.DimensionsToKey(
                        animations.Width ?? sprite.Width, animations.Height ?? sprite.Height);
                    var frameKeyPrefix = $"{spriteName} ({spritesheetKey})";
                    var spritesheetDef = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
                    var spriteAnimsByDirection = (spritesheetDef?.AnimationSetsByDirections?.TryGetValue(directions, out var sadOut) == true) ? sadOut : null;

                    foreach (var animationName in animations.Animations ?? new string[0]) {
                        var spriteAnimation = (animationName != null && spriteAnimsByDirection?.AnimationsByName?.TryGetValue(animationName, out var animOut) == true) ? animOut : null;
                        if (spriteAnimation != null) {
                            chrWriter.StartAnimationForCurrentSprite(animationIndex);

                            var currentDirections = directions;
                            foreach (var aniCommand in spriteAnimation.AnimationCommands ?? new AnimationCommand[0]) {
                                string[] frameKeys = null;

                                // The 'SetDirectionCount' command (0xF1) updates the number of frames in our key from now on.
                                if (aniCommand.Command == SpriteAnimationCommandType.SetDirectionCount)
                                    currentDirections = (SpriteDirectionCountType) aniCommand.Parameter;
                                // If this is a frame, we need to generate a key that will be used to locate a FrameID later.
                                else if (aniCommand.Command == SpriteAnimationCommandType.Frame) {
                                    var frameDirections = CHR_Utils.GetCHR_FrameGroupDirections(currentDirections);

                                    // The 'FrameGroup' command assumes the standard directions for this frame.
                                    if (aniCommand.FrameGroup != null) {
                                        frameKeys = frameDirections
                                            .Select(x => $"{frameKeyPrefix} {aniCommand.FrameGroup} ({x.ToString()})")
                                            .ToArray();
                                    }
                                    // The 'Frames' command has manually specified FrameGroup + Direction pairs.
                                    else if (aniCommand.FramesByDirection != null) {
                                        frameKeys = frameDirections
                                            .ToDictionary(x => x, x => aniCommand.FramesByDirection.TryGetValue(x, out var f) ? f : null)
                                            .Select(x => (x.Value == null) ? null : $"{frameKeyPrefix} {x.Value.FrameGroup} ({x.Value.Direction.ToString()})")
                                            .ToArray();
                                    }
                                    // If nothing is there, something is wrong.
                                    else
                                        // TODO: what to do if we reach this point?
                                        ;
                                }

                                // We have enough info; write the frame.
                                chrWriter.WriteAnimationCommand(spriteIndex, aniCommand.Command, aniCommand.Parameter, frameKeys);
                            }
                        }

                        animationIndex++;
                    }
                }

                // Now that all frame tables have been written, write the animation table.
                // The CHR_Writer knows the locations of all the frame tables we just wrote,
                // so we don't need to pass it any information.
                chrWriter.WriteAnimationTable(spriteIndex);
            }
        }

        private void WriteFrames(CHR_Writer chrWriter) {
            // For what are likely older CHRs, images for each sprite are written before their own frame table.
            if (_chrDef.WriteFrameImagesBeforeTables == true) {
                for (int i = 0; i < _chrDef.Sprites.Length; i++) {
                    WriteFrameImages(chrWriter);
                    WriteFrameTable(i, chrWriter);
                }
            }
            // 99% of the time, the images are written after the frame tables.
            else {
                WriteFrameTables(chrWriter);

                // XBTL127.CHR has junk data after the frame table. Write it here.
                if (_chrDef.JunkAfterFrameTables != null)
                    chrWriter.Write(_chrDef.JunkAfterFrameTables);
                WriteFrameImages(chrWriter);
            }
        }

        private void BuildFrameTableAndImages() {
            foreach (var (sprite, spriteIndex) in _chrDef.Sprites.Select((x, i) => (CHR: x, Index: i)))
                BuildFrameTableAndImages(sprite, spriteIndex);
        }

        private void BuildFrameTableAndImages(SpriteDef sprite, int spriteIndex) {
            string lastFrameKey = null;
            _framesToWriteBySpriteIndex.Add(spriteIndex, new List<(string, string)>());

            foreach (var spriteFrames in sprite.FrameGroupsForSpritesheets ?? new FrameGroupsForSpritesheet[0]) {
                var spriteName  = spriteFrames.SpriteName ?? sprite.SpriteName;
                var frameWidth  = spriteFrames.Width      ?? sprite.Width;
                var frameHeight = spriteFrames.Height     ?? sprite.Height;
                var spriteDef   = SpriteUtils.GetSpriteDef(spriteName);

                foreach (var frameGroup in spriteFrames.FrameGroups ?? new FrameGroup[0]) {
                    // Attempt to load the spritesheet referenced by the spritesheetDef.
                    // Don't bothe if the def couldn't be found.
                    var spritesheetKey = Spritesheet.DimensionsToKey(frameWidth, frameHeight);
                    var spritesheetImageKey = $"{spriteName} ({spritesheetKey})";
                    var spritesheetDef = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
                    var spriteFrameGroupDef = (spritesheetDef?.FrameGroupsByName?.TryGetValue(frameGroup.Name, out var spriteFrameGroupOut) == true) ? spriteFrameGroupOut : null;

                    if (spritesheetDef != null && !_spritesheetImageDict.ContainsKey(spritesheetImageKey)) {
                        Bitmap bitmap = null;
                        try {
                            var bitmapPath = SpriteUtils.SpritesheetImagePath($"{SpriteUtils.FilesystemName(spritesheetImageKey)}.png");
                            bitmap = (Bitmap) Image.FromFile(bitmapPath);
                        }
                        catch { }
                        _spritesheetImageDict.Add(spritesheetImageKey, bitmap);
                    }

                    var frames = frameGroup.Frames
                        ?? CHR_Utils.GetCHR_FrameGroupDirections(sprite.Directions)
                            .Select(x => new Frame() { Direction = x })
                            .ToArray();

                    foreach (var frame in frames) {
                        var spriteFrameDef = (spriteFrameGroupDef?.Frames?.TryGetValue(frame.Direction, out var spriteFrameOut) == true) ? spriteFrameOut : null;
                        var aniFrameKey = $"{spritesheetImageKey} {frameGroup.Name} ({frame.Direction})";
                        var frameKey = $"{spritesheetImageKey} {aniFrameKey}" + (frame.DuplicateKey == null ? "" : $" ({frame.DuplicateKey})");

                        // Add a reference to the image whether the spritesheet resources were found or not.
                        // If they're invalid, simply display a red image.
                        if (!_spritesheetImageDefsByFrameKey.ContainsKey(frameKey)) {
                            _spritesheetImageDefsByFrameKey.Add(frameKey, new CHR_CompilationUnit.SpritesheetImageDef() {
                                SpritesheetBitmap = _spritesheetImageDict.TryGetValue(spritesheetImageKey, out var bmpOut) ? bmpOut : null,
                                X = spriteFrameDef?.SpritesheetX ?? -1,
                                Y = spriteFrameDef?.SpritesheetY ?? -1,
                                Width  = frameWidth,
                                Height = frameHeight
                            });
                        }

                        _framesToWriteBySpriteIndex[spriteIndex].Add((FrameKey: frameKey, AniFrameKey: aniFrameKey));
                        lastFrameKey = frameKey;
                    }
                }
            }

            // Track the final frame that needs to be written. There will need to be some padding after it.
            if (lastFrameKey != null)
                _finalSpriteFrames.Add(lastFrameKey);
        }

        private void WriteFrameTable(int spriteIndex, CHR_Writer chrWriter) {
            foreach (var frame in _framesToWriteBySpriteIndex[spriteIndex])
                chrWriter.WriteFrameTableFrame(spriteIndex, frame.FrameKey, frame.AniFrameKey);
            chrWriter.WriteFrameTableTerminator(spriteIndex);
        }

        private void WriteFrameTables(CHR_Writer chrWriter) {
            for (int i = 0; i < _chrDef.Sprites.Length; i++)
                WriteFrameTable(i, chrWriter);
        }

        private void WriteFrameImages(CHR_Writer chrWriter) {
            // Write all images, updating pointers that reference each image by its key.
            foreach (var imageRefKv in _spritesheetImageDefsByFrameKey.Where(x => !_frameImagesWritten.Contains(x.Key)).ToArray()) {
                var imageKey = imageRefKv.Key;
                var imageRef = imageRefKv.Value;

                ushort[] data = null;

                var bitmap = imageRef.SpritesheetBitmap;
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
                if (_finalSpriteFrames.Contains(imageKey))
                    chrWriter.WriteToAlignTo(4);

                // Don't write this frame to the CHR file again.
                _frameImagesWritten.Add(imageKey);
            }
        }

        // CHR_Def to be written
        private readonly CHR_Def _chrDef;

        // Spritesheet bitmaps loaded
        private readonly Dictionary<string, Bitmap> _spritesheetImageDict = new Dictionary<string, Bitmap>();

        // Used to identify images for individual frames in a spritesheet
        private class SpritesheetImageDef {
            public Bitmap SpritesheetBitmap;
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        // Set of spritesheet frame images to retrieve by their frame key
        private readonly Dictionary<string, SpritesheetImageDef> _spritesheetImageDefsByFrameKey = new Dictionary<string, SpritesheetImageDef>();

        // List of frames to write for each sprite, with their frame key (identifying the actual image) and their optional animation frame key (for specifying what is not a duplicate)
        private readonly Dictionary<int, List<(string FrameKey, string AniFrameKey)>> _framesToWriteBySpriteIndex = new Dictionary<int, List<(string FrameKey, string AniFrameKey)>>();

        // CHR files must have had their compressed frames written by sprite, because there's a forced alignment of 4
        // after every sprite's groups of frames. We don't bother to do that -- different sprites can share compressed
        // images -- but most cases are fixed if we record which image is the final image for each sprite.
        private readonly HashSet<string> _finalSpriteFrames = new HashSet<string>();

        // Keep track of frame images already written if we're written frame images sprite-by-sprite.
        private readonly HashSet<string> _frameImagesWritten = new HashSet<string>();
    }
}
