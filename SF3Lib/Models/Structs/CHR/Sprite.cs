using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Tables.CHR;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class Sprite : Struct {
        public Sprite(IByteData data, int id, int idInGroup, string name, int address, uint dataOffset, INameGetterContext ngc)
        : base(data, id, name, address, 0x18 /* just the header */) {
            IDInGroup = idInGroup;
            DataOffset = dataOffset;
            NameGetterContext = ngc;

            Header = new SpriteHeader(data, 0, $"{nameof(Header)}{ID:D2}", address, dataOffset);

            // We're often reading invalid headers when looking at CHP files. If this doesn't look like a valid header, abort reading here.
            if (!Header.IsValid() || Header.SpriteID == 0xFFFF)
                return;

            FrameTable = FrameTable.Create(
                Data,
                $"Sprite{ID:D2}_Frames",
                (int) (DataOffset + Header.FrameTableOffset),
                DataOffset, Header.Width, Header.Height,
                $"Sprite{ID:D2}_",
                ID,
                Header.SpriteID,
                Header.Directions);

            // It seems that this CHR and *only* this CHR has a bigger animation table than the rest.
            var nextId = (uint) Data.GetWord(Address + 0x18);

            // Determine the size of the animation table, which isn't always 16 (in XOP101.CHR, it's 21).
            int nextAnimationTableOffset;
            if (nextId != 0xFFFF) {
                nextAnimationTableOffset = Data.GetDouble(Address + 0x18 + 0x14);
                var nextAnimationTableFirstAnimationOffset = Data.GetDouble((int) DataOffset + nextAnimationTableOffset);
                // TODO: This will break if the first animation has an offset of 0x00.
                if (nextAnimationTableFirstAnimationOffset != 0)
                    nextAnimationTableOffset = Math.Min(nextAnimationTableOffset, nextAnimationTableFirstAnimationOffset);
            }
            else {
                var firstHeaderAddr = Address - 0x18 * IDInGroup;
                var firstFrameTableOffset = Data.GetDouble(firstHeaderAddr + 0x10);
                var firstFrameOffset = Data.GetDouble(firstFrameTableOffset + (int) DataOffset);
                nextAnimationTableOffset = (firstFrameOffset == 0) ? firstFrameTableOffset : Math.Min(firstFrameTableOffset, firstFrameOffset);
            }
            var animationTableSize = Math.Max(16, (nextAnimationTableOffset - (int) Header.AnimationTableOffset) / 4);

            AnimationOffsetTable = AnimationOffsetTable.Create(Data, nameof(AnimationOffsetTable), (int) (DataOffset + Header.AnimationTableOffset), animationTableSize);

            var aniOffsets = AnimationOffsetTable.Select(x => x.Offset).Concat(new uint[] { Header.AnimationTableOffset }).ToArray();
            for (int i = aniOffsets.Length - 1; i >= 0; i--)
                if (aniOffsets[i] == 0)
                    aniOffsets[i] = aniOffsets[i + 1];

            AnimationCommandTablesByIndex = AnimationOffsetTable
                .Where(x => x.Offset != 0)
                .Select(x => AnimationCommandTable.Create(
                    Data,
                    $"Sprite{ID:D2}_Animation{x.ID:D2}",
                    (int) (DataOffset + x.Offset),
                    $"Sprite{ID:D2}_Animation{x.ID:D2}_",
                    ID,
                    Header.SpriteID,
                    (SpriteDirectionCountType) Header.Directions,
                    x.ID,
                    FrameTable,
                    (int) ((aniOffsets[x.ID + 1] - aniOffsets[x.ID]) / 4)))
                .ToDictionary(x => x.AnimationIndex, x => x);

            AnimationTable = AnimationTable.Create(Data, $"Sprite{ID:D2}_{nameof(AnimationTable)}", Header.Directions, AnimationCommandTablesByIndex.Values.ToArray(),
                FrameTable, $"Sprite{ID:D2}_");

            var spriteNames = AnimationTable.Select(x => x.SpriteName).Distinct().ToArray();
            foreach (var spriteName in spriteNames) {
                var infoName = $"{spriteName} ({Header.Width}x{Header.Height})";
                CHR_Utils.AddSpriteHeaderInfo(infoName, Header.SpriteID, Header.VerticalOffset, Header.Unknown0x08, Header.CollisionShadowDiameter, Header.Scale / 65536.0f);
            }

            // Set the name of this sprite to the most common SpriteName used in FrameTable.
            var spriteNameCounts = FrameTable
                .Select(x => x.SpriteName)
                .Distinct()
                .ToDictionary(x => x, x => AnimationTable.Count(y => y.SpriteName == x));
            var mostCommonSpriteName = spriteNameCounts.OrderByDescending(x => x.Value).FirstOrDefault().Key;
            var spriteIdPropInfo = typeof(SpriteHeader).GetProperty(nameof(Header.SpriteID));
            SpriteName = mostCommonSpriteName ?? NameGetterContext.GetName(Header, spriteIdPropInfo, Header.SpriteID, new object[] { NamedValueType.Sprite });

            DropdownName = Name + " - "
                + SpriteName
                + $" ({Header.Width}x{Header.Height}x{Header.Directions})"
                + (Header.PromotionLevel > 0 ? $" (P{Header.PromotionLevel})" : "")
                ;

            TotalCompressedFramesSize = (uint) FrameTable.Sum(x => x.TextureCompressedSize);

            // TODO: shouldn't be here! We need a separate view for just the sprite.
            Header.TotalCompressedFramesSize = TotalCompressedFramesSize;
        }

        public SpriteDef ToCHR_SpriteDef(HashSet<string> framesWithDuplicates) {
            return new SpriteDef() {
                SpriteName       = SpriteName,
                Width            = Header.Width,
                Height           = Header.Height,
                Directions       = (SpriteDirectionCountType) Header.Directions,
                PromotionLevel   = Header.PromotionLevel,

                SpriteID         = Header.SpriteID,
                VerticalOffset   = Header.VerticalOffset,
                Unknown0x08      = Header.Unknown0x08,
                CollisionSize    = Header.CollisionShadowDiameter,
                Scale            = Header.Scale / 65536.0f,

                FrameGroupsForSpritesheets = CreateSpriteFrames(framesWithDuplicates),
                AnimationsForSpritesheetAndDirections = CreateSpriteAnimations()
            };
        }

        private FrameGroupsForSpritesheet[] CreateSpriteFrames(HashSet<string> framesWithDuplicates) {
            if (FrameTable == null)
                return new FrameGroupsForSpritesheet[0];

            // Track the sprite whose frame groups are being built.
            string lastSpriteName = null;
            int? lastWidth = null;
            int? lastHeight = null;
            var spriteFrames = new List<FrameGroupsForSpritesheet>();
            FrameGroupsForSpritesheet lastSpriteFrames = null;

            // Track the frame group whose directions are being built.
            string lastFrameGroupName = null;
            var frameGroups = new List<FrameGroup>();
            FrameGroup lastFrameGroup = null;

            // Track directions to add to the current frame group being built.
            var frameGroupFrames = new List<SF3.CHR.Frame>();

            // Commits the current set of diections to the current frame group.
            void CommitFrameGroupDirections() {
                if (lastFrameGroup == null)
                    return;

                if (lastFrameGroup.Frames == null)
                    if (!AreExpectedFrameDirections(frameGroupFrames.Select(x => x.Direction).ToArray(), Header.Directions) || frameGroupFrames.Any(x => x.DuplicateKey != null))
                        lastFrameGroup.Frames = frameGroupFrames.ToArray();

                frameGroupFrames = new List<SF3.CHR.Frame>();
            }

            // Commits the current frame group to the current sprite.
            void CommitFrameGroup() {
                CommitFrameGroupDirections();

                if (lastSpriteFrames == null)
                    return;

                if (lastSpriteFrames.FrameGroups == null)
                    lastSpriteFrames.FrameGroups = frameGroups.ToArray();

                lastFrameGroupName = null;
                frameGroups        = new List<FrameGroup>();
                lastFrameGroup     = null;
            }

            // Gather a list of unique sprite names. This will be used for resolving
            // frames whose image hashes are for more than one sprite.
            var singularFrameRefSpriteNames = new HashSet<string>(FrameTable
                .Select(x => x.FrameRefs.GetUniqueSpriteName())
                .Where(x => x != null)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.First()));

            // Go through each frame, building a set of CHR_SpriteFrameDef's.
            foreach (var frame in FrameTable) {
                var bestSpriteName = singularFrameRefSpriteNames.FirstOrDefault(x => frame.FrameRefs.Any(y => y.SpriteName == x));
                var frameRef = (bestSpriteName == null) ? frame.FrameRefs.FirstOrDefault() : frame.FrameRefs.First(x => x.SpriteName == bestSpriteName);
                if (frameRef == null)
                    continue;

                var frameName  = frameRef?.FrameGroupName ?? frame.FrameName;
                var spriteName = (frameRef?.SpriteName  == SpriteName)    ? null        : frameRef.SpriteName;
                var width      = (frameRef?.FrameWidth  == Header.Width)  ? (int?) null : frameRef.FrameWidth;
                var height     = (frameRef?.FrameHeight == Header.Height) ? (int?) null : frameRef.FrameHeight;

                // Correct some very specific cases where the exact spritesheet to use can't be // determined without a little help:
                // TODO: these really shouldn't be hard-coded like this!!
                //
                // 1) Edmund (P1) has identical sprites shared between two spritesheets. They're identified with a different name,
                //    so use the name for the expected spritesheet.
                if (frame.SpriteName == "Edmund (P1) (Sword/Weaponless)")
                    spriteName = null;
                // 2) Murasame (P1) has Nodding and ShakingHead frames that don't have a weapon in the spritesheet with a sword.
                //    They've been duplicated in his sword spritesheet, so if we see them in there, don't use the weaponless name.
                else if (SpriteName == "Murasame (P1)" && frame.SpriteName == "Murasame (P1) (Weaponless)" && (frameName == "Nodding 2" || frameName == "ShakingHead 1" || frameName == "ShakingHead 2"))
                    spriteName = null;
                // 3) Explosions have a transparent frame
                else if (SpriteName == "Explosion" && frame.SpriteName == "Transparency")
                    spriteName = null;
                // 4) Waltz (U) has a ShakingHead frame that don't have a weapon in one of her w/ weapon animations.
                //    It's been been duplicated in her weapon spritesheet, so if we see it in there, don't use the weaponless name.
                else if (SpriteName == "Waltz (U)" && frame.SpriteName == "Waltz (U) (Weaponless)")
                    spriteName = null;

                // If the sprite name has changed, begin a new one.
                if (spriteName != lastSpriteName || width != lastWidth || height != lastHeight || lastSpriteFrames == null) {
                    CommitFrameGroup();

                    lastSpriteName = spriteName;
                    lastWidth      = width;
                    lastHeight     = height;
                    lastSpriteFrames = new FrameGroupsForSpritesheet() {
                        SpriteName = spriteName,
                        Width      = width,
                        Height     = height
                    };
                    spriteFrames.Add(lastSpriteFrames);
                }

                // If the frame group has changed, begin a new one.
                if (frameName != lastFrameGroupName || lastFrameGroup == null) {
                    CommitFrameGroupDirections();

                    lastFrameGroupName = frameName;
                    lastFrameGroup = new FrameGroup() {
                        Name = frame.FrameName,
                    };
                    frameGroups.Add(lastFrameGroup);
                }

                // If the frame's hash appears more than once, use the texture offset as a key.
                // This will ensure that duplicate frames aren't merged as duplicates if they
                // didn't share the same offset.
                string key = framesWithDuplicates.Contains(frame.TextureHash) ? frame.TextureOffset.ToString() : null;

                // Add the direction of this frame to the current frame group's directions.
                frameGroupFrames.Add(new SF3.CHR.Frame() { Direction = frame.Direction, DuplicateKey = key });
            }

            // Commit everything to the last sprite being built.
            CommitFrameGroup();

            // Return all the frames for every sprite.
            return spriteFrames.ToArray();
        }

        private static bool AreExpectedFrameDirections(SpriteFrameDirection[] directions, int dirCount) {
            var expectedDirections = CHR_Utils.GetCHR_FrameGroupDirections(dirCount);
            return Enumerable.SequenceEqual(directions, expectedDirections);
        }

        private AnimationsForSpritesheetAndDirection[] CreateSpriteAnimations() {
            if (AnimationTable == null)
                return new AnimationsForSpritesheetAndDirection[0];

            // Track the sprite whose animation groups are being built.
            string lastSpriteName = null;
            int? lastWidth = null;
            int? lastHeight = null;
            SpriteDirectionCountType? lastDirections = null;
            var spriteAnimations = new List<AnimationsForSpritesheetAndDirection>();
            AnimationsForSpritesheetAndDirection lastSpriteAnimations = null;

            // Track animations to add to the current animation group being built.
            var animations = new List<string>();

            // Commits the current animation group to the current sprite.
            void CommitAnimationGroup() {
                if (lastSpriteAnimations == null)
                    return;

                if (lastSpriteAnimations.Animations == null)
                    lastSpriteAnimations.Animations = animations.ToArray();

                lastDirections = null;

                animations = new List<string>();
            }

            // We want to add 'null' entries for empty animations, so build an array of animations
            // by AnimationIndex, with 'null' entries for missing indices.
            var animationArraySize = AnimationTable.Length > 0 ? (AnimationTable.Max(x => x.AnimationIndex) + 1) : 0;
            var animationArray = new Animation[animationArraySize];
            foreach (var animation in AnimationTable)
                animationArray[animation.AnimationIndex] = animation;

            // Go through each animation, building a set of CHR_SpriteFrameDef's.
            foreach (var animation in animationArray) {
                var spriteName = (animation.SpriteName == SpriteName) ? null : animation.SpriteName;
                var directions = animation.Directions == (SpriteDirectionCountType) Header.Directions ? (SpriteDirectionCountType?) null : animation.Directions;
                var width      = animation.AnimationInfo.Width  == Header.Width  ? (int?) null : animation.AnimationInfo.Width;
                var height     = animation.AnimationInfo.Height == Header.Height ? (int?) null : animation.AnimationInfo.Height;

                // Correct some very specific cases where the exact spritesheet to use can't be determined without a little help:
                // TODO: these really shouldn't be hard-coded like this!!
                //
                // 1) Explosions have a StillFrame animation that are duplicated in the 'Transparency' sprite.
                if (SpriteName == "Explosion" && animation.SpriteName == "Transparency")
                    spriteName = null;

                // If the first animation is null, we need to create an empty animation group for it.
                if (animation == null) {
                    if (lastSpriteAnimations == null) {
                        lastSpriteName = null;
                        lastWidth      = null;
                        lastHeight     = null;
                        lastDirections = null;

                        lastSpriteAnimations = new AnimationsForSpritesheetAndDirection();
                        spriteAnimations.Add(lastSpriteAnimations);
                    }
                    animations.Add(null);
                    continue;
                }

                // If the sprite or spritesheet info has changed, begin a new one.
                if (spriteName != lastSpriteName ||
                    width      != lastWidth      ||
                    height     != lastHeight     ||
                    directions != lastDirections ||
                    lastSpriteAnimations == null
                ) {
                    CommitAnimationGroup();

                    lastSpriteName = spriteName;
                    lastWidth      = width;
                    lastHeight     = height;
                    lastDirections = directions;

                    // If the previous sprite was a set of 'null' animations, let's hijack it and
                    // use it as the current sprite + animations, effectively removing one redundant
                    // sprite at front.
                    if (lastSpriteAnimations != null &&
                        lastSpriteAnimations.SpriteName == null &&
                        lastSpriteAnimations.Width      == null &&
                        lastSpriteAnimations.Height     == null &&
                        lastSpriteAnimations.Directions == null &&
                        lastSpriteAnimations.Animations != null &&
                        lastSpriteAnimations.Animations.All(x => x == null)
                    ) {
                        lastSpriteAnimations.SpriteName = spriteName;
                        lastSpriteAnimations.Width      = width;
                        lastSpriteAnimations.Height     = height;
                        lastSpriteAnimations.Directions = directions;

                        animations = lastSpriteAnimations.Animations.ToList();
                        lastSpriteAnimations.Animations = null;
                    }
                    else {
                        lastSpriteAnimations = new AnimationsForSpritesheetAndDirection() {
                            SpriteName = spriteName,
                            Width = width,    
                            Height = height,
                            Directions = directions,
                        };
                        spriteAnimations.Add(lastSpriteAnimations);
                    }
                }

                // Add the current animation to the animation group.
                animations.Add(animation.AnimationName);
            }

            // Commit everything to the last sprite being built.
            CommitAnimationGroup();

            // Return all the animations for every sprite.
            return spriteAnimations.ToArray();
        }

        public int IDInGroup { get; }
        public uint DataOffset { get; }
        public INameGetterContext NameGetterContext { get; }
        public string SpriteName { get; }
        public string DropdownName { get; }

        public SpriteHeader Header { get; }
        public AnimationOffsetTable AnimationOffsetTable { get; }
        public FrameTable FrameTable { get; }
        public Dictionary<int, AnimationCommandTable> AnimationCommandTablesByIndex { get; }
        public AnimationTable AnimationTable { get; }

        // TODO: show in a view
        public uint TotalCompressedFramesSize { get; }
    }
}
