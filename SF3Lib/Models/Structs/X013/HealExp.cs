using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class HealExp : Struct {
        private readonly int healExp;

        public HealExp(IRawData editor, int id, string name, int address)
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
