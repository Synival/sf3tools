using SF3.Models.Structs.X1;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1 {
    public class TreasureTable : Table<Treasure> {
        public TreasureTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Treasure(Data, id, name, address),
                (rows, model) => model.Searched != 0xFFFF);

        public override int? MaxSize => 255;
    }
}
