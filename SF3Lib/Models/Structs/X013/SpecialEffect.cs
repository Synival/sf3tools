using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawEditors;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SpecialEffect : Struct {
        private readonly int specialAddress;

        public SpecialEffect(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            specialAddress  = Address; // 1 byte
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special {
            get => Editor.GetByte(specialAddress);
            set => Editor.SetByte(specialAddress, (byte) value);
        }
    }
}
