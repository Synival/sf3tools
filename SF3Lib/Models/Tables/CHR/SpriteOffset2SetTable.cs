using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteOffset2SetTable : AddressedTable<SpriteOffset2Set> {
        protected SpriteOffset2SetTable(IByteData data, string name, int[] addresses, uint[] dataOffsets)
        : base(data, name, addresses) {
            DataOffsets = dataOffsets;
        }

        public static SpriteOffset2SetTable Create(IByteData data, string name, int[] addresses, uint[] dataOffsets) {
            var newTable = new SpriteOffset2SetTable(data, name, addresses, dataOffsets);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new SpriteOffset2Set(Data, id, $"{nameof(SpriteOffset2Set)}{id:D2}", addr, DataOffsets[id])
            );
        }

        public uint[] DataOffsets { get; }
    }
}
