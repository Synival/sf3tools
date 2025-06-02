using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class FileIdModel : Struct {
        private readonly int _fileIdAddr;

        public FileIdModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x04) {
            _fileIdAddr = Address + 0x00; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int FileId {
            get => Data.GetDouble(_fileIdAddr);
            set => Data.SetDouble(_fileIdAddr, value);
        }
    }
}
