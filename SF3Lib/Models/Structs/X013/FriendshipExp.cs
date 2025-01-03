using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class FriendshipExp : Struct {
        private readonly int sLvl0;
        private readonly int sLvl1;
        private readonly int sLvl2;
        private readonly int sLvl3;
        private readonly int sLvl4;

        public FriendshipExp(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            sLvl0 = Address;     // 1 byte
            sLvl1 = Address + 1; // 1 byte
            sLvl2 = Address + 2; // 1 byte
            sLvl3 = Address + 3; // 1 byte
            sLvl4 = Address + 4; // 1 byte
        }

        [BulkCopy]
        public int SLvl0 {
            get => Data.GetByte(sLvl0);
            set => Data.SetByte(sLvl0, (byte) value);
        }

        [BulkCopy]
        public int SLvl1 {
            get => Data.GetByte(sLvl1);
            set => Data.SetByte(sLvl1, (byte) value);
        }

        [BulkCopy]
        public int SLvl2 {
            get => Data.GetByte(sLvl2);
            set => Data.SetByte(sLvl2, (byte) value);
        }

        [BulkCopy]
        public int SLvl3 {
            get => Data.GetByte(sLvl3);
            set => Data.SetByte(sLvl3, (byte) value);
        }

        [BulkCopy]
        public int SLvl4 {
            get => Data.GetByte(sLvl4);
            set => Data.SetByte(sLvl4, (byte) value);
        }
    }
}
