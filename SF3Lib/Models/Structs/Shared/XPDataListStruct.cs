using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class XPDataListStruct : Struct {
        private readonly int _xpdataListOffsetAddr;

        public XPDataListStruct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _xpdataListOffsetAddr = Address + 0x00; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_xpdataListOffsetAddr), displayOrder: 0, isPointer: true)]
        public int XPDataListOffset {
            get => Data.GetInt32(_xpdataListOffsetAddr);
            set => Data.SetInt32(_xpdataListOffsetAddr, value);
        }
    }
}
