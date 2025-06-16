using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationOffsetsTable : AddressedTable<AnimationOffsets> {
        protected AnimationOffsetsTable(IByteData data, string name, int[] addresses, uint[] dataOffsets)
        : base(data, name, addresses) {
            DataOffsets = dataOffsets;
        }

        public static AnimationOffsetsTable Create(IByteData data, string name, int[] addresses, uint[] dataOffsets) {
            var newTable = new AnimationOffsetsTable(data, name, addresses, dataOffsets);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new AnimationOffsets(Data, id, $"Sprite_AnimationOffsets{id:D2}", addr, DataOffsets[id])
            );
        }

        public uint[] DataOffsets { get; }
    }
}
