using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X002 {
    public class WeaponRank : Struct {
        private readonly int skill0;
        private readonly int skill1;
        private readonly int skill2;
        private readonly int skill3;

        public WeaponRank(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            skill0 = Address;     // 1 byte
            skill1 = Address + 1; // 1 byte
            skill2 = Address + 2; // 1 byte
            skill3 = Address + 3; // 1 byte
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Skill0 Atk+")]
        [BulkCopy]
        public int Skill0 {
            get => Data.GetByte(skill0);
            set => Data.SetByte(skill0, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Skill1 Atk+")]
        [BulkCopy]
        public int Skill1 {
            get => Data.GetByte(skill1);
            set => Data.SetByte(skill1, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Skill2 Atk+")]
        [BulkCopy]
        public int Skill2 {
            get => Data.GetByte(skill2);
            set => Data.SetByte(skill2, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Skill3 Atk+")]
        [BulkCopy]
        public int Skill3 {
            get => Data.GetByte(skill3);
            set => Data.SetByte(skill3, (byte) value);
        }
    }
}
