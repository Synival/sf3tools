using CommonLib.Attributes;
using SF3.RawData;

namespace SF3.Models.Structs {
    public class UnknownUInt32Struct : Struct {
        public UnknownUInt32Struct(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "UInt32 Value", displayFormat: "X8")]
        public uint Value {
            get => (uint) Editor.GetDouble(Address);
            set => Editor.SetDouble(Address, (int) value);
        }
    }
}
