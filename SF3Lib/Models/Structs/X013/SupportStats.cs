using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class SupportStats : Struct {
        private readonly int _sLvlStat1Addr;
        private readonly int _sLvlStat2Addr;
        private readonly int _sLvlStat3Addr;
        private readonly int _sLvlStat4Addr;

        public SupportStats(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _sLvlStat1Addr = Address;     // 1 byte
            _sLvlStat2Addr = Address + 1; // 1 byte
            _sLvlStat3Addr = Address + 2; // 1 byte
            _sLvlStat4Addr = Address + 3; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat1Addr), displayOrder: 0)]
        [BulkCopy]
        public int SLvlStat1 {
            get => Data.GetByte(_sLvlStat1Addr);
            set => Data.SetByte(_sLvlStat1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat2Addr), displayOrder: 1)]
        [BulkCopy]
        public int SLvlStat2 {
            get => Data.GetByte(_sLvlStat2Addr);
            set => Data.SetByte(_sLvlStat2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat3Addr), displayOrder: 2)]
        [BulkCopy]
        public int SLvlStat3 {
            get => Data.GetByte(_sLvlStat3Addr);
            set => Data.SetByte(_sLvlStat3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat4Addr), displayOrder: 3)]
        [BulkCopy]
        public int SLvlStat4 {
            get => Data.GetByte(_sLvlStat4Addr);
            set => Data.SetByte(_sLvlStat4Addr, (byte) value);
        }
    }
}
