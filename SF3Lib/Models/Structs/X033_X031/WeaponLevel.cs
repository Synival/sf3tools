using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X033_X031 {
    public class WeaponLevel : Struct {
        private readonly int _wlevel1Addr;
        private readonly int _wlevel2Addr;
        private readonly int _wlevel3Addr;
        private readonly int _wlevel4Addr;

        public WeaponLevel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x11) {
            _wlevel1Addr = Address + 0x02; // 4 bytes
            _wlevel2Addr = Address + 0x06; // 4 bytes
            _wlevel3Addr = Address + 0x0a; // 4 bytes
            _wlevel4Addr = Address + 0x0e; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_wlevel1Addr), displayOrder: 0, displayName: "Weapon Level 1 Exp")]
        [BulkCopy]
        public int WLevel1 {
            get => Data.GetWord(_wlevel1Addr);
            set => Data.SetWord(_wlevel1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_wlevel2Addr), displayOrder: 1, displayName: "Weapon Level 2 Exp")]
        [BulkCopy]
        public int WLevel2 {
            get => Data.GetWord(_wlevel2Addr);
            set => Data.SetWord(_wlevel2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_wlevel3Addr), displayOrder: 2, displayName: "Weapon Level 3 Exp")]
        [BulkCopy]
        public int WLevel3 {
            get => Data.GetWord(_wlevel3Addr);
            set => Data.SetWord(_wlevel3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_wlevel4Addr), displayOrder: 3, displayName: "Weapon Level 4 Exp")]
        [BulkCopy]
        public int WLevel4 {
            get => Data.GetWord(_wlevel4Addr);
            set => Data.SetWord(_wlevel4Addr, value);
        }
    }
}
