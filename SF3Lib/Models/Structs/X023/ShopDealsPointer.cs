using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X023 {
    public class ShopDealsPointer : Struct {
        private readonly int _shopDealsAddr;

        public ShopDealsPointer(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _shopDealsAddr = Address + 0x00; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_shopDealsAddr), isPointer: true, minWidth: 150)]
        [BulkCopy]
        public uint ShopDeals {
            get => (uint) Data.GetDouble(_shopDealsAddr);
            set => Data.SetDouble(_shopDealsAddr, (int) value);
        }
    }
}
