using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class SupportStats : Struct {
        private readonly int sLvlStat1;
        private readonly int sLvlStat2;
        private readonly int sLvlStat3;
        private readonly int sLvlStat4;

        public SupportStats(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            sLvlStat1 = Address;     // 1 byte
            sLvlStat2 = Address + 1; // 1 byte
            sLvlStat3 = Address + 2; // 1 byte
            sLvlStat4 = Address + 3; // 1 byte
        }

        [TableViewModelColumn(displayOrder: 0)]
        [BulkCopy]
        public int SLvlStat1 {
            get => Data.GetByte(sLvlStat1);
            set => Data.SetByte(sLvlStat1, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1)]
        [BulkCopy]
        public int SLvlStat2 {
            get => Data.GetByte(sLvlStat2);
            set => Data.SetByte(sLvlStat2, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2)]
        [BulkCopy]
        public int SLvlStat3 {
            get => Data.GetByte(sLvlStat3);
            set => Data.SetByte(sLvlStat3, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3)]
        [BulkCopy]
        public int SLvlStat4 {
            get => Data.GetByte(sLvlStat4);
            set => Data.SetByte(sLvlStat4, (byte) value);
        }
    }
}
