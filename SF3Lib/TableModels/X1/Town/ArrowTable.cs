using SF3.RawEditors;
using SF3.Structs.X1.Town;

namespace SF3.TableModels.X1.Town {
    public class ArrowTable : Table<Arrow> {
        public ArrowTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Arrow(Editor, id, name, address),
                (rows, model) => model.ArrowUnknown0 != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
