using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteOffset1SetTable : AddressedTable<SpriteOffset1Set> {
        protected SpriteOffset1SetTable(IByteData data, string name, int[] addresses, uint[] dataOffsets)
        : base(data, name, addresses) {
            DataOffsets = dataOffsets;
        }

        public static SpriteOffset1SetTable Create(IByteData data, string name, int[] addresses, uint[] dataOffsets) {
            var newTable = new SpriteOffset1SetTable(data, name, addresses, dataOffsets);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new SpriteOffset1Set(Data, id, $"{nameof(SpriteOffset1Set)}{id:D2}", addr, DataOffsets[id])
            );
        }

        public uint[] DataOffsets { get; }
    }
}
