using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownUInt32Struct : Struct {
        private readonly int _valueAddr;

        public UnknownUInt32Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _valueAddr = Address + 0x00; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_valueAddr), displayName: "UInt32 Value", displayFormat: "X8")]
        public uint Value {
            get => (uint) Data.GetDouble(_valueAddr);
            set => Data.SetDouble(_valueAddr, (int) value);
        }
    }
}
