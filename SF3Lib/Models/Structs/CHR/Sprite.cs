using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Sprite : Struct {
        public Sprite(IByteData data, int id, string name, int address, uint dataOffset, INameGetterContext ngc)
        : base(data, id, name, address, 0x18 /* just the header */) {
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

            FrameOffsetTable     = FrameOffsetTable.Create(Data, nameof(FrameOffsetTable), (int) (DataOffset + Header.FrameTableOffset));
            AnimationOffsetTable = AnimationOffsetTable.Create(Data, nameof(AnimationOffsetTable), (int) (DataOffset + Header.AnimationTableOffset));

            FrameTable = FrameTable.Create(
                Data,
                $"Sprite{ID:D2}_Frames",
                (int) (DataOffset + Header.FrameTableOffset),
                DataOffset, Header.Width, Header.Height,
                $"Sprite{ID:D2}_",
                ID,
                Header.SpriteID,
                Header.Directions);

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
                    x.ID))
                .ToDictionary(x => x.AnimIndex, x => x);

            AnimationTable = AnimationTable.Create(Data, $"Sprite{ID:D2}_{nameof(AnimationTable)}", AnimationFrameTablesByIndex.Values.ToArray(),
                $"Sprite{ID:D2}_");
        }

        public uint DataOffset { get; }
        public INameGetterContext NameGetterContext { get; }
        public string SpriteName { get; }
        public string DropdownName { get; }

        public SpriteHeader Header { get; }
        public FrameOffsetTable FrameOffsetTable { get; }
        public AnimationOffsetTable AnimationOffsetTable { get; }
        public FrameTable FrameTable { get; }
        public Dictionary<int, AnimationFrameTable> AnimationFrameTablesByIndex { get; }
        public AnimationTable AnimationTable { get; }
    }
}
