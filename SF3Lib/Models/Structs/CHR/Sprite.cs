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

/*
            var frameTableOffsets = .DataOffset + x.FrameTableOffset)).ToArray();
            int[] GetAnimationTableOffsets(SpriteHeaderTable st)
                => st.Select(x => (int) (x.DataOffset + x.AnimationTableOffset)).ToArray();
            uint[] GetDataOffsets(SpriteHeaderTable st)
                => st.Select(x => x.DataOffset).ToArray();
*/

            var spriteIdPropInfo = typeof(SpriteHeader).GetProperty(nameof(Header.SpriteID));
            SpriteName = NameGetterContext.GetName(Header, spriteIdPropInfo, Header.SpriteID, new object[] { NamedValueType.Sprite });

            DropdownName = Name + " - "
                + SpriteName
                + $" ({Header.Width}x{Header.Height}x{Header.Directions})"
                + (Header.PromotionLevel > 0 ? $" (P{Header.PromotionLevel})" : "")
                ;

            FrameOffsetTable     = FrameOffsetTable.Create(Data, nameof(FrameOffsetTable), Address + (int) Header.FrameTableOffset);
            AnimationOffsetTable = AnimationOffsetTable.Create(Data, nameof(AnimationOffsetTable), Address + (int) Header.AnimationTableOffset);

            FrameTable = FrameTable.Create(
                Data,
                $"Sprite{ID:D2}_Frames ({SpriteName})",
                (int) (DataOffset + Header.FrameTableOffset),
                DataOffset, Header.Width, Header.Height,
                $"Sprite{ID:D2}_",
                ID,
                Header.SpriteID,
                Header.Directions);

/*
            AnimationFrameTablesByAddr = AnimationOffsetsTable
                .SelectMany(x => x.Select((y, i) => new {
                    AnimOffsets = x, FileAddr = (int) (y + x.DataOffset), Index = i, Offset = y })
                    .Where(y => y.Offset != 0)
                )
                .Select(x => AnimationFrameTable.Create(
                    Data,
                    $"Sprite{x.AnimOffsets.ID:D2}_Animation{x.Index:D2} ({spriteNames[x.AnimOffsets.ID]})",
                    x.FileAddr,
                    $"Sprite{x.AnimOffsets.ID:D2}_Animation{x.Index:D2}_",
                    x.AnimOffsets.ID,
                    x.AnimOffsets.SpriteID,
                    SpriteHeaderTable[x.AnimOffsets.ID].Directions,
                    x.Index))
                .ToDictionary(x => x.Address, x => x);
*/
        }

        public uint DataOffset { get; }
        public INameGetterContext NameGetterContext { get; }
        public string SpriteName { get; }
        public string DropdownName { get; }

        public SpriteHeader Header { get; }
        public FrameOffsetTable FrameOffsetTable { get; }
        public AnimationOffsetTable AnimationOffsetTable { get; }
        public FrameTable FrameTable { get; }
/*
        public Dictionary<int, AnimationFrameTable> AnimationFrameTablesByAddr { get; }
*/
    }
}
