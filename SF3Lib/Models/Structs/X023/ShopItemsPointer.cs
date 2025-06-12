using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X023 {
    public class ShopItemsPointer : Struct {
        private readonly int _shopItemsAddr;

        public ShopItemsPointer(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _shopItemsAddr = Address + 0x00; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_shopItemsAddr), isPointer: true, minWidth: 150)]
        [BulkCopy]
        public uint ShopItems {
            get => (uint) Data.GetDouble(_shopItemsAddr);
            set => Data.SetDouble(_shopItemsAddr, (int) value);
        }
    }
}
