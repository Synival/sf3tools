using SF3.FileEditors;
using SF3.Models.X1.Battle;

namespace SF3.Tables.X1.Battle {
    public class BattlePointersTable : Table<BattlePointers> {
        public BattlePointersTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new BattlePointers(FileEditor, id, name, address));

        public override int? MaxSize => 5;
    }
}
