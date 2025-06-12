using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X023 {
    public class ShopHaggle : Struct {
        private readonly int _itemAddr;
        private readonly int _flagAddr;

        public ShopHaggle(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            _itemAddr = Address + 0x00; // 4 bytes
            _flagAddr = Address + 0x04; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, addressField: nameof(_itemAddr), minWidth: 150)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Item {
            get => Data.GetDouble(_itemAddr);
            set => Data.SetDouble(_itemAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, addressField: nameof(_flagAddr), minWidth: 250)]
        [NameGetter(NamedValueType.GameFlag)]
        [BulkCopy]
        public int Flag {
            get => Data.GetDouble(_flagAddr);
            set => Data.SetDouble(_flagAddr, value);
        }
    }
}
