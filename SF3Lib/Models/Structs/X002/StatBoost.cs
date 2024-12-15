using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X002 {
    public class StatBoost : Struct {
        private readonly int stat;

        public StatBoost(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            stat = Address; // 1 byte
        }

        [BulkCopy]
        public int Stat {
            get => Data.GetByte(stat);
            set => Data.SetByte(stat, (byte) value);
        }
    }
}
