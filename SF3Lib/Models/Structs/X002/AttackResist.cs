using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X002 {
    public class AttackResist : Struct {
        private readonly int _attackAddr;
        private readonly int _resistAddr;

        public AttackResist(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0xD3) {
            _attackAddr = Address;        // 1 byte
            _resistAddr = Address + 0xd2; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_attackAddr), displayOrder: 0, displayName: "Attack Atk+")]
        [BulkCopy]
        public int Attack {
            get => Data.GetByte(_attackAddr);
            set => Data.SetByte(_attackAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_resistAddr), displayOrder: 1, displayName: "Resist MDef+")]
        [BulkCopy]
        public int Resist {
            get => Data.GetByte(_resistAddr);
            set => Data.SetByte(_resistAddr, (byte) value);
        }
    }
}
