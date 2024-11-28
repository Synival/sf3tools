using SF3.RawEditors;
using SF3.Structs.MPD.TextureChunk;

namespace SF3.TableModels.MPD.TextureChunk {
    public class TextureHeaderTable : Table<TextureHeader> {
        public TextureHeaderTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new TextureHeader(Editor, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
