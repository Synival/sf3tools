using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class ModelAnimLoadInfoSc1 : Struct {
        private readonly int _modelFileIdAddr;
        private readonly int _animIdAddr;
        private readonly int _animFileIdAddr;
        private readonly int _filePosInSectorsAddr;
        private readonly int _sizeInSectorsAddr;

        public ModelAnimLoadInfoSc1(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0c) {
            _modelFileIdAddr      = Address + 0x00; // 4 bytes
            _animIdAddr           = Address + 0x04; // 2 bytes
            _animFileIdAddr       = Address + 0x06; // 2 bytes
            _filePosInSectorsAddr = Address + 0x08; // 2 bytes
            _sizeInSectorsAddr    = Address + 0x0a; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_modelFileIdAddr), displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int ModelFileID {
            get => Data.GetInt32(_modelFileIdAddr);
            set => Data.SetInt32(_modelFileIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_animIdAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public short AnimID {
            get => Data.GetInt16(_animIdAddr);
            set => Data.SetInt16(_animIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_animFileIdAddr), displayOrder: 2, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public short AnimFileID {
            get => Data.GetInt16(_animFileIdAddr);
            set => Data.SetInt16(_animFileIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_filePosInSectorsAddr), displayOrder: 3, displayFormat: "X4")]
        [BulkCopy]
        public short FilePosInSectors {
            get => Data.GetInt16(_filePosInSectorsAddr);
            set => Data.SetInt16(_filePosInSectorsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_sizeInSectorsAddr), displayOrder: 4, displayFormat: "X4")]
        [BulkCopy]
        public short sizeInSectors {
            get => Data.GetInt16(_sizeInSectorsAddr);
            set => Data.SetInt16(_sizeInSectorsAddr, value);
        }
    }
}
