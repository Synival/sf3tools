using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class HealExp : Struct {
        private readonly int _healExpAddr;

        public HealExp(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            _healExpAddr = Address; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_healExpAddr), displayOrder: 0, displayName: "Heal Exp Bonus")]
        [BulkCopy]
        public int HealBonus {
            get => Data.GetByte(_healExpAddr);
            set => Data.SetByte(_healExpAddr, (byte) value);
        }
    }
}
