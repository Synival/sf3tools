using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class ColorTable : Table<Color> {
        public ColorTable(IRawData editor, int address, int colors) : base(editor, address) {
            MaxSize = colors;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Color(Editor, id, "Color" + id.ToString("D3"), address));

        public override int? MaxSize { get; }
    }
}
