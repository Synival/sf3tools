using SF3.RawEditors;
using SF3.Models.X019;

namespace SF3.Tables.X019 {
    public class MonsterTable : Table<Monster> {
        public MonsterTable(IRawEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Monster(FileEditor, id, name, address));

        public override int? MaxSize => 256;
    }
}
