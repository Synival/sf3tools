using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
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

            AnimationTable = AnimationTable.Create(Data, $"Sprite{ID:D2}_{nameof(AnimationTable)}", AnimationFrameTablesByIndex.Values.ToArray(),
                FrameTable, $"Sprite{ID:D2}_");

            var spriteNames = AnimationTable.Select(x => x.SpriteName).Distinct().ToArray();
            foreach (var spriteName in spriteNames) {
                var infoName = $"{spriteName} ({Header.Width}x{Header.Height})";
                CHR_Utils.AddSpriteHeaderInfo(infoName, Header.VerticalOffset, Header.Unknown0x08, Header.CollisionShadowDiameter, Header.Scale / 65536.0f);
            }

            TotalCompressedFramesSize = (uint) FrameTable.Sum(x => x.TextureCompressedSize);

            // TODO: shouldn't be here! We need a separate view for just the sprite.
            Header.TotalCompressedFramesSize = TotalCompressedFramesSize;
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
