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
    public class CHR_CompilationJob {
        /// <summary>
        /// Creates a compilation unit/context for a single CHR_Def.
        /// </summary>
        /// <param name="chrDef">The CHR_Def to be compiled.</param>
        public CHR_CompilationJob(CHR_Def chrDef) {
            if (chrDef.Sprites != null) {
                foreach (var sprite in chrDef.Sprites) {
                    StartSprite(sprite);
                    AddFrames(sprite);
                    AddAnimations(sprite);
                    FinishSprite();
                }
            }
        }

        /// <summary>
        /// Writes the CHR_Def to an output stream.
        /// </summary>
        /// <param name="outputStream">The stream to write the CHR_Def to.</param>
        /// <returns>The number of bytes written to 'outputStream'.</returns>
        public int Write(Stream outputStream, bool writeFrameImagesBeforeTables, byte[] junkAfterFrameTables) {
            // If the output stream can't seek, we're need to write to an intermediate in-memory stream.
            var targetOutputStream = outputStream.CanSeek ? outputStream : new MemoryStream();
            var chrWriter = new CHR_Writer(targetOutputStream);

            // Write data.
            WriteHeader(chrWriter);
            WriteAnimations(chrWriter);
            WriteFrames(chrWriter, writeFrameImagesBeforeTables, junkAfterFrameTables);
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
            for (var spriteIndex = 0; spriteIndex < _spriteCount; spriteIndex++) {
                var spriteInfo = GetSpriteInfo(spriteIndex);
                var header = spriteInfo.Header;

                chrWriter.WriteHeaderEntry(
                    spriteIndex,
                    (ushort) header.SpriteID,
                    (ushort) header.Width,
                    (ushort) header.Height,
                    (byte) header.Directions,
                    (byte) header.VerticalOffset,
                    (byte) header.Unknown0x08,
                    (byte) header.CollisionSize,
                    (byte) header.PromotionLevel,
                    (int) Math.Round(header.Scale * 65536.0f)
                );
            }
            chrWriter.WriteHeaderTerminator();
        }

        private void WriteAnimations(CHR_Writer chrWriter) {
            for (int spriteIndex = 0; spriteIndex < _spriteCount; spriteIndex++) {
                var spriteInfo = GetSpriteInfo(spriteIndex);

                foreach (var animationKv in spriteInfo.AnimationsByIndex) {
                    var aniIndex = animationKv.Key;
                    var animation = animationKv.Value;

                    chrWriter.StartAnimationCommandTable(spriteIndex, aniIndex);
                    foreach (var cmd in animation.Commands)
                        chrWriter.WriteAnimationCommand(cmd.Command, cmd.Parameter);
                }

                chrWriter.WriteAnimationTable(spriteIndex);
            }
        }

        private void AddAnimations(SpriteDef sprite) {
            // Write all individual animations.
            var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
            var spriteFrameKeys = spriteInfo.Frames.Select(x => x.AniFrameKey).ToArray();

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

                            // Figure out the command. For frames, figure out the FrameID.
                            var command = (int) aniCommand.Command;
                            if (aniCommand.Command == SpriteAnimationCommandType.Frame) {
                                var frameId = spriteFrameKeys.GetFirstIndexOf(frameKeys, allowExceedingSize: true);
                                if (frameId >= 0)
                                    command = frameId;
                                else
                                    // TODO: what to do here???
                                    ;
                            }

                            if (!spriteInfo.AnimationsByIndex.ContainsKey(animationIndex))
                                spriteInfo.AnimationsByIndex.Add(animationIndex, new AnimationInfo());
                            spriteInfo.AnimationsByIndex[animationIndex].Commands.Add(new AnimationCommandInfo() {
                                Command = command,
                                Parameter = aniCommand.Parameter
                            });
                        }
                    }

                    animationIndex++;
                }
            }
        }

        private void WriteFrames(CHR_Writer chrWriter, bool writeFrameImagesBeforeTables, byte[] junkAfterFrameTables) {
            // For what are likely older CHRs, images for each sprite are written before their own frame table.
            if (writeFrameImagesBeforeTables) {
                for (int i = 0; i < _spriteCount; i++) {
                    WriteFrameImages(i, chrWriter);
                    WriteFrameTable(i, chrWriter);
                }
            }
            // 99% of the time, the images are written after the frame tables.
            else {
                WriteFrameTables(chrWriter);

                // XBTL127.CHR has junk data after the frame table. Write it here.
                if (junkAfterFrameTables != null)
                    chrWriter.Write(junkAfterFrameTables);

                WriteFrameImages(chrWriter);
            }
        }

        private void StartSprite(SpriteDef spriteDef) {
            StartSprite(new SpriteHeaderEntry() {
                SpriteID       = spriteDef.SpriteID,
                Width          = spriteDef.Width,
                Height         = spriteDef.Height,
                Directions     = spriteDef.Directions,
                VerticalOffset = spriteDef.VerticalOffset,
                Unknown0x08    = spriteDef.Unknown0x08,
                CollisionSize  = spriteDef.CollisionSize,
                PromotionLevel = spriteDef.PromotionLevel,
                Scale          = spriteDef.Scale,
            });
        }

        private void StartSprite(SpriteHeaderEntry header) {
            if (_currentSpriteIndex < _spriteCount)
                FinishSprite();

            var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
            spriteInfo.Header = header;

            _spriteCount = _currentSpriteIndex + 1;
            
        }

        private void FinishSprite() {
            if (_currentSpriteIndex < _spriteCount)
                _currentSpriteIndex = _spriteCount;
        }

        private void AddFrames(SpriteDef sprite) {
            foreach (var spriteFrames in sprite.FrameGroupsForSpritesheets ?? new FrameGroupsForSpritesheet[0]) {
                if (spriteFrames.FrameGroups == null)
                    continue;

                var spriteName  = spriteFrames.SpriteName ?? sprite.SpriteName;
                var frameWidth  = spriteFrames.Width      ?? sprite.Width;
                var frameHeight = spriteFrames.Height     ?? sprite.Height;

                foreach (var frameGroup in spriteFrames.FrameGroups)
                    AddFrames(frameGroup, spriteName, frameWidth, frameHeight, sprite.Directions);
            }
        }

        private void AddFrames(FrameGroup frameGroup, string spriteName, int frameWidth, int frameHeight, SpriteDirectionCountType spriteDirections) {
            var spriteDef = SpriteUtils.GetSpriteDef(spriteName);

            // Attempt to load the spritesheet referenced by the spritesheetDef.
            // Don't bothe if the def couldn't be found.
            var spritesheetKey      = Spritesheet.DimensionsToKey(frameWidth, frameHeight);
            var spritesheetImageKey = $"{spriteName} ({spritesheetKey})";
            var spritesheetDef      = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
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
                ?? CHR_Utils.GetCHR_FrameGroupDirections(spriteDirections)
                    .Select(x => new Frame() { Direction = x })
                    .ToArray();

            foreach (var frame in frames) {
                var spriteFrameDef = (spriteFrameGroupDef?.Frames?.TryGetValue(frame.Direction, out var spriteFrameOut) == true) ? spriteFrameOut : null;
                var aniFrameKey    = $"{spritesheetImageKey} {frameGroup.Name} ({frame.Direction})";
                var frameKey       = $"{spritesheetImageKey} {aniFrameKey}" + (frame.DuplicateKey == null ? "" : $" ({frame.DuplicateKey})");

                // Add a reference to the image whether the spritesheet resources were found or not.
                // If they're invalid, simply display a red image.
                if (!_spritesheetFramesByFrameKey.ContainsKey(frameKey)) {
                    _spritesheetFramesByFrameKey.Add(frameKey, new SpritesheetFrame() {
                        SpritesheetBitmap = _spritesheetImageDict.TryGetValue(spritesheetImageKey, out var bmpOut) ? bmpOut : null,
                        X      = spriteFrameDef?.SpritesheetX ?? -1,
                        Y      = spriteFrameDef?.SpritesheetY ?? -1,
                        Width  = frameWidth,
                        Height = frameHeight,
                        FirstSeenSpriteIndex = _currentSpriteIndex,
                    });
                }

                var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
                spriteInfo.Frames.Add(new FrameInfo() { FrameKey = frameKey, AniFrameKey = aniFrameKey });
            }
        }

        private void WriteFrameTable(int spriteIndex, CHR_Writer chrWriter) {
            var spriteInfo = GetSpriteInfo(spriteIndex);
            foreach (var frame in spriteInfo.Frames)
                chrWriter.WriteFrameTableFrame(spriteIndex, frame.FrameKey);
            chrWriter.WriteFrameTableTerminator(spriteIndex);
        }

        private void WriteFrameTables(CHR_Writer chrWriter) {
            for (int i = 0; i < _spriteCount; i++)
                WriteFrameTable(i, chrWriter);
        }

        private void WriteFrameImages(CHR_Writer chrWriter) {
            for (int i = 0; i < _spriteCount; i++)
                WriteFrameImages(i, chrWriter);
        }

        private void WriteFrameImages(int spriteIndex, CHR_Writer chrWriter) {
            var spritesheetFrames = _spritesheetFramesByFrameKey
                .Where(x => !_frameImagesWritten.Contains(x.Key))
                .Where(x => x.Value.FirstSeenSpriteIndex == spriteIndex)
                .ToArray();

            // Write all images, updating pointers that reference each image by its key.
            foreach (var spritesheetFrameKv in spritesheetFrames) {
                var frameKey = spritesheetFrameKv.Key;
                var spritesheetFrame = spritesheetFrameKv.Value;

                ushort[] data = null;

                var bitmap = spritesheetFrame.SpritesheetBitmap;
                var x1 = spritesheetFrame.X;
                var y1 = spritesheetFrame.Y;
                var x2 = x1 + spritesheetFrame.Width;
                var y2 = y1 + spritesheetFrame.Height;

                // If the image is invalid, display a bright red square instead.
                if (bitmap == null || x1 < 0 || y1 < 0 || x2 > bitmap.Width || y2 > bitmap.Height) {
                    data = new ushort[spritesheetFrame.Width * spritesheetFrame.Height];
                    for (int i = 0; i < data.Length; i++)
                        data[i] = 0x801F;
                }
                // It looks like a valid image. Copy it into the data.
                else
                    data = bitmap.GetDataAt(x1, y1, spritesheetFrame.Width, spritesheetFrame.Height).To1DArrayTransposed();

                // Write the compressed image, updating any pointers that reference it by its key.
                chrWriter.WriteFrameImage(frameKey, Compression.CompressSpriteData(data, 0, data.Length));

                // Don't write this frame to the CHR file again.
                _frameImagesWritten.Add(frameKey);
            }

            // There's some padding after every sprite's frame images to enforce an alignment of 4.
            chrWriter.WriteToAlignTo(4);
        }

        private SpriteInfo GetSpriteInfo(int spriteIndex) {
            if (_spriteInfoBySpriteIndex.TryGetValue(spriteIndex, out var info))
                return info;
            return _spriteInfoBySpriteIndex[spriteIndex] = new SpriteInfo();
        }

        // The current sprite being built
        private int _currentSpriteIndex = 0;

        // The total number of sprites built (or at least started)
        private int _spriteCount = 0;

        // Basic container for information that will need to be written via CHR_Writer.WriteHeaderEntry()
        private class SpriteHeaderEntry {
            public int SpriteID;
            public int Width;
            public int Height;
            public SpriteDirectionCountType Directions;
            public int VerticalOffset;
            public int Unknown0x08;
            public int CollisionSize;
            public int PromotionLevel;
            public float Scale;
        };

        private class FrameInfo {
            public string FrameKey;
            public string AniFrameKey;
        };

        private class AnimationCommandInfo {
            public override string ToString() => $"{Command:X2},{Parameter}";

            public int Command;
            public int Parameter;
        };

        private class AnimationInfo {
            public List<AnimationCommandInfo> Commands = new List<AnimationCommandInfo>();
        };

        private class SpriteInfo {
            public SpriteHeaderEntry Header;
            public List<FrameInfo> Frames = new List<FrameInfo>();
            public Dictionary<int, AnimationInfo> AnimationsByIndex = new Dictionary<int, AnimationInfo>();
        };

        // Collection of all info for each sprite to be written
        private readonly Dictionary<int, SpriteInfo> _spriteInfoBySpriteIndex = new Dictionary<int, SpriteInfo>();

        // Spritesheet bitmaps loaded
        private readonly Dictionary<string, Bitmap> _spritesheetImageDict = new Dictionary<string, Bitmap>();

        // Used to identify images for individual frames in a spritesheet
        private class SpritesheetFrame {
            public Bitmap SpritesheetBitmap;
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public int FirstSeenSpriteIndex;
        }

        // Set of spritesheet frame images to retrieve by their frame key
        private readonly Dictionary<string, SpritesheetFrame> _spritesheetFramesByFrameKey = new Dictionary<string, SpritesheetFrame>();

        // Keep track of frame images already written if we're written frame images sprite-by-sprite.
        private readonly HashSet<string> _frameImagesWritten = new HashSet<string>();
    }
}
