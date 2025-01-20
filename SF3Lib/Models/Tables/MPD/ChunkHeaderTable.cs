using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ChunkHeaderTable : FixedSizeTable<ChunkHeader> {
        protected ChunkHeaderTable(IByteData data, int address) : base(data, address, 32) {
        }

        public static ChunkHeaderTable Create(IByteData data, int address) {
            var newTable = new ChunkHeaderTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ChunkHeader(Data, id, "Chunk" + id.ToString("D2"), address));
    }
}
