using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownUInt8Struct : Struct {
        private readonly int _valueAddr;

        public UnknownUInt8Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            _valueAddr = Address + 0x00; // 1 byte
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_valueAddr), displayName: "UInt8 Value", displayFormat: "X2")]
        public byte Value {
            get => (byte) Data.GetByte(_valueAddr);
            set => Data.SetByte(_valueAddr, value);
        }
    }
}
