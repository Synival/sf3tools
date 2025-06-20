using System.Collections.Generic;
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

/*
            var frameTableOffsets = .DataOffset + x.FrameTableOffset)).ToArray();
            int[] GetAnimationTableOffsets(SpriteHeaderTable st)
                => st.Select(x => (int) (x.DataOffset + x.AnimationTableOffset)).ToArray();
            uint[] GetDataOffsets(SpriteHeaderTable st)
                => st.Select(x => x.DataOffset).ToArray();
*/

            var spriteIdPropInfo = typeof(SpriteHeader).GetProperty(nameof(Header.SpriteID));
            SpriteName = NameGetterContext.GetName(Header, spriteIdPropInfo, Header.SpriteID, new object[] { NamedValueType.Sprite })
                + $" ({Header.Width}x{Header.Height}x{Header.Directions})"
                + (Header.PromotionLevel > 0 ? $" (P{Header.PromotionLevel})" : "")
                ;

/*
            FrameDataOffsetsTable = FrameDataOffsetsTable.Create(Data, nameof(FrameDataOffsetsTable),
                GetFrameTableOffsets(SpriteHeaderTable), GetDataOffsets(SpriteHeaderTable), spriteIds);
            FrameTablesByFileAddr = SpriteHeaderTable
                .Select(x => FrameTable.Create(
                    Data,
                    $"Sprite{x.ID:D2}_Frames ({spriteNames[x.ID]})",
                    (int) (x.DataOffset + x.FrameTableOffset),
                    x.DataOffset, x.Width, x.Height,
                    $"Sprite{x.ID:D2}_",
                    x.ID,
                    x.SpriteID,
                    x.Directions))
                .ToDictionary(x => x.Address, x => x);

            AnimationOffsetsTable = AnimationOffsetsTable.Create(Data, nameof(AnimationOffsetsTable),
                GetAnimationTableOffsets(SpriteHeaderTable), GetDataOffsets(SpriteHeaderTable), spriteIds);
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

            var tables = new List<ITable>() {
                SpriteHeaderTable,
                FrameDataOffsetsTable,
                AnimationOffsetsTable
            };
            tables.AddRange(FrameTablesByFileAddr.Values);
            tables.AddRange(AnimationFrameTablesByAddr.Values);
*/
        }

        public uint DataOffset { get; }
        public INameGetterContext NameGetterContext { get; }
        public string SpriteName { get; }

        public SpriteHeader Header { get; }
/*
        public FrameDataOffsetsTable FrameDataOffsetsTable { get; }
        public Dictionary<int, FrameTable> FrameTablesByFileAddr { get; }
        public AnimationOffsetsTable AnimationOffsetsTable { get; }
        public Dictionary<int, AnimationFrameTable> AnimationFrameTablesByAddr { get; }
*/
    }
}
