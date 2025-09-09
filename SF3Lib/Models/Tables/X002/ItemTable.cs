using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class ItemTable : ResourceTable<Item> {
        protected ItemTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static ItemTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new ItemTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Item(Data, id, name, address));
    }
}
