using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class FriendshipExp : Struct {
        private readonly int _sLvl0Addr;
        private readonly int _sLvl1Addr;
        private readonly int _sLvl2Addr;
        private readonly int _sLvl3Addr;
        private readonly int _sLvl4Addr;

        public FriendshipExp(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x05) {
            _sLvl0Addr = Address + 0x00; // 1 byte
            _sLvl1Addr = Address + 0x01; // 1 byte
            _sLvl2Addr = Address + 0x02; // 1 byte
            _sLvl3Addr = Address + 0x03; // 1 byte
            _sLvl4Addr = Address + 0x04; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_sLvl0Addr), displayOrder: 0, displayName: "Ally")]
        [BulkCopy]
        public int SLvl0_Ally {
            get => Data.GetByte(_sLvl0Addr);
            set => Data.SetByte(_sLvl0Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvl1Addr), displayOrder: 0, displayName: "Partner")]
        [BulkCopy]
        public int SLvl1_Partner {
            get => Data.GetByte(_sLvl1Addr);
            set => Data.SetByte(_sLvl1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvl2Addr), displayOrder: 0, displayName: "Friend")]
        [BulkCopy]
        public int SLvl2_Friend {
            get => Data.GetByte(_sLvl2Addr);
            set => Data.SetByte(_sLvl2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvl3Addr), displayOrder: 0, displayName: "Trusted")]
        [BulkCopy]
        public int SLvl3_Trusted {
            get => Data.GetByte(_sLvl3Addr);
            set => Data.SetByte(_sLvl3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sLvl4Addr), displayOrder: 0, displayName: "Soulmate")]
        [BulkCopy]
        public int SLvl4_Soulmate {
            get => Data.GetByte(_sLvl4Addr);
            set => Data.SetByte(_sLvl4Addr, (byte) value);
        }
    }
}
