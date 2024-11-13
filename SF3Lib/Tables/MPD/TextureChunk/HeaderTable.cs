using SF3.RawEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Header(Editor, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
