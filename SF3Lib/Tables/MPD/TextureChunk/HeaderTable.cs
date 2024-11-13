using SF3.RawEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IRawEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Header(FileEditor, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
