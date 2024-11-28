using SF3.Models.Structs.X1;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1 {
    public class TreasureTable : Table<Treasure> {
        public TreasureTable(IRawData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Treasure(Data, id, name, address),
                (rows, model) => model.Searched != 0xFFFF);

        public override int? MaxSize => 255;
    }
}
