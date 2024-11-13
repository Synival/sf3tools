using SF3.StreamEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Header(FileEditor, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
