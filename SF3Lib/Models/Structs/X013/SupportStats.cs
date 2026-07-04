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
        public byte SLvlStat1 {
            get => Data.GetUInt8(_sLvlStat1Addr);
            set => Data.SetUInt8(_sLvlStat1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat2Addr), displayOrder: 1)]
        [BulkCopy]
        public byte SLvlStat2 {
            get => Data.GetUInt8(_sLvlStat2Addr);
            set => Data.SetUInt8(_sLvlStat2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat3Addr), displayOrder: 2)]
        [BulkCopy]
        public byte SLvlStat3 {
            get => Data.GetUInt8(_sLvlStat3Addr);
            set => Data.SetUInt8(_sLvlStat3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvlStat4Addr), displayOrder: 3)]
        [BulkCopy]
        public byte SLvlStat4 {
            get => Data.GetUInt8(_sLvlStat4Addr);
            set => Data.SetUInt8(_sLvlStat4Addr, value);
        }
    }
}
