using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class Soulfail : Struct {
        private readonly int expLost;

        public Soulfail(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            expLost = Address; // 1 bytes
        }

        [BulkCopy]
        public int ExpLost {
            get => Data.GetByte(expLost);
            set => Data.SetByte(expLost, (byte) value);
        }
    }
}
