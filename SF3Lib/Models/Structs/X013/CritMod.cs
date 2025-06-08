using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class CritMod : Struct {
        private readonly int _advantageAddr;
        private readonly int _disadvantageAddr;

        public CritMod(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x12) {
            _advantageAddr    = Address + 0x01; // 1 byte
            _disadvantageAddr = Address + 0x11; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_advantageAddr), displayOrder: 0)]
        [BulkCopy]
        public int Advantage {
            get => (sbyte) Data.GetByte(_advantageAddr);
            set => Data.SetByte(_advantageAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_disadvantageAddr), displayOrder: 1)]
        [BulkCopy]
        public int Disadvantage {
            get => (sbyte) Data.GetByte(_disadvantageAddr);
            set => Data.SetByte(_disadvantageAddr, (byte) value);
        }
    }
}
