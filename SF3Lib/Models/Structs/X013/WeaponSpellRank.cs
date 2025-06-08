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
        public int RankNone {
            get => Data.GetByte(_rankNoneAddr);
            set => Data.SetByte(_rankNoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_rankCAddr), displayOrder: 0)]
        [BulkCopy]
        public int RankC {
            get => Data.GetByte(_rankCAddr);
            set => Data.SetByte(_rankCAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_rankBAddr), displayOrder: 1)]
        [BulkCopy]
        public int RankB {
            get => Data.GetByte(_rankBAddr);
            set => Data.SetByte(_rankBAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_rankAAddr), displayOrder: 2)]
        [BulkCopy]
        public int RankA {
            get => Data.GetByte(_rankAAddr);
            set => Data.SetByte(_rankAAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_rankSAddr), displayOrder: 3)]
        [BulkCopy]
        public int RankS {
            get => Data.GetByte(_rankSAddr);
            set => Data.SetByte(_rankSAddr, (byte) value);
        }
    }
}
