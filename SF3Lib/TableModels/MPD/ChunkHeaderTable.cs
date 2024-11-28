using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.TableModels.MPD {
    public class ChunkHeaderTable : Table<ChunkHeader> {
        public ChunkHeaderTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ChunkHeader(Editor, id, "Chunk" + id.ToString("D2"), address));

        public override int? MaxSize => 32;
    }
}
