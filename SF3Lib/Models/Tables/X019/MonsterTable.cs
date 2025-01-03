using SF3.Models.Structs.X019;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X019 {
    public class MonsterTable : Table<Monster> {
        protected MonsterTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static MonsterTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new MonsterTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Monster(Data, id, name, address));

        public override int? MaxSize => 256;
    }
}
