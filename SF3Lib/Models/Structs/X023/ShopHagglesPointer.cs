using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X023 {
    public class ShopHagglesPointer : Struct {
        private readonly int _shopHagglesAddr;

        public ShopHagglesPointer(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _shopHagglesAddr = Address + 0x00; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_shopHagglesAddr), isPointer: true, minWidth: 150)]
        [BulkCopy]
        public uint ShopHaggles {
            get => (uint) Data.GetDouble(_shopHagglesAddr);
            set => Data.SetDouble(_shopHagglesAddr, (int) value);
        }
    }
}
