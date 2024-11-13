using SF3.StreamEditors;
using SF3.Models.X013;

namespace SF3.Tables.X013 {
    public class SpecialChanceTable : Table<SpecialChance> {
        public SpecialChanceTable(IByteEditor fileEditor, string resourceFile, int address, bool hasLargeTable) : base(fileEditor, resourceFile, address) {
            HasLargeTable = hasLargeTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpecialChance(FileEditor, id, name, address, HasLargeTable));

        public override int? MaxSize => 1;

        public bool HasLargeTable { get; }
    }
}
