using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class UnknownInt16Struct : Struct {
        public UnknownInt16Struct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Int16 Value")]
        public short Value {
            get => (short) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }
    }
}
