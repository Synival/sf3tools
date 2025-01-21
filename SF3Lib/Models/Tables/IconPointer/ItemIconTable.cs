using System;
using SF3.ByteData;
using SF3.Models.Structs.IconPointer;

namespace SF3.Models.Tables.IconPointer {
    public class ItemIconTable : ResourceTable<ItemIcon> {
        protected ItemIconTable(IByteData data, string name, string resourceFile, int address, bool has16BitIconAddr)
        : base(data, name, resourceFile, address, 300) {
            Has16BitIconAddr = has16BitIconAddr;
        }

        public static ItemIconTable Create(IByteData data, string name, string resourceFile, int address, bool has16BitIconAddr) {
            var newTable = new ItemIconTable(data, name, resourceFile, address, has16BitIconAddr);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new ItemIcon(Data, id, name, address, Has16BitIconAddr));

        public bool Has16BitIconAddr { get; }
    }
}
