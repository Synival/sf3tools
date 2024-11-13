using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IRawEditor fileEditor, int address, bool hasPalette3) : base(fileEditor, address) {
            HasPalette3 = hasPalette3;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Header(FileEditor, id, "Header", address, HasPalette3));

        public override int? MaxSize => 1;
        public bool HasPalette3 { get; }
    }
}
