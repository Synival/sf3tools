using SF3.Models.Structs.MPD;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class MPDHeaderTable : Table<MPDHeaderModel> {
        protected MPDHeaderTable(IByteData data, int address, bool hasPalette3) : base(data, address) {
            HasPalette3 = hasPalette3;
        }

        public static MPDHeaderTable Create(IByteData data, int address, bool hasPalette3) {
            var newTable = new MPDHeaderTable(data, address, hasPalette3);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new MPDHeaderModel(Data, id, "Header", address, HasPalette3));

        public override int? MaxSize => 1;
        public bool HasPalette3 { get; }
    }
}
