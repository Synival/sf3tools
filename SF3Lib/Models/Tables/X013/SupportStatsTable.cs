using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SupportStatsTable : ResourceTable<SupportStats> {
        protected SupportStatsTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 256) {
        }

        public static SupportStatsTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new SupportStatsTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new SupportStats(Data, id, name, address));
    }
}
