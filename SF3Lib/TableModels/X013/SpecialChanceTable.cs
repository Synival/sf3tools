using SF3.RawEditors;
using SF3.Models.X013;

namespace SF3.TableModels.X013 {
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
