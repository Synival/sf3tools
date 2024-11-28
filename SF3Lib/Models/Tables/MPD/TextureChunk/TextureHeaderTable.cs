using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.MPD.TextureChunk {
    public class TextureHeaderTable : Table<TextureHeader> {
        public TextureHeaderTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new TextureHeader(Editor, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
