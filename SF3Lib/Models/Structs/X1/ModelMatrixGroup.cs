using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;

namespace SF3.Models.Structs.X1 {
    public class ModelMatrixGroup : Struct {
        private readonly int _modelPtrAddr;
        private readonly int _matrixPtrAddr;
        private readonly int _posXAddr;
        private readonly int _posYAddr;
        private readonly int _posZAddr;

        public ModelMatrixGroup(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x14) {
            _modelPtrAddr  = Address + 0x00; // 4 bytes
            _matrixPtrAddr = Address + 0x04; // 4 bytes
            _posXAddr      = Address + 0x08; // 4 bytes
            _posYAddr      = Address + 0x0C; // 4 bytes
            _posZAddr      = Address + 0x10; // 4 bytes
        }

        public INameGetterContext NameGetterContext { get; }

        [TableViewModelColumn(displayOrder: 0, isPointer: true)]
        [BulkCopy]
        public uint ModelPtr {
            get => (uint) Data.GetDouble(_modelPtrAddr);
            set => Data.SetDouble(_modelPtrAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 1, isPointer: true)]
        [BulkCopy]
        public uint MatrixPtr {
            get => (uint) Data.GetDouble(_matrixPtrAddr);
            set => Data.SetDouble(_matrixPtrAddr, (int) value);
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
