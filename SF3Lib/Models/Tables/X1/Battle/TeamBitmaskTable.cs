using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class TeamBitmaskTable : FixedSizeTable<TeamBitmask> {
        protected TeamBitmaskTable(IByteData data, string name, int address, string itemNamePrefix)
        : base(data, name, address, 16) {
            ItemNamePrefix = itemNamePrefix;
        }

        public static TeamBitmaskTable Create(IByteData data, string name, int address, string itemNamePrefix)
            => Create(() => new TeamBitmaskTable(data, name, address, itemNamePrefix));

        public override bool Load()
            => Load((id, address) => new TeamBitmask(Data, id, $"{ItemNamePrefix}{id:D2}", address));

        public string ItemNamePrefix { get; }
    }
}
