using SF3.FileEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class WeaponRankTable : Table<WeaponRank> {
        public WeaponRankTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponRank(FileEditor, id, name, address));

        public override int? MaxSize => 5;
    }
}
