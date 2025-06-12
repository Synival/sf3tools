using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X023 {
    public class ShopItem : Struct {
        private readonly int _itemAddr;

        public ShopItem(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _itemAddr = Address + 0x00; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_itemAddr), displayName: "Item", minWidth: 150)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public uint Item {
            get => (uint) Data.GetDouble(_itemAddr);
            set => Data.SetDouble(_itemAddr, (int) value);
        }
    }
}
