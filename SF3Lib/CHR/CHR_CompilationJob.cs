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
    /// <summary>
    /// A compilation unit/context for a single CHR_Def.
    /// </summary>
    public class CHR_CompilationJob {
        /// <summary>
        /// Begins a new sprite. Animations and frames and be added with AddFrames() and AddAnimations().
        /// FinishSprite() must be called when frames and animations are complete.
        /// </summary>
        /// <param name="spriteDef">The sprite with the header entry information.</param>
        public void StartSprite(SpriteDef spriteDef) {
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

        /// <summary>
        /// Begins a new sprite. Animations and frames and be added with AddFrames() and AddAnimations().
        /// FinishSprite() must be called when frames and animations are complete.
        /// </summary>
        /// <param name="spritesheet">The spritesheet which contains the frames and animations for this sprite.</param>
        /// <param name="frameWidth">The width of the frames in the spritesheet.</param>
        /// <param name="frameHeight">The height of the frames in the spritesheet.</param>
        /// <param name="directions">The number of directions for this sprite.</param>
        /// <param name="promotionLevel">The promotion level of the sprite, if it needs to be disambiguated for a player character. Usually 0.</param>
        public void StartSprite(Spritesheet spritesheet, int frameWidth, int frameHeight, SpriteDirectionCountType directions, int promotionLevel) {
            StartSprite(new SpriteHeaderEntry() {
                SpriteID       = spritesheet.SpriteID,
                Width          = frameWidth,
                Height         = frameHeight,
                Directions     = directions,
                VerticalOffset = spritesheet.VerticalOffset,
                Unknown0x08    = spritesheet.Unknown0x08,
                CollisionSize  = spritesheet.CollisionSize,
                PromotionLevel = promotionLevel,
                Scale          = spritesheet.Scale,
            });
        }

        /// <summary>
        /// Begins a new sprite. Animations and frames and be added with AddFrames() and AddAnimations().
        /// FinishSprite() must be called when frames and animations are complete.
        /// </summary>
        /// <param name="header">Header information for the sprite.</param>
        public void StartSprite(SpriteHeaderEntry header) {
            if (_currentSpriteIndex < _spriteCount)
                FinishSprite();

            var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
            spriteInfo.Header = header;

            _spriteCount = _currentSpriteIndex + 1;
            
        }

        /// <summary>
        /// Finishes the last sprite being built.
        /// </summary>
        public void FinishSprite() {
            if (_currentSpriteIndex < _spriteCount)
                _currentSpriteIndex = _spriteCount;
        }

        /// <summary>
        /// Adds an entire sprite entire to the CHR to be compiled.
        /// A new sprite is automatically started and finished via StartSprite() and FinishSprite().
        /// </summary>
        /// <param name="spriteDef">The CHR SpriteDef to add, in its entirety.</param>
        public void AddCompleteSprite(SpriteDef spriteDef) {
            StartSprite(spriteDef);
            AddFrames(spriteDef);
            AddAnimations(spriteDef);
            FinishSprite();
        }

        /// <summary>
        /// Adds frames from a CHR SpriteDef.
        /// </summary>
        /// <param name="sprite">The CHR SpriteDef that contains the frames to be added.</param>
        public void AddFrames(SpriteDef sprite) {
            foreach (var spriteFrames in sprite.FrameGroupsForSpritesheets ?? new FrameGroupsForSpritesheet[0]) {
                if (spriteFrames.FrameGroups == null)
                    continue;

                var spriteName  = spriteFrames.SpriteName ?? sprite.SpriteName;
                var frameWidth  = spriteFrames.Width      ?? sprite.Width;
                var frameHeight = spriteFrames.Height     ?? sprite.Height;

                foreach (var frameGroup in spriteFrames.FrameGroups)
                    AddFrames(frameGroup, spriteName, frameWidth, frameHeight);
            }
        }

        /// <summary>
        /// Adds frames from a CHR FrameGroup, which is contained in a FrameGroupForSpritesheet.
        /// </summary>
        /// <param name="frameGroup">The FrameGroup to add.</param>
        /// <param name="spriteName">The name of the sprite to which 'frameGroup' belongs.</param>
        /// <param name="frameWidth">The width of the frames in 'frameGroup'.</param>
        /// <param name="frameHeight">The height of the frames in 'frameGroup'.</param>
        public void AddFrames(FrameGroup frameGroup, string spriteName, int frameWidth, int frameHeight) {
            var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
            var spriteDef = SpriteUtils.GetSpriteDef(spriteName);

            // Attempt to load the spritesheet referenced by the spritesheetDef.
            // Don't bother if the def couldn't be found.
            var spritesheetKey = Spritesheet.DimensionsToKey(frameWidth, frameHeight);
            var spritesheet    = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;

            var frames = frameGroup.GetFrames(spriteInfo.Header.Directions);
            foreach (var frame in frames) 
                AddFrame(spritesheet, spriteName, frameWidth, frameHeight, frameGroup.Name, frame.Direction, frame.DuplicateKey);
        }

        /// <summary>
        /// Add an individual frame from a Spritesheet to the current sprite.
        /// </summary>
        /// <param name="spritesheet">Spritesheet which contains the frame.</param>
        /// <param name="frameWidth">Width of frames in the spritesheet.</param>
        /// <param name="frameHeight">Height of frames in the spritesheet.</param>
        /// <param name="spriteName">Name of the sprite which contains the Spritesheet.</param>
        /// <param name="frameGroupName">Name of the frame group which contains the sprite to add.</param>
        /// <param name="direction">Direction of the frame to add from the frame group.</param>
        /// <param name="duplicateKey">Optional key appended to the 'frameKey', which forces duplicate frames to be considered separate.</param>
        public void AddFrame(Spritesheet spritesheet, string spriteName, int frameWidth, int frameHeight, string frameGroupName, SpriteFrameDirection direction, string duplicateKey) {
            var spritesheetKey      = Spritesheet.DimensionsToKey(frameWidth, frameHeight);
            var spritesheetImageKey = $"{spriteName} ({spritesheetKey})";
            var spriteFrameGroup    = (spritesheet?.FrameGroupsByName?.TryGetValue(frameGroupName, out var spriteFrameGroupOut) == true) ? spriteFrameGroupOut : null;
            var spriteFrameDef      = (spriteFrameGroup?.Frames?.TryGetValue(direction, out var spriteFrameOut) == true) ? spriteFrameOut : null;
            var aniFrameKey         = $"{spritesheetImageKey} {frameGroupName} ({direction})";
            var frameKey            = $"{spritesheetImageKey} | {aniFrameKey}" + (duplicateKey == null ? "" : $" ({duplicateKey})");

            // Load the spritesheet if it wasn't loaded already.
            var spritesheetBitmap = GetSpritesheetBitmap(spriteName, frameWidth, frameHeight);

            AddFrame(spritesheetBitmap, frameWidth, frameHeight, spriteFrameDef?.SpritesheetX ?? -1, spriteFrameDef?.SpritesheetY ?? -1, frameKey, aniFrameKey);
        }

        /// <summary>
        /// Adds an individual frame contained within a bitmap to the current sprite.
        /// </summary>
        /// <param name="spritesheetBitmap">Spritesheet image to use.</param>
        /// <param name="frameWidth">Width of frames in the spritesheet.</param>
        /// <param name="frameHeight">Height of frames in the spritesheet.</param>
        /// <param name="frameX">Top-left X coordinate of the frame image in the spritesheet image.</param>
        /// <param name="frameY">Top-left Y coordinate of the frame image in the spritesheet image.</param>
        /// <param name="frameKey">Identifier for this frame for the purpose of eliminating duplicates.</param>
        /// <param name="aniFrameKey">Identifier for this frame for the purpose of matching specific frames in animations.</param>
        public void AddFrame(Bitmap spritesheetBitmap, int frameWidth, int frameHeight, int frameX, int frameY, string frameKey, string aniFrameKey) {
            // Track the frames we're adding to the entire CHR.
            if (!_spritesheetFramesByFrameKey.ContainsKey(frameKey)) {
                // Add a reference to the image whether the spritesheet resources were found or not.
                // If they're invalid, simply display a red image.
                _spritesheetFramesByFrameKey.Add(frameKey, new SpritesheetFrame() {
                    SpritesheetBitmap = spritesheetBitmap,
                    X      = frameX,
                    Y      = frameY,
                    Width  = frameWidth,
                    Height = frameHeight,
                    FirstSeenSpriteIndex = _currentSpriteIndex,
                });
            }

            var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
            spriteInfo.Frames.Add(new FrameInfo() { FrameKey = frameKey, AniFrameKey = aniFrameKey });
        }

        /// <summary>
        /// Adds an individual frame to the current sprite.
        /// </summary>
        /// <param name="frameImage">Image to use for the frame.</param>
        /// <param name="frameKey">Identifier for this frame for the purpose of eliminating duplicates.</param>
        /// <param name="aniFrameKey">Identifier for this frame for the purpose of matching specific frames in animations.</param>
        public void AddFrame(Bitmap frameImage, string frameKey, string aniFrameKey) {
            // Track the frames we're adding to the entire CHR.
            if (!_spritesheetFramesByFrameKey.ContainsKey(frameKey)) {
                // Add a reference to the image whether the spritesheet resources were found or not.
                // If they're invalid, simply display a red image.
                _spritesheetFramesByFrameKey.Add(frameKey, new SpritesheetFrame() {
                    SpritesheetBitmap = frameImage,
                    X      = 0,
                    Y      = 0,
                    Width  = frameImage.Width,
                    Height = frameImage.Height,
                    FirstSeenSpriteIndex = _currentSpriteIndex,
                });
            }

            var spriteInfo = GetSpriteInfo(_currentSpriteIndex);
            spriteInfo.Frames.Add(new FrameInfo() { FrameKey = frameKey, AniFrameKey = aniFrameKey });
        }

        /// <summary>
        /// Adds animations from a CHR SpriteDef.
        /// </summary>
        /// <param name="sprite">The CHR SpriteDef that contains the animations to be added.</param>
        public void AddAnimations(SpriteDef sprite) {
            if (sprite.AnimationsForSpritesheetAndDirections == null)
                return;

            // Write all animations for each particular sprite with a set of directions.
            foreach (var animations in sprite.AnimationsForSpritesheetAndDirections)
                AddAnimations(animations, sprite.SpriteName, sprite.Width, sprite.Height, sprite.Directions);
        }

        /// <summary>
        /// Adds animations from a specific set by spritesheet and directions in a CHR.
        /// </summary>
        /// <param name="animations">The set of animations for a specific spritesheet and directions.</param>
        /// <param name="defaultSpriteName">The name of the sprite if 'animations' does not specify one.</param>
        /// <param name="defaultWidth">The width of the sprite if not specified by 'animations'.</param>
        /// <param name="defaultHeight">The height of the sprite if not specified by 'animations'.</param>
        /// <param name="defaultDirections">The directions of the sprite if not specified by 'animations'.</param>
        public void AddAnimations(AnimationsForSpritesheetAndDirection animations, string defaultSpriteName, int defaultWidth, int defaultHeight, SpriteDirectionCountType defaultDirections) {
            ForEachAnimation(
                animations, defaultSpriteName, defaultWidth, defaultHeight, defaultDirections,
                (animation, animationName, spriteName, frameWidth, frameHeight, directions) => AddAnimation(animation, animationName, spriteName, frameWidth, frameHeight, directions)
            );
        }

        /// <summary>
        /// Adds any frames in from a set of animations that are missing in the FrameTable.
        /// </summary>
        /// <param name="animations">The set of animations for a specific spritesheet and directions.</param>
        /// <param name="defaultSpriteName">The name of the sprite if 'animations' does not specify one.</param>
        /// <param name="defaultWidth">The width of the sprite if not specified by 'animations'.</param>
        /// <param name="defaultHeight">The height of the sprite if not specified by 'animations'.</param>
        /// <param name="defaultDirections">The directions of the sprite if not specified by 'animations'.</param>
        public void AddMissingFrames(AnimationsForSpritesheetAndDirection animations, string defaultSpriteName, int defaultWidth, int defaultHeight, SpriteDirectionCountType defaultDirections) {
            ForEachAnimation(
                animations, defaultSpriteName, defaultWidth, defaultHeight, defaultDirections,
                (animation, animationName, spriteName, frameWidth, frameHeight, directions) => AddMissingFrames(animation, animationName, spriteName, frameWidth, frameHeight, directions)
            );
        }

        public void ForEachAnimation(AnimationsForSpritesheetAndDirection animations, string defaultSpriteName, int defaultWidth, int defaultHeight, SpriteDirectionCountType defaultDirections,
            Action<Animation, string, string, int, int, SpriteDirectionCountType> action
        ) {
            if (animations.Animations == null)
                return;

            var spriteName         = animations.SpriteName ?? defaultSpriteName;
            var directions         = animations.Directions ?? defaultDirections;
            var frameWidth         = animations.Width ?? defaultWidth;
            var frameHeight        = animations.Height ?? defaultHeight;
            var spritesheetKey     = Spritesheet.DimensionsToKey(frameWidth, frameHeight);

            var spriteDef          = SpriteUtils.GetSpriteDef(spriteName);
            var spritesheetDef     = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
            var spriteAnimationSet = (spritesheetDef?.AnimationSetsByDirections?.TryGetValue(directions, out var sadOut) == true) ? sadOut : null;

            // Write all individual animations.
            foreach (var animationName in animations.Animations) {
                var spriteAnimation = (animationName != null && spriteAnimationSet?.AnimationsByName?.TryGetValue(animationName, out var animOut) == true) ? animOut : null;
                action(spriteAnimation, animationName, spriteName, frameWidth, frameHeight, directions);
            }
        }

        /// <summary>
        /// Adds an animation from a sprite, automatically matching frames.
        /// </summary>
        /// <param name="animation">Animation definition in the SpriteDef.</param>
        /// <param name="animationName">Name of the animation.</param>
        /// <param name="spriteName">The name of the sprite to which 'animation' belongs.</param>
        /// <param name="frameWidth">The width of frames in the spritesheet.</param>
        /// <param name="frameHeight">The height of frames in the spritesheet.</param>
        /// <param name="directions">The initial number of directional frames used for 'Frame' commands.</param>
        public void AddAnimation(Animation animation, string animationName, string spriteName, int frameWidth, int frameHeight, SpriteDirectionCountType directions) {
            var aniFrameKeyPrefix  = $"{spriteName} ({Spritesheet.DimensionsToKey(frameWidth, frameHeight)})";
            var spriteInfo         = GetSpriteInfo(_currentSpriteIndex);
            var spriteAniFrameKeys = spriteInfo.Frames.Select(x => x.AniFrameKey).ToArray();
            var frameDirections    = CHR_Utils.GetCHR_FrameGroupDirections(directions);

            // Write an empty animation entry for deliberately null animations, missing animations, or empty animations.
            if (animation == null || animation.AnimationCommands == null) {
                spriteInfo.CurrentAnimationIndex++;
                return;
            }

            // Add all commands for the sprite.
            foreach (var aniCommand in animation.AnimationCommands) {
                var command = (int) aniCommand.Command;

                // The 'SetDirectionCount' command (0xF1) updates the number of frames in our key from now on.
                if (aniCommand.Command == SpriteAnimationCommandType.SetDirectionCount)
                    frameDirections = CHR_Utils.GetCHR_FrameGroupDirections((SpriteDirectionCountType) aniCommand.Parameter);
                // If this is a frame, we need to generate a key that will be used to locate a FrameID later.
                else if (aniCommand.Command == SpriteAnimationCommandType.Frame) {
                    // Get the FrameKey and AniFrameKey for each direction in this animation frame command.
                    var frameInfos = GetAnimationFrameCommandFrameInfos(aniCommand, frameDirections, aniFrameKeyPrefix);
                    if (frameInfos != null) {
                        var frameId = spriteAniFrameKeys.GetFirstIndexOf(frameInfos.Select(x => x?.AniFrameKey).ToArray(), allowExceedingSize: true);
                        if (frameId >= 0)
                            command = frameId;
                        else
                            ; // TODO: what to do here???
                    }
                    else
                        ; // TODO: what to do here???
                }

                // Add the command to the command table.
                if (!spriteInfo.AnimationsByIndex.ContainsKey(spriteInfo.CurrentAnimationIndex))
                    spriteInfo.AnimationsByIndex.Add(spriteInfo.CurrentAnimationIndex, new AnimationInfo());
                spriteInfo.AnimationsByIndex[spriteInfo.CurrentAnimationIndex].Commands.Add(new AnimationCommandInfo() {
                    Command = command,
                    Parameter = aniCommand.Parameter
                });
            }

            spriteInfo.CurrentAnimationIndex++;
        }

        /// <summary>
        /// Adds any frames in an Animation that are missing in the FrameTable.
        /// </summary>
        /// <param name="animation">Animation definition in the SpriteDef.</param>
        /// <param name="animationName">Name of the animation.</param>
        /// <param name="spriteName">The name of the sprite to which 'animation' belongs.</param>
        /// <param name="frameWidth">The width of frames in the spritesheet.</param>
        /// <param name="frameHeight">The height of frames in the spritesheet.</param>
        /// <param name="directions">The initial number of directional frames used for 'Frame' commands.</param>
        public void AddMissingFrames(Animation animation, string animationName, string spriteName, int frameWidth, int frameHeight, SpriteDirectionCountType directions) {
            if (animation == null)
                return;

            // There's a bug that appears in sprites with multiple missing animation frames: Unless the frames are added
            // in order of least missing frames to most missing frames, the frames will be added multiple times, and the
            // missing frames will suddenly be filled. This bug triggers with Rainblood Rook (Capeless), whose animation
            // isn't properly identified because the frames that *should* be missing *aren't*.

            var aniFrameKeyPrefix   = $"{spriteName} ({Spritesheet.DimensionsToKey(frameWidth, frameHeight)})";
            var spriteInfo          = GetSpriteInfo(_currentSpriteIndex);
            var spriteAniFrameKeys  = spriteInfo.Frames.Select(x => x.AniFrameKey).ToArray();
            var frameDirections     = CHR_Utils.GetCHR_FrameGroupDirections(directions);

            // Only loaded if needed for adding frames.
            Sprites.SpriteDef spriteDef = null;
            Spritesheet spritesheet     = null;
            string spritesheetKey       = null;
            string spritesheetImageKey  = null;
            Bitmap spritesheetImage     = null;

            // Add all commands for the sprite.
            foreach (var aniCommand in animation.AnimationCommands) {
                // The 'SetDirectionCount' command (0xF1) updates the number of frames in our key from now on.
                if (aniCommand.Command == SpriteAnimationCommandType.SetDirectionCount)
                    frameDirections = CHR_Utils.GetCHR_FrameGroupDirections((SpriteDirectionCountType) aniCommand.Parameter);

                // The only other command we caer about in this loop is for frames.
                if (aniCommand.Command != SpriteAnimationCommandType.Frame)
                    continue;

                // Get the FrameKey and AniFrameKey for each direction in this animation frame command.
                var aniFrameGroupsAndKeys = GetAnimationFrameCommandFrameInfos(aniCommand, frameDirections, aniFrameKeyPrefix);
                if (aniFrameGroupsAndKeys == null)
                    continue;

                // If the frames could be found, there's nothing to add.
                if (spriteAniFrameKeys.GetFirstIndexOf(aniFrameGroupsAndKeys.Select(x => x.AniFrameKey).ToArray(), allowExceedingSize: true) >= 0)
                    continue;

                // Load the spritesheet if it wasn't loaded before.
                if (spritesheetKey == null) {
                    spriteDef           = SpriteUtils.GetSpriteDef(spriteName);
                    spritesheetKey      = Spritesheet.DimensionsToKey(frameWidth, frameHeight);
                    spritesheet         = (spriteDef?.Spritesheets?.TryGetValue(spritesheetKey, out var spritesheetOut) == true) ? spritesheetOut : null;
                    spritesheetImageKey = $"{spriteName} ({spritesheetKey})";
                    spritesheetImage    = GetSpritesheetBitmap(spriteName, frameWidth, frameHeight);
                }

                // Add all frames required for this animation frame.
                for (int i = 0; i < frameDirections.Length; i++) {
                    var frameGroupName = aniFrameGroupsAndKeys[i].FrameGroup;
                    var aniFrameKey    = aniFrameGroupsAndKeys[i].AniFrameKey;

                    // Skip missing frames.
                    if (frameGroupName == null || aniFrameKey == null)
                        continue;

                    var frameKey   = $"{spritesheetImageKey} | {aniFrameKey}";
                    var frameGroup = (spritesheet?.FrameGroupsByName?.TryGetValue(frameGroupName, out var frameGroupOut) == true) ? frameGroupOut : null;
                    var frame      = (frameGroup?.Frames?.TryGetValue(frameDirections[i], out var frameOut) == true) ? frameOut : null;

                    AddFrame(spritesheetImage, frameWidth, frameHeight, frame?.SpritesheetX ?? -1, frame?.SpritesheetY ?? -1, frameKey, aniFrameKey);
                }

                // Update 'spriteAniFrameKeys' with the new frames.
                spriteAniFrameKeys = spriteInfo.Frames.Select(x => x.AniFrameKey).ToArray();
            }
        }

        private class AnimationFrameCommandFrameInfo {
            public string FrameGroup;
            public string AniFrameKey;
        }

        private AnimationFrameCommandFrameInfo[] GetAnimationFrameCommandFrameInfos(AnimationCommand aniCommand, SpriteFrameDirection[] frameDirections, string aniFrameKeyPrefix) {
            if (aniCommand.FrameGroup != null) {
                return frameDirections
                    .Select(x => new AnimationFrameCommandFrameInfo() {
                        FrameGroup  = aniCommand.FrameGroup,
                        AniFrameKey = $"{aniFrameKeyPrefix} {aniCommand.FrameGroup} ({x.ToString()})"
                    })
                    .ToArray();
            }
            // The 'Frames' command has manually specified FrameGroup + Direction pairs.
            else if (aniCommand.FramesByDirection != null) {
                return frameDirections
                    .ToDictionary(x => x, x => aniCommand.FramesByDirection.TryGetValue(x, out var f) ? f : null)
                    .Select(x => (x.Value == null) ? null : new AnimationFrameCommandFrameInfo() {
                        FrameGroup  = x.Value.FrameGroup,
                        AniFrameKey = $"{aniFrameKeyPrefix} {x.Value.FrameGroup} ({x.Value.Direction.ToString()})"
                    })
                    .ToArray();
            }

            // If nothing is there, something is wrong.
            return null;
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

        private SpriteInfo GetSpriteInfo(int spriteIndex) {
            if (_spriteInfoBySpriteIndex.TryGetValue(spriteIndex, out var info))
                return info;
            return _spriteInfoBySpriteIndex[spriteIndex] = new SpriteInfo();
        }

        private Bitmap GetSpritesheetBitmap(string spriteName, int width, int height) {
            var spritesheetKey = Spritesheet.DimensionsToKey(width, height);
            var spritesheetImageKey = $"{spriteName} ({spritesheetKey})";

            if (_spritesheetImageDict.TryGetValue(spritesheetImageKey, out var bitmap))
                return bitmap;

            try {
                var bitmapPath = SpriteUtils.SpritesheetImagePath($"{SpriteUtils.FilesystemName(spritesheetImageKey)}.png");
                bitmap = (Bitmap) Image.FromFile(bitmapPath);
            }
            catch {
                // TODO: log an error
                bitmap = null;
            }

            _spritesheetImageDict.Add(spritesheetImageKey, bitmap);
            return bitmap;
        }

        // The current sprite being built
        private int _currentSpriteIndex = 0;

        // The total number of sprites built (or at least started)
        private int _spriteCount = 0;

        // Basic container for information that will need to be written via CHR_Writer.WriteHeaderEntry()
        public class SpriteHeaderEntry {
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
            public int CurrentAnimationIndex = 0;
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
