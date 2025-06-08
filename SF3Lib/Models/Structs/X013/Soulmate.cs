using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class Soulmate : Struct {
        private readonly int _chanceAddr;

        public Soulmate(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            _chanceAddr = Address; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_chanceAddr), displayOrder: 0)]
        [BulkCopy]
        public int Chance {
            get => Data.GetByte(_chanceAddr);
            set => Data.SetByte(_chanceAddr, (byte) value);
        }
    }
}
