using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class MPDHeaderTable : Table<MPDHeaderModel> {
        public MPDHeaderTable(IRawData editor, int address, bool hasPalette3) : base(editor, address) {
            HasPalette3 = hasPalette3;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new MPDHeaderModel(Data, id, "Header", address, HasPalette3));

        public override int? MaxSize => 1;
        public bool HasPalette3 { get; }
    }
}
