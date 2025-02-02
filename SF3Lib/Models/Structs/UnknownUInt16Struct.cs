using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs {
    public class UnknownUInt16Struct : Struct {
        public UnknownUInt16Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "UInt16 Value", displayFormat: "X4")]
        public ushort Value {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }
    }
}
