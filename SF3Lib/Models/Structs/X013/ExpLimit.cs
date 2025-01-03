using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class ExpLimit : Struct {
        private readonly int expCheck;
        private readonly int expReplacement;

        public ExpLimit(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x07) {
            expCheck       = Address;     // 1 byte
            expReplacement = Address + 6; // 1 byte
        }

        [BulkCopy]
        public int ExpCheck {
            get => Data.GetByte(expCheck);
            set => Data.SetByte(expCheck, (byte) value);
        }

        [BulkCopy]
        public int ExpReplacement {
            get => Data.GetByte(expReplacement);
            set => Data.SetByte(expReplacement, (byte) value);
        }
    }
}
