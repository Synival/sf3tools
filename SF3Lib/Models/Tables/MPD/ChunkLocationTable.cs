using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ChunkLocationTable : FixedSizeTable<ChunkLocation> {
        protected ChunkLocationTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static ChunkLocationTable Create(IByteData data, string name, int address)
            => Create(() => new ChunkLocationTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new ChunkLocation(Data, id, "Chunk" + id.ToString("D2"), address));
    }
}
