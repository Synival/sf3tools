using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class CharacterBattleSc3Models : Struct {
        private readonly int _modelFileIdAddr;
        private readonly int _modelFileUnknownHeaderAddrAddr;
        private readonly int _modelIndicesAvailableAddr;

        public CharacterBattleSc3Models(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _modelFileIdAddr                = Address + 0x00; // 4 bytes
            _modelFileUnknownHeaderAddrAddr = Address + 0x04; // 4 bytes
            _modelIndicesAvailableAddr      = Address + 0x08; // 4 bytes
        }

        private bool GetModelIndexAvailable(int promotionWeaponIndex)
            => ((Data.GetDouble(_modelIndicesAvailableAddr) >> promotionWeaponIndex) & 0x01) == 0x01;

        private void SetModelIndexAvailable(int promotionWeaponIndex, bool value)
            => Data.SetDouble(_modelIndicesAvailableAddr, value ? (Data.GetDouble(_modelIndicesAvailableAddr) | (0x01 << promotionWeaponIndex)) : (Data.GetDouble(_modelIndicesAvailableAddr) & ~(0x01 << promotionWeaponIndex)));

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int ModelFileID {
            get => Data.GetDouble(_modelFileIdAddr);
            set => Data.SetDouble(_modelFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public uint ModelFileUnknownHeaderAddr {
            get => (uint) Data.GetDouble(_modelFileUnknownHeaderAddrAddr);
            set => Data.SetDouble(_modelFileUnknownHeaderAddrAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 2)]
        [BulkCopy]
        public bool UWp1 {
            get => GetModelIndexAvailable(0);
            set => SetModelIndexAvailable(0, value);
        }

        [TableViewModelColumn(displayOrder: 3)]
        [BulkCopy]
        public bool UWp2 {
            get => GetModelIndexAvailable(1);
            set => SetModelIndexAvailable(1, value);
        }

        [TableViewModelColumn(displayOrder: 4)]
        [BulkCopy]
        public bool UWp3 {
            get => GetModelIndexAvailable(2);
            set => SetModelIndexAvailable(2, value);
        }

        [TableViewModelColumn(displayOrder: 5)]
        [BulkCopy]
        public bool UWp4 {
            get => GetModelIndexAvailable(3);
            set => SetModelIndexAvailable(3, value);
        }

        [TableViewModelColumn(displayOrder: 6)]
        [BulkCopy]
        public bool P1Wp1 {
            get => GetModelIndexAvailable(4);
            set => SetModelIndexAvailable(4, value);
        }

        [TableViewModelColumn(displayOrder: 7)]
        [BulkCopy]
        public bool P1Wp2 {
            get => GetModelIndexAvailable(5);
            set => SetModelIndexAvailable(5, value);
        }

        [TableViewModelColumn(displayOrder: 8)]
        [BulkCopy]
        public bool P1Wp3 {
            get => GetModelIndexAvailable(6);
            set => SetModelIndexAvailable(6, value);
        }

        [TableViewModelColumn(displayOrder: 9)]
        [BulkCopy]
        public bool P1Wp4 {
            get => GetModelIndexAvailable(7);
            set => SetModelIndexAvailable(7, value);
        }

        [TableViewModelColumn(displayOrder: 10)]
        [BulkCopy]
        public bool P2Wp1 {
            get => GetModelIndexAvailable(8);
            set => SetModelIndexAvailable(8, value);
        }

        [TableViewModelColumn(displayOrder: 11)]
        [BulkCopy]
        public bool P2Wp2 {
            get => GetModelIndexAvailable(9);
            set => SetModelIndexAvailable(9, value);
        }

        [TableViewModelColumn(displayOrder: 12)]
        [BulkCopy]
        public bool P2Wp3 {
            get => GetModelIndexAvailable(10);
            set => SetModelIndexAvailable(10, value);
        }

        [TableViewModelColumn(displayOrder: 13)]
        [BulkCopy]
        public bool P2Wp4 {
            get => GetModelIndexAvailable(11);
            set => SetModelIndexAvailable(11, value);
        }
    }
}
