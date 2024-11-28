using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Structs
{
    public class UnknownUInt32Struct : Struct {
        public UnknownUInt32Struct(IRawEditor editor, int id, string name, int address)
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
