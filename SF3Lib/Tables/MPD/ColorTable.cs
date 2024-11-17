using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class ColorTable : Table<Color> {
        public ColorTable(IRawEditor editor, int address, int colors) : base(editor, address) {
            MaxSize = colors;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Color(Editor, id, "Color " + id.ToString("D3"), address));

        public override int? MaxSize { get; }
    }
}
