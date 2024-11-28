using SF3.RawEditors;
using SF3.Models.X1;

namespace SF3.TableModels.X1 {
    public class TreasureTable : Table<Treasure> {
        public TreasureTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Treasure(Editor, id, name, address),
                (rows, model) => model.Searched != 0xFFFF);

        public override int? MaxSize => 255;
    }
}
