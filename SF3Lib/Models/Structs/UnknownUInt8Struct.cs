using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownUInt8Struct : Struct {
        public UnknownUInt8Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(Address), displayName: "UInt8 Value", displayFormat: "X2")]
        public byte Value {
            get => (byte) Data.GetByte(Address);
            set => Data.SetByte(Address, value);
        }
    }
}
