using SF3.Models.Structs.X033_X031;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X033_X031 {
    public class StatsTable : Table<Stats> {
        public StatsTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Stats(Data, id, name, address));

        public override int? MaxSize => 300;
    }
}
