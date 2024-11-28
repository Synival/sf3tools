using SF3.RawEditors;
using SF3.Models.X1.Town;

namespace SF3.TableModels.X1.Town {
    public class EnterTable : Table<Enter> {
        public EnterTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Enter(Editor, id, name, address),
                (rows, models) => models.Entered != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
