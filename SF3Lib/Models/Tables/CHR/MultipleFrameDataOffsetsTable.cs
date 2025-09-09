using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class MultipleFrameDataOffsetsTable : AddressedTable<MultipleFrameDataOffsetsRow> {
        protected MultipleFrameDataOffsetsTable(IByteData data, string name, int[] addresses, uint[] dataOffsets, int[] spriteIds)
        : base(data, name, addresses) {
            DataOffsets = dataOffsets;
            SpriteIDs   = spriteIds;
        }

        public static MultipleFrameDataOffsetsTable Create(IByteData data, string name, int[] addresses, uint[] dataOffsets, int[] spriteIds)
            => Create(() => new MultipleFrameDataOffsetsTable(data, name, addresses, dataOffsets, spriteIds));

        public override bool Load() {
            return Load(
                (id, addr) => new MultipleFrameDataOffsetsRow(Data, id, $"Sprite_FrameDataOffsets{id:D2}", addr, DataOffsets[id], SpriteIDs[id])
            );
        }

        public uint[] DataOffsets { get; }
        public int[] SpriteIDs { get; }
    }
}
