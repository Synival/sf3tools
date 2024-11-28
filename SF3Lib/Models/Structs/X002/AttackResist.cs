using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X002 {
    public class AttackResist : Struct {
        private readonly int attack;
        private readonly int resist;

        public AttackResist(IRawData data, int id, string name, int address)
        : base(data, id, name, address, 0xD3) {
            attack = Address;        // 1 byte
            resist = Address + 0xd2; // 1 byte
        }

        [BulkCopy]
        public int Attack {
            get => Data.GetByte(attack);
            set => Data.SetByte(attack, (byte) value);
        }

        [BulkCopy]
        public int Resist {
            get => Data.GetByte(resist);
            set => Data.SetByte(resist, (byte) value);
        }
    }
}
