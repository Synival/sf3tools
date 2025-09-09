using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class StatsTable : ResourceTable<Stats> {
        protected StatsTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static StatsTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new StatsTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Stats(Data, id, name, address));
    }
}
