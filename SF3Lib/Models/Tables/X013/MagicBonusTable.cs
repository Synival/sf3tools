using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X013 {
    public class MagicBonusTable : Table<MagicBonus> {
        public MagicBonusTable(IRawData data, string resourceFile, int address, bool has32BitValues) : base(data, resourceFile, address) {
            Has32BitValues = has32BitValues;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new MagicBonus(Data, id, name, address, Has32BitValues));

        public override int? MaxSize => 256;

        public bool Has32BitValues { get; }
    }
}
