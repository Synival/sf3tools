using SF3.FileEditors;
using SF3.Models;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class WeaponSpellTable : Table<WeaponSpell> {
        public WeaponSpellTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpell(FileEditor, id, name, address));

        public override int? MaxSize => 31;
    }
}
