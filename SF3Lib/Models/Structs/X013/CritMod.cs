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
        public sbyte Advantage {
            get => Data.GetInt8(_advantageAddr);
            set => Data.SetInt8(_advantageAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_disadvantageAddr), displayOrder: 1)]
        [BulkCopy]
        public sbyte Disadvantage {
            get => Data.GetInt8(_disadvantageAddr);
            set => Data.SetInt8(_disadvantageAddr, value);
        }
    }
}
