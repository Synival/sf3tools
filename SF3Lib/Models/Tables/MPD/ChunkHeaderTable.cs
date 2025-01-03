using SF3.Models.Structs.MPD;
using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class ChunkHeaderTable : Table<ChunkHeader> {
        protected ChunkHeaderTable(IByteData data, int address) : base(data, address) {
        }

        public static ChunkHeaderTable Create(IByteData data, int address) {
            var newTable = new ChunkHeaderTable(data, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ChunkHeader(Data, id, "Chunk" + id.ToString("D2"), address));

        public override int? MaxSize => 32;
    }
}
