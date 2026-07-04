using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownUInt16Struct : Struct {
        private readonly int _valueAddr;

        public UnknownUInt16Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
            _valueAddr = Address + 0x00; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_valueAddr), displayName: "UInt16 Value", displayFormat: "X4")]
        public ushort Value {
            get => Data.GetUInt16(_valueAddr);
            set => Data.SetUInt16(_valueAddr, value);
        }
    }
}
