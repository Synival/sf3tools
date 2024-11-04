using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class WeaponSpellRankTable : Table<WeaponSpellRank> {
        public WeaponSpellRankTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpellRank(FileEditor, id, name, address));

        public override int? MaxSize => 4;
    }
}
