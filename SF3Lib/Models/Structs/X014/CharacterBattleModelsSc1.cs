using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class CharacterBattleModelsSc1 : Struct {
        private readonly int _modelUWp1FileIdAddr;
        private readonly int _modelUWp2FileIdAddr;
        private readonly int _modelUWp3FileIdAddr;
        private readonly int _modelUWp4FileIdAddr;
        private readonly int _modelPWp1FileIdAddr;
        private readonly int _modelPWp2FileIdAddr;
        private readonly int _modelPWp3FileIdAddr;
        private readonly int _modelPWp4FileIdAddr;

        public CharacterBattleModelsSc1(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x20) {
            _modelUWp1FileIdAddr = Address + 0x00; // 4 bytes
            _modelUWp2FileIdAddr = Address + 0x04; // 4 bytes
            _modelUWp3FileIdAddr = Address + 0x08; // 4 bytes
            _modelUWp4FileIdAddr = Address + 0x0C; // 4 bytes
            _modelPWp1FileIdAddr = Address + 0x10; // 4 bytes
            _modelPWp2FileIdAddr = Address + 0x14; // 4 bytes
            _modelPWp3FileIdAddr = Address + 0x18; // 4 bytes
            _modelPWp4FileIdAddr = Address + 0x1C; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int UWp1FileId {
            get => Data.GetDouble(_modelUWp1FileIdAddr);
            set => Data.SetDouble(_modelUWp1FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int UWp2FileId {
            get => Data.GetDouble(_modelUWp2FileIdAddr);
            set => Data.SetDouble(_modelUWp2FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int UWp3FileId {
            get => Data.GetDouble(_modelUWp3FileIdAddr);
            set => Data.SetDouble(_modelUWp3FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int UWp4FileId {
            get => Data.GetDouble(_modelUWp4FileIdAddr);
            set => Data.SetDouble(_modelUWp4FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int PWp1FileId {
            get => Data.GetDouble(_modelPWp1FileIdAddr);
            set => Data.SetDouble(_modelPWp1FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int PWp2FileId {
            get => Data.GetDouble(_modelPWp2FileIdAddr);
            set => Data.SetDouble(_modelPWp2FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int PWp3FileId {
            get => Data.GetDouble(_modelPWp3FileIdAddr);
            set => Data.SetDouble(_modelPWp3FileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [BulkCopy]
        public int PWp4FileId {
            get => Data.GetDouble(_modelPWp4FileIdAddr);
            set => Data.SetDouble(_modelPWp4FileIdAddr, value);
        }
    }
}
