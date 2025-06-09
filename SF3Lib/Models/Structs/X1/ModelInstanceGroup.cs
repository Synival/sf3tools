using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1 {
    public class ModelInstanceGroup : Struct {
        private readonly int _modelInstanceTablePtrAddr;
        private readonly int _matrixTablePtrAddr;
        private readonly int _posXAddr;
        private readonly int _posYAddr;
        private readonly int _posZAddr;

        public ModelInstanceGroup(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x14) {
            _modelInstanceTablePtrAddr = Address + 0x00; // 4 bytes
            _matrixTablePtrAddr        = Address + 0x04; // 4 bytes
            _posXAddr                  = Address + 0x08; // 4 bytes
            _posYAddr                  = Address + 0x0C; // 4 bytes
            _posZAddr                  = Address + 0x10; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_modelInstanceTablePtrAddr), displayOrder: 0, displayName: "ModelsPtr", isPointer: true)]
        [BulkCopy]
        public uint ModelInstanceTablePtr {
            get => (uint) Data.GetDouble(_modelInstanceTablePtrAddr);
            set => Data.SetDouble(_modelInstanceTablePtrAddr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_matrixTablePtrAddr), displayOrder: 1, displayName: "MatricesPtr", isPointer: true)]
        [BulkCopy]
        public uint MatrixTablePtr {
            get => (uint) Data.GetDouble(_matrixTablePtrAddr);
            set => Data.SetDouble(_matrixTablePtrAddr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_posXAddr), displayOrder: 2, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public int PosX {
            get => Data.GetDouble(_posXAddr);
            set => Data.SetDouble(_posXAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_posYAddr), displayOrder: 3, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public int PosY {
            get => Data.GetDouble(_posYAddr);
            set => Data.SetDouble(_posYAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_posZAddr), displayOrder: 4, displayFormat: "X8", minWidth: 75)]
        [BulkCopy]
        public int PosZ {
            get => Data.GetDouble(_posZAddr);
            set => Data.SetDouble(_posZAddr, value);
        }
    }
}
