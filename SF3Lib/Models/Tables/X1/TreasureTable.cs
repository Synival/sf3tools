using SF3.Models.Structs.X1;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X1 {
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
