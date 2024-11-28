using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Models
{
    public class UnknownUInt16Model : Model {
        public UnknownUInt16Model(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x02) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "UInt16 Value", displayFormat: "X4")]
        public ushort Value {
            get => (ushort) Editor.GetWord(Address);
            set => Editor.SetWord(Address, value);
        }
    }
}
