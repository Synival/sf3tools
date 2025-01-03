using SF3.Models.Structs.IconPointer;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.IconPointer {
    public class ItemIconTable : Table<ItemIcon> {
        protected ItemIconTable(IByteData data, string resourceFile, int address, bool has16BitIconAddr)
        : base(data, resourceFile, address) {
            Has16BitIconAddr = has16BitIconAddr;
        }

        public static ItemIconTable Create(IByteData data, string resourceFile, int address, bool has16BitIconAddr) {
            var newTable = new ItemIconTable(data, resourceFile, address, has16BitIconAddr);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ItemIcon(Data, id, name, address, Has16BitIconAddr));

        public override int? MaxSize => 300;

        public bool Has16BitIconAddr { get; }
    }
}
