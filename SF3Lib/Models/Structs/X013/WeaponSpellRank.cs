using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class WeaponSpellRank : Struct {
        private readonly int _rankNoneAddr;
        private readonly int _rankCAddr;
        private readonly int _rankBAddr;
        private readonly int _rankAAddr;
        private readonly int _rankSAddr;

        public WeaponSpellRank(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x05) {
            _rankNoneAddr = Address;     // 1 byte
            _rankCAddr    = Address + 1; // 1 byte
            _rankBAddr    = Address + 2; // 1 byte
            _rankAAddr    = Address + 3; // 1 byte
            _rankSAddr    = Address + 4; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_rankNoneAddr), displayOrder: 0)]
        [BulkCopy]
        public byte RankNone {
            get => Data.GetUInt8(_rankNoneAddr);
            set => Data.SetUInt8(_rankNoneAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rankCAddr), displayOrder: 0)]
        [BulkCopy]
        public byte RankC {
            get => Data.GetUInt8(_rankCAddr);
            set => Data.SetUInt8(_rankCAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rankBAddr), displayOrder: 1)]
        [BulkCopy]
        public byte RankB {
            get => Data.GetUInt8(_rankBAddr);
            set => Data.SetUInt8(_rankBAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rankAAddr), displayOrder: 2)]
        [BulkCopy]
        public byte RankA {
            get => Data.GetUInt8(_rankAAddr);
            set => Data.SetUInt8(_rankAAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rankSAddr), displayOrder: 3)]
        [BulkCopy]
        public byte RankS {
            get => Data.GetUInt8(_rankSAddr);
            set => Data.SetUInt8(_rankSAddr, value);
        }
    }
}
