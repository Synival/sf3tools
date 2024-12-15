using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X002 {
    public class ItemTable : Table<Item> {
        public ItemTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Item(Data, id, name, address));

        public override int? MaxSize => 300;
    }
}
