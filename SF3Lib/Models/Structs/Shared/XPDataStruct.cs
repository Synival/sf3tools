using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class XPDataStruct : PDataStruct {
        private readonly int _vertexNormalsOffsetAddr;

        public XPDataStruct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _vertexNormalsOffsetAddr = Address + 0x14; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_vertexNormalsOffsetAddr), displayOrder: 5, isPointer: true)]
        public uint VertexNormalsOffset {
            get => Data.GetUInt32(_vertexNormalsOffsetAddr);
            set => Data.SetUInt32(_vertexNormalsOffsetAddr, value);
        }
    }
}
