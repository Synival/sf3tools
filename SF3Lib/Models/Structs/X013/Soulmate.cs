using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class Soulmate : Struct {
        private readonly int chance;

        public Soulmate(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            chance = Address; // 2 bytes
        }

        [BulkCopy]
        public int Chance {
            get => Data.GetByte(chance);
            set => Data.SetByte(chance, (byte) value);
        }
    }
}
