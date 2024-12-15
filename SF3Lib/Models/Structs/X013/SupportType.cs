using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SupportType : Struct {
        private readonly int supportA;
        private readonly int supportB;

        public SupportType(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
            supportA = Address;     // 1 byte
            supportB = Address + 1; // 1 byte
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportA {
            get => Data.GetByte(supportA);
            set => Data.SetByte(supportA, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportB {
            get => Data.GetByte(supportB);
            set => Data.SetByte(supportB, (byte) value);
        }
    }
}
