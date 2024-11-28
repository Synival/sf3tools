using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X013 {
    public class SpecialEffectTable : Table<SpecialEffect> {
        public SpecialEffectTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpecialEffect(Editor, id, name, address));

        public override int? MaxSize => 500;
    }
}
