using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1 {
    public class ModelMatrixGroupLink : Struct {
        private readonly int _modelIdAddr;
        private readonly int _matrixBasisAddr;
        private readonly int _typeAddr;
        private readonly int _scriptAddr;

        public ModelMatrixGroupLink(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            _modelIdAddr     = Address + 0x00; // 2 bytes
            _matrixBasisAddr = Address + 0x02; // 1 bytes
            _typeAddr        = Address + 0x03; // 1 bytes
            _scriptAddr      = Address + 0x04; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public short ModelID {
            get => (short) Data.GetWord(_modelIdAddr);
            set => Data.SetWord(_modelIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte MatrixBasis {
            get => (byte) Data.GetByte(_matrixBasisAddr);
            set => Data.SetByte(_matrixBasisAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public byte Type {
            get => (byte) Data.GetByte(_typeAddr);
            set => Data.SetByte(_typeAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, isPointer: true)]
        [BulkCopy]
        public uint ScriptAddr {
            get => (uint) Data.GetDouble(_scriptAddr);
            set => Data.SetDouble(_scriptAddr, (int) value);
        }
    }
}
