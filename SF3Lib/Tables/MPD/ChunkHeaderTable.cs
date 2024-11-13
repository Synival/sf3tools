using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class ChunkHeaderTable : Table<ChunkHeader> {
        public ChunkHeaderTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ChunkHeader(FileEditor, id, "Chunk" + id, address));

        public override int? MaxSize => 32;
    }
}
