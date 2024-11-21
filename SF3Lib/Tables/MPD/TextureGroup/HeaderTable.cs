using SF3.Models.MPD.TextureGroup;
using SF3.RawEditors;

namespace SF3.Tables.MPD.TextureGroup {
    public class HeaderTable : Table<HeaderModel> {
        public HeaderTable(IRawEditor editor, int address, bool is32Bit) : base(editor, address) {
            Is32Bit = is32Bit;
            _textureEndId = is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    uint textureId = Editor.GetData(address, Is32Bit ? 4 : 2);
                    var atEnd = textureId == _textureEndId;
                    return new HeaderModel(Editor, id, atEnd ? "--" : "TexGroup" + id, address, Is32Bit);
                },
                (currentRows, model) => model.TextureID != _textureEndId);
        }

        public bool Is32Bit { get; }

        private uint _textureEndId;
    }
}
