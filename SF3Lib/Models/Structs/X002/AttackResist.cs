using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X002 {
    public class AttackResist : Struct {
        private readonly int attack;
        private readonly int resist;

        public AttackResist(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0xD3) {
            attack = Address;        // 1 byte
            resist = Address + 0xd2; // 1 byte
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Attack Atk+")]
        [BulkCopy]
        public int Attack {
            get => Data.GetByte(attack);
            set => Data.SetByte(attack, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "Resist MDef+")]
        [BulkCopy]
        public int Resist {
            get => Data.GetByte(resist);
            set => Data.SetByte(resist, (byte) value);
        }
    }
}
