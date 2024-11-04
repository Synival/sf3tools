using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
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
