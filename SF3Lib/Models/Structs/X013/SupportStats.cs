using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class SupportStats : Struct {
        private readonly int sLvlStat1;
        private readonly int sLvlStat2;
        private readonly int sLvlStat3;
        private readonly int sLvlStat4;

        public SupportStats(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            sLvlStat1 = Address;     // 1 byte
            sLvlStat2 = Address + 1; // 1 byte
            sLvlStat3 = Address + 2; // 1 byte
            sLvlStat4 = Address + 3; // 1 byte
        }

        [BulkCopy]
        public int SLvlStat1 {
            get => Data.GetByte(sLvlStat1);
            set => Data.SetByte(sLvlStat1, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat2 {
            get => Data.GetByte(sLvlStat2);
            set => Data.SetByte(sLvlStat2, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat3 {
            get => Data.GetByte(sLvlStat3);
            set => Data.SetByte(sLvlStat3, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat4 {
            get => Data.GetByte(sLvlStat4);
            set => Data.SetByte(sLvlStat4, (byte) value);
        }
    }
}
