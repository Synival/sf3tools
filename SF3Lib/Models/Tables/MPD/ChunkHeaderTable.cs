using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class ChunkHeaderTable : Table<ChunkHeader> {
        public ChunkHeaderTable(IRawData editor, int address) : base(editor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ChunkHeader(Data, id, "Chunk" + id.ToString("D2"), address));

        public override int? MaxSize => 32;
    }
}
