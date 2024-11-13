using SF3.RawEditors;
using SF3.Models.X1.Town;

namespace SF3.Tables.X1.Town {
    public class EnterTable : Table<Enter> {
        public EnterTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Enter(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.Entered != 0xffff);

        public override int? MaxSize => 100;
    }
}
