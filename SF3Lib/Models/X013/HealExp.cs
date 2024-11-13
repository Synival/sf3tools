using CommonLib.Attributes;
using SF3.StreamEditors;

namespace SF3.Models.X013 {
    public class HealExp : Model {
        private readonly int healExp;

        public HealExp(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            healExp = Address; // 1 byte
        }

        [BulkCopy]
        public int HealBonus {
            get => Editor.GetByte(healExp);
            set => Editor.SetByte(healExp, (byte) value);
        }
    }
}
