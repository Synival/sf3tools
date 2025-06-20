using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class MultipleAnimationOffsetsTable : AddressedTable<MultipleAnimationOffsetsRow> {
        protected MultipleAnimationOffsetsTable(IByteData data, string name, int[] addresses, uint[] dataOffsets, int[] spriteIds)
        : base(data, name, addresses) {
            DataOffsets = dataOffsets;
            SpriteIDs   = spriteIds;
        }

        public static MultipleAnimationOffsetsTable Create(IByteData data, string name, int[] addresses, uint[] dataOffsets, int[] spriteIds) {
            var newTable = new MultipleAnimationOffsetsTable(data, name, addresses, dataOffsets, spriteIds);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new MultipleAnimationOffsetsRow(Data, id, $"Sprite_AnimationOffsets{id:D2}", addr, DataOffsets[id], SpriteIDs[id])
            );
        }

        public uint[] DataOffsets { get; }
        public int[] SpriteIDs { get; }
    }
}
