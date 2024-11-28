using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X013 {
    public class SpecialChanceTable : Table<SpecialChance> {
        public SpecialChanceTable(IRawEditor editor, string resourceFile, int address, bool hasLargeTable) : base(editor, resourceFile, address) {
            HasLargeTable = hasLargeTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpecialChance(Editor, id, name, address, HasLargeTable));

        public override int? MaxSize => 1;

        public bool HasLargeTable { get; }
    }
}
