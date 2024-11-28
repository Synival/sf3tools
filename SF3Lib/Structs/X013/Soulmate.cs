using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Structs.X013 {
    public class Soulmate : Struct {
        private readonly int chance;

        public Soulmate(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            chance = Address; // 2 bytes
        }

        [BulkCopy]
        public int Chance {
            get => Editor.GetByte(chance);
            set => Editor.SetByte(chance, (byte) value);
        }
    }
}
