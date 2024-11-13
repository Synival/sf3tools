using SF3.RawEditors;
using SF3.Models.X013;

namespace SF3.Tables.X013 {
    public class MagicBonusTable : Table<MagicBonus> {
        public MagicBonusTable(IRawEditor editor, string resourceFile, int address, bool has32BitValues) : base(editor, resourceFile, address) {
            Has32BitValues = has32BitValues;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new MagicBonus(Editor, id, name, address, Has32BitValues));

        public override int? MaxSize => 256;

        public bool Has32BitValues { get; }
    }
}
