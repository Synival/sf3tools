using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1 {
    public class ModelInstanceGroup : Struct {
        private readonly int _modelIstanceTablePtr;
        private readonly int _matrixTablePtrAddr;
        private readonly int _posXAddr;
        private readonly int _posYAddr;
        private readonly int _posZAddr;

        public ModelInstanceGroup(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x14) {
            _modelIstanceTablePtr = Address + 0x00; // 4 bytes
            _matrixTablePtrAddr   = Address + 0x04; // 4 bytes
            _posXAddr             = Address + 0x08; // 4 bytes
            _posYAddr             = Address + 0x0C; // 4 bytes
            _posZAddr             = Address + 0x10; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "ModelsPtr", isPointer: true)]
        [BulkCopy]
        public uint ModelInstanceTablePtr {
            get => (uint) Data.GetDouble(_modelIstanceTablePtr);
            set => Data.SetDouble(_modelIstanceTablePtr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "MatricesPtr", isPointer: true)]
        [BulkCopy]
        public uint MatrixTablePtr {
            get => (uint) Data.GetDouble(_matrixTablePtrAddr);
            set => Data.SetDouble(_matrixTablePtrAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public int PosX {
            get => Data.GetDouble(_posXAddr);
            set => Data.SetDouble(_posXAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public int PosY {
            get => Data.GetDouble(_posYAddr);
            set => Data.SetDouble(_posYAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public int PosZ {
            get => Data.GetDouble(_posZAddr);
            set => Data.SetDouble(_posZAddr, value);
        }
    }
}
