using SF3.Models.Structs.X019;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X019 {
    public class MonsterTable : Table<Monster> {
        public MonsterTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Monster(Data, id, name, address));

        public override int? MaxSize => 256;
    }
}
