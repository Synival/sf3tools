using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X013 {
    public class CritModTable : Table<CritMod> {
        protected CritModTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static CritModTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new CritModTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new CritMod(Data, id, name, address));

        public override int? MaxSize => 1;
    }
}
