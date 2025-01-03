using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownUInt32Struct : Struct {
        public UnknownUInt32Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "UInt32 Value", displayFormat: "X8")]
        public uint Value {
            get => (uint) Data.GetDouble(Address);
            set => Data.SetDouble(Address, (int) value);
        }
    }
}
