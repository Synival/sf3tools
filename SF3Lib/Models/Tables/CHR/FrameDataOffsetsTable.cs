using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class FrameDataOffsetsTable : AddressedTable<FrameDataOffsets> {
        protected FrameDataOffsetsTable(IByteData data, string name, int[] addresses, uint[] dataOffsets)
        : base(data, name, addresses) {
            DataOffsets = dataOffsets;
        }

        public static FrameDataOffsetsTable Create(IByteData data, string name, int[] addresses, uint[] dataOffsets) {
            var newTable = new FrameDataOffsetsTable(data, name, addresses, dataOffsets);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new FrameDataOffsets(Data, id, $"Sprite_FrameDataOffsets{id:D2}", addr, DataOffsets[id])
            );
        }

        public uint[] DataOffsets { get; }
    }
}
