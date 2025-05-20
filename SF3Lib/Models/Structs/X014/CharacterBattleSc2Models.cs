using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class CharacterBattleSc2Models : Struct {
        private readonly int _modelFileIdAddr;
        private readonly int _modelIndicesAddr;

        public CharacterBattleSc2Models(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _modelFileIdAddr  = Address + 0x00; // 4 bytes
            _modelIndicesAddr = Address + 0x04; // 4 bytes (8 "nibbles")
        }

        private byte GetModelIndex(int promotionWeaponIndex)
            => (byte) ((Data.GetDouble(_modelIndicesAddr) >> (promotionWeaponIndex * 4)) & 0x0F);

        private void SetModelIndex(int promotionWeaponIndex, byte value)
            => Data.SetDouble(_modelIndicesAddr, Data.GetDouble(_modelIndicesAddr) & ~(0x0F << (promotionWeaponIndex * 4)) | ((value & 0x0F) << (promotionWeaponIndex * 4)));

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int ModelFileID {
            get => Data.GetDouble(_modelFileIdAddr);
            set => Data.SetDouble(_modelFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp1 {
            get => GetModelIndex(0);
            set => SetModelIndex(0, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp2 {
            get => GetModelIndex(1);
            set => SetModelIndex(1, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp3 {
            get => GetModelIndex(2);
            set => SetModelIndex(2, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp4 {
            get => GetModelIndex(3);
            set => SetModelIndex(3, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp1 {
            get => GetModelIndex(4);
            set => SetModelIndex(4, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp2 {
            get => GetModelIndex(5);
            set => SetModelIndex(5, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp3 {
            get => GetModelIndex(6);
            set => SetModelIndex(6, value);
        }

        [TableViewModelColumn(displayOrder: 8, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp4 {
            get => GetModelIndex(7);
            set => SetModelIndex(7, value);
        }
    }
}
