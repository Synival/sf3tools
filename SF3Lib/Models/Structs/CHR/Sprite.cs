using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Sprites;
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
            if (!Header.IsValid())
                return;

            var spriteIdPropInfo = typeof(SpriteHeader).GetProperty(nameof(Header.SpriteID));
            SpriteName = NameGetterContext.GetName(Header, spriteIdPropInfo, Header.SpriteID, new object[] { NamedValueType.Sprite });

            DropdownName = Name + " - "
                + SpriteName
                + $" ({Header.Width}x{Header.Height}x{Header.Directions})"
                + (Header.PromotionLevel > 0 ? $" (P{Header.PromotionLevel})" : "")
                ;

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
            if (nextId != 0xFFFF)
                nextAnimationTableOffset = Data.GetDouble((int) DataOffset + Data.GetDouble(Address + 0x18 + 0x14));
            else {
                var firstHeaderAddr = Address - 0x18 * IDInGroup;
                var firstFrameTableOffset = Data.GetDouble(firstHeaderAddr + 0x10);
                var firstFrameOffset = Data.GetDouble(firstFrameTableOffset + (int) DataOffset);
                nextAnimationTableOffset = Math.Min(firstFrameTableOffset, firstFrameOffset);
            }
            var animationTableSize = (nextAnimationTableOffset - (int) Header.AnimationTableOffset) / 4;

            bool isXOP101_Masqurin = FrameTable.Length == 144;
            AnimationOffsetTable = AnimationOffsetTable.Create(Data, nameof(AnimationOffsetTable), (int) (DataOffset + Header.AnimationTableOffset), animationTableSize);

            var aniOffsets = AnimationOffsetTable.Select(x => x.Offset).Concat(new uint[] { Header.AnimationTableOffset }).ToArray();
            for (int i = aniOffsets.Length - 1; i >= 0; i--)
                if (aniOffsets[i] == 0)
                    aniOffsets[i] = aniOffsets[i + 1];

            AnimationFrameTablesByIndex = AnimationOffsetTable
                .Where(x => x.Offset != 0)
                .Select(x => AnimationFrameTable.Create(
                    Data,
                    $"Sprite{ID:D2}_Animation{x.ID:D2}",
                    (int) (DataOffset + x.Offset),
                    $"Sprite{ID:D2}_Animation{x.ID:D2}_",
                    ID,
                    Header.SpriteID,
                    Header.Directions,
                    x.ID,
                    FrameTable,
                    (int) ((aniOffsets[x.ID + 1] - aniOffsets[x.ID]) / 4)))
                .ToDictionary(x => x.AnimationIndex, x => x);

            AnimationTable = AnimationTable.Create(Data, $"Sprite{ID:D2}_{nameof(AnimationTable)}", Header.Directions, AnimationFrameTablesByIndex.Values.ToArray(),
                FrameTable, $"Sprite{ID:D2}_");

            var spriteNames = AnimationTable.Select(x => x.SpriteName).Distinct().ToArray();
            foreach (var spriteName in spriteNames) {
                var infoName = $"{spriteName} ({Header.Width}x{Header.Height})";
                CHR_Utils.AddSpriteHeaderInfo(infoName, Header.SpriteID, Header.VerticalOffset, Header.Unknown0x08, Header.CollisionShadowDiameter, Header.Scale / 65536.0f);
            }

            TotalCompressedFramesSize = (uint) FrameTable.Sum(x => x.TextureCompressedSize);

            // TODO: shouldn't be here! We need a separate view for just the sprite.
            Header.TotalCompressedFramesSize = TotalCompressedFramesSize;
        }

        public CHR_SpriteDef ToCHR_SpriteDef() {
            return new CHR_SpriteDef() {
                SpriteID         = Header.SpriteID,
                Width            = Header.Width,
                Height           = Header.Height,
                Directions       = Header.Directions,
                SpriteFrames     = CreateSpriteFrames(),
                SpriteAnimations = CreateSpriteAnimations()
            };
        }

        private CHR_SpriteFramesDef[] CreateSpriteFrames() {
            // Track the sprite whose frame groups are being built.
            string lastSpriteName = null;
            var spriteFrames = new List<CHR_SpriteFramesDef>();
            CHR_SpriteFramesDef lastSpriteFrames = null;

            // Track the frame group whose directions are being built.
            string lastFrameGroupName = null;
            var frameGroups = new List<CHR_FrameGroupDef>();
            CHR_FrameGroupDef lastFrameGroup = null;

            // Track directions to add to the current frame group being built.
            var frameGroupDirections = new List<SpriteFrameDirection>();

            // Commits the current set of diections to the current frame group.
            void CommitFrameGroupDirections() {
                if (lastFrameGroup == null)
                    return;

                if (lastFrameGroup.Directions == null)
                    if (!AreExpectedFrameDirections(frameGroupDirections.ToArray(), Header.Directions))
                        lastFrameGroup.Directions = frameGroupDirections.Select(x => x.ToString()).ToArray();

                frameGroupDirections = new List<SpriteFrameDirection>();
            }

            // Commits the current frame group to the current sprite.
            void CommitFrameGroup() {
                CommitFrameGroupDirections();

                if (lastSpriteFrames == null)
                    return;

                if (lastSpriteFrames.FrameGroups == null)
                    lastSpriteFrames.FrameGroups = frameGroups.ToArray();

                lastFrameGroupName = null;
                frameGroups        = new List<CHR_FrameGroupDef>();
                lastFrameGroup     = null;
            }

            // Go through each frame, building a set of CHR_SpriteFrameDef's.
            foreach (var frame in FrameTable) {
                // If the sprite name has changed, begin a new one.
                if (frame.SpriteName != lastSpriteName || lastSpriteFrames == null) {
                    CommitFrameGroup();

                    lastSpriteName = frame.SpriteName;
                    lastSpriteFrames = new CHR_SpriteFramesDef() {
                        SpriteName = frame.SpriteName
                    };
                    spriteFrames.Add(lastSpriteFrames);
                }

                // If the frame group has changed, begin a new one.
                if (frame.FrameName != lastFrameGroupName || lastFrameGroup == null) {
                    CommitFrameGroupDirections();

                    lastFrameGroupName = frame.FrameName;
                    lastFrameGroup = new CHR_FrameGroupDef() {
                        Name = frame.FrameName
                    };
                    frameGroups.Add(lastFrameGroup);
                }

                // Add the direction of this frame to the current frame group's directions.
                frameGroupDirections.Add(frame.Direction);
            }

            // Commit everything to the last sprite being built.
            CommitFrameGroup();

            // Return all the frames for every sprite.
            return spriteFrames.ToArray();
        }

        private static bool AreExpectedFrameDirections(SpriteFrameDirection[] directions, int dirCount) {
            switch (dirCount) {
                case 1:
                case 0x11:
                    return Enumerable.SequenceEqual(directions, new SpriteFrameDirection[] {
                        SpriteFrameDirection.First,
                    });

                case 2:
                    return Enumerable.SequenceEqual(directions, new SpriteFrameDirection[] {
                        SpriteFrameDirection.First,
                        SpriteFrameDirection.Second,
                    });

                case 4:
                    return Enumerable.SequenceEqual(directions, new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                    });

                case 5:
                    return Enumerable.SequenceEqual(directions, new SpriteFrameDirection[] {
                        SpriteFrameDirection.S,
                        SpriteFrameDirection.SE,
                        SpriteFrameDirection.E,
                        SpriteFrameDirection.NE,
                        SpriteFrameDirection.N,
                    });

                case 6:
                    return Enumerable.SequenceEqual(directions, new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.S,
                        SpriteFrameDirection.N,
                    });

                case 8:
                    return Enumerable.SequenceEqual(directions, new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.NNW,
                        SpriteFrameDirection.WNW,
                        SpriteFrameDirection.WSW,
                        SpriteFrameDirection.SSW,
                    });

                default:
                    return false;
            }
        }

        private CHR_SpriteAnimationsDef[] CreateSpriteAnimations() {
            // Track the sprite whose animation groups are being built.
            string lastSpriteName = null;
            var spriteAnimations = new List<CHR_SpriteAnimationsDef>();
            CHR_SpriteAnimationsDef lastSpriteAnimations = null;

            // Track the animation group whose animations are being built.
            int? lastAnimationGroupDirections = null;
            var animationGroups = new List<CHR_AnimationGroupDef>();
            CHR_AnimationGroupDef lastAnimationGroup = null;

            // Track animations to add to the current animation group being built.
            var animations = new List<string>();

            // Commits the current set of animations to the current animation group.
            void CommitAnimationGroupAnimations() {
                if (lastAnimationGroup == null)
                    return;

                if (lastAnimationGroup.Animations == null)
                    lastAnimationGroup.Animations = animations.ToArray();

                animations = new List<string>();
            }

            // Commits the current animation group to the current sprite.
            void CommitAnimationGroup() {
                CommitAnimationGroupAnimations();

                if (lastSpriteAnimations == null)
                    return;

                if (lastSpriteAnimations.AnimationGroups == null)
                    lastSpriteAnimations.AnimationGroups = animationGroups.ToArray();

                lastAnimationGroupDirections = null;
                animationGroups    = new List<CHR_AnimationGroupDef>();
                lastAnimationGroup = null;
            }

            // We want to add 'null' entries for empty animations, so build an array of animations
            // by AnimationIndex, with 'null' entries for missing indices.
            var animationArraySize = AnimationTable.Length > 0 ? (AnimationTable.Max(x => x.AnimationIndex) + 1) : 0;
            var animationArray = new Animation[animationArraySize];
            foreach (var animation in AnimationTable)
                animationArray[animation.AnimationIndex] = animation;

            // Go through each animation, building a set of CHR_SpriteFrameDef's.
            foreach (var animation in animationArray) {
                var directions = animation.Directions == Header.Directions ? (int?) null : animation.Directions;

                if (animation == null) {
                    if (lastSpriteAnimations == null) {
                        lastSpriteName = null;
                        lastSpriteAnimations = new CHR_SpriteAnimationsDef() { SpriteName = null };
                        spriteAnimations.Add(lastSpriteAnimations);

                        lastAnimationGroupDirections = 0;
                        lastAnimationGroup = new CHR_AnimationGroupDef() { Directions = null };
                        animationGroups.Add(lastAnimationGroup);
                    }
                    animations.Add(null);
                    continue;
                }

                // If the sprite name has changed, begin a new one.
                if (animation.SpriteName != lastSpriteName || lastSpriteAnimations == null) {
                    CommitAnimationGroup();

                    // If the previous sprite was a set of 'null' animations, let's hijack it and
                    // use it as the current sprite + animations, effectively removing one redundant
                    // sprite at front.
                    if (lastSpriteAnimations != null && lastSpriteAnimations.SpriteName == null) {
                        lastSpriteName = animation.SpriteName;
                        lastSpriteAnimations.SpriteName = animation.SpriteName;

                        lastAnimationGroupDirections = directions;
                        lastAnimationGroup = lastSpriteAnimations.AnimationGroups.Last();
                        lastAnimationGroup.Directions = directions;

                        animations = lastAnimationGroup.Animations.ToList();
                        lastAnimationGroup.Animations = null;
                    }
                    else {
                        lastSpriteName = animation.SpriteName;
                        lastSpriteAnimations = new CHR_SpriteAnimationsDef() {
                            SpriteName = animation.SpriteName
                        };
                        spriteAnimations.Add(lastSpriteAnimations);
                    }
                }

                // If the animation group has changed, begin a new one.
                if (directions != lastAnimationGroupDirections || lastAnimationGroup == null) {
                    CommitAnimationGroupAnimations();

                    lastAnimationGroupDirections = directions;
                    lastAnimationGroup = new CHR_AnimationGroupDef() {
                        Directions = directions
                    };
                    animationGroups.Add(lastAnimationGroup);
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
        public Dictionary<int, AnimationFrameTable> AnimationFrameTablesByIndex { get; }
        public AnimationTable AnimationTable { get; }

        // TODO: show in a view
        public uint TotalCompressedFramesSize { get; }
    }
}
