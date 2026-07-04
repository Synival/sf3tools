using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X002 {
    public class WeaponRank : Struct {
        private readonly int _skill0Addr;
        private readonly int _skill1Addr;
        private readonly int _skill2Addr;
        private readonly int _skill3Addr;

        public WeaponRank(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _skill0Addr = Address;     // 1 byte
            _skill1Addr = Address + 1; // 1 byte
            _skill2Addr = Address + 2; // 1 byte
            _skill3Addr = Address + 3; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_skill0Addr), displayOrder: 0, displayName: "Skill0 Atk+")]
        [BulkCopy]
        public byte Skill0 {
            get => Data.GetUInt8(_skill0Addr);
            set => Data.SetUInt8(_skill0Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_skill1Addr), displayOrder: 1, displayName: "Skill1 Atk+")]
        [BulkCopy]
        public byte Skill1 {
            get => Data.GetUInt8(_skill1Addr);
            set => Data.SetUInt8(_skill1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_skill2Addr), displayOrder: 2, displayName: "Skill2 Atk+")]
        [BulkCopy]
        public byte Skill2 {
            get => Data.GetUInt8(_skill2Addr);
            set => Data.SetUInt8(_skill2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_skill3Addr), displayOrder: 0, displayName: "Skill3 Atk+")]
        [BulkCopy]
        public byte Skill3 {
            get => Data.GetUInt8(_skill3Addr);
            set => Data.SetUInt8(_skill3Addr, value);
        }
    }
}
