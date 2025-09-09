using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class StatBoostTable : ResourceTable<StatBoost> {
        protected StatBoostTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static StatBoostTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new StatBoostTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new StatBoost(Data, id, name, address));
    }
}
