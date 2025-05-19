using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class CharacterBattleSc3Model : Struct {
        private readonly int _modelFileIdAddr;
        private readonly int _unknownFileOffsetAddr;
        private readonly int _weaponModelMaskAddr;

        public CharacterBattleSc3Model(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _modelFileIdAddr       = Address + 0x00; // 4 bytes
            _unknownFileOffsetAddr = Address + 0x04; // 4 bytes
            _weaponModelMaskAddr   = Address + 0x08; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int ModelFileID {
            get => Data.GetDouble(_modelFileIdAddr);
            set => Data.SetDouble(_modelFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public uint UnknownFileOffset {
            get => (uint) Data.GetDouble(_unknownFileOffsetAddr);
            set => Data.SetDouble(_unknownFileOffsetAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public uint WeaponModelMask {
            get => (uint) Data.GetDouble(_weaponModelMaskAddr);
            set => Data.SetDouble(_weaponModelMaskAddr, (int) value);
        }
    }
}
