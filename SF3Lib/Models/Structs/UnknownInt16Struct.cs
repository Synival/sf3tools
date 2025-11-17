using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownInt16Struct : Struct {
        private readonly int _valueAddr;

        public UnknownInt16Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
            _valueAddr = Address + 0x00; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_valueAddr), displayName: "Int16 Value")]
        public short Value {
            get => (short) Data.GetWord(_valueAddr);
            set => Data.SetWord(_valueAddr, value);
        }
    }
}
