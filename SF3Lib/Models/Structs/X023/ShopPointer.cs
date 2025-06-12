using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X023 {
    public class ShopPointer : Struct {
        private readonly int _shopAddr;

        public ShopPointer(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _shopAddr = Address + 0x00; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_shopAddr), displayName: "Shop", isPointer: true, minWidth: 150)]
        public uint Shop {
            get => (uint) Data.GetDouble(_shopAddr);
            set => Data.SetDouble(_shopAddr, (int) value);
        }
    }
}
