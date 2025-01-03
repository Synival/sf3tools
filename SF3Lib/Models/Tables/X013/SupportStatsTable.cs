using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X013 {
    public class SupportStatsTable : Table<SupportStats> {
        protected SupportStatsTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static SupportStatsTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SupportStatsTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportStats(Data, id, name, address));

        public override int? MaxSize => 256;
    }
}
