using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class HealExp : Struct {
        private readonly int healExp;

        public HealExp(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            healExp = Address; // 1 byte
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Heal Exp Bonus")]
        [BulkCopy]
        public int HealBonus {
            get => Data.GetByte(healExp);
            set => Data.SetByte(healExp, (byte) value);
        }
    }
}
