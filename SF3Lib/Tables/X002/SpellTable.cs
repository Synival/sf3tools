using SF3.RawEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class SpellTable : Table<Spell> {
        public SpellTable(IRawEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Spell(FileEditor, id, name, address));

        public override int? MaxSize => 78;
    }
}
