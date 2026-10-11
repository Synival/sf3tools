using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class ModelAnimLoadInfoSc1 : Struct {
        public bool IsAllInts { get; }

        private readonly int _modelFileIdAddr;
        private readonly int _animIdAddr;
        private readonly int _animFileIdAddr;
        private readonly int _filePosInSectorsAddr;
        private readonly int _sizeInSectorsAddr;

        public ModelAnimLoadInfoSc1(IByteData data, int id, string name, int address, bool isAllInts) : base(data, id, name, address, isAllInts ? 0x14 : 0x0c) {
            IsAllInts = isAllInts;
            var valueSize = IsAllInts ? 4 : 2;

            _modelFileIdAddr      = Address + 0x00;                   // 4 bytes
            _animIdAddr           = Address + 0x04 + (0 * valueSize); // 2 bytes (4 for prototype)
            _animFileIdAddr       = Address + 0x04 + (1 * valueSize); // 2 bytes (4 for prototype)
            _filePosInSectorsAddr = Address + 0x04 + (2 * valueSize); // 2 bytes (4 for prototype)
            _sizeInSectorsAddr    = Address + 0x04 + (3 * valueSize); // 2 bytes (4 for prototype)
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
            get => Data.GetInt16(_animIdAddr + (IsAllInts ? 2 : 0));
            set => Data.SetInt16(_animIdAddr + (IsAllInts ? 2 : 0), value);
        }

        [TableViewModelColumn(addressField: nameof(_animFileIdAddr), displayOrder: 2, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public short AnimFileID {
            get => Data.GetInt16(_animFileIdAddr + (IsAllInts ? 2 : 0));
            set => Data.SetInt16(_animFileIdAddr + (IsAllInts ? 2 : 0), value);
        }

        [TableViewModelColumn(addressField: nameof(_filePosInSectorsAddr), displayOrder: 3, displayFormat: "X4")]
        [BulkCopy]
        public short FilePosInSectors {
            get => IsAllInts ? (short) (Data.GetInt32(_filePosInSectorsAddr) / 0x800) : Data.GetInt16(_filePosInSectorsAddr);
            set {
                if (IsAllInts)
                    Data.SetInt32(_filePosInSectorsAddr, value * 0x800);
                else
                    Data.SetInt16(_filePosInSectorsAddr, value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_sizeInSectorsAddr), displayOrder: 4, displayFormat: "X4")]
        [BulkCopy]
        public short SizeInSectors {
            get => IsAllInts ? (short) (Data.GetInt32(_sizeInSectorsAddr) / 0x800) : Data.GetInt16(_sizeInSectorsAddr);
            set {
                if (IsAllInts)
                    Data.SetInt32(_sizeInSectorsAddr, value * 0x800);
                else
                    Data.SetInt16(_sizeInSectorsAddr, value);
            }
        }

    }
}
