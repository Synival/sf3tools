using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SpecialEffect : Struct {
        private readonly int specialAddress;

        public SpecialEffect(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            specialAddress  = Address; // 1 byte
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special {
            get => Data.GetByte(specialAddress);
            set => Data.SetByte(specialAddress, (byte) value);
        }
    }
}
