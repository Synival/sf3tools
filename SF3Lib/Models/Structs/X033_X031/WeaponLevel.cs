using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X033_X031 {
    public class WeaponLevel : Struct {
        private readonly int level1;
        private readonly int level2;
        private readonly int level3;
        private readonly int level4;

        public WeaponLevel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x11) {
            level1 = Address + 0x02; // 4 bytes
            level2 = Address + 0x06; // 4 bytes
            level3 = Address + 0x0a; // 4 bytes
            level4 = Address + 0x0e; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Weapon Level 1 Exp")]
        [BulkCopy]
        public int WLevel1 {
            get => Data.GetWord(level1);
            set => Data.SetWord(level1, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "Weapon Level 2 Exp")]
        [BulkCopy]
        public int WLevel2 {
            get => Data.GetWord(level2);
            set => Data.SetWord(level2, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "Weapon Level 3 Exp")]
        [BulkCopy]
        public int WLevel3 {
            get => Data.GetWord(level3);
            set => Data.SetWord(level3, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "Weapon Level 4 Exp")]
        [BulkCopy]
        public int WLevel4 {
            get => Data.GetWord(level4);
            set => Data.SetWord(level4, value);
        }
    }
}
