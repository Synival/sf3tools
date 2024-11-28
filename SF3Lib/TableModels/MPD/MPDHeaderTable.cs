using SF3.RawEditors;
using SF3.Structs.MPD;

namespace SF3.TableModels.MPD {
    public class MPDHeaderTable : Table<MPDHeaderModel> {
        public MPDHeaderTable(IRawEditor editor, int address, bool hasPalette3) : base(editor, address) {
            HasPalette3 = hasPalette3;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new MPDHeaderModel(Editor, id, "Header", address, HasPalette3));

        public override int? MaxSize => 1;
        public bool HasPalette3 { get; }
    }
}
