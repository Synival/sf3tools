using SF3.RawEditors;
using SF3.Models.X013;

namespace SF3.Tables.X013 {
    public class SpecialEffectTable : Table<SpecialEffect> {
        public SpecialEffectTable(IRawEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpecialEffect(FileEditor, id, name, address));

        public override int? MaxSize => 500;
    }
}
