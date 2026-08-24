using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared.SGL {
    public class PDataStruct : Struct, IPDATA {
        private readonly int _verticesOffsetAddr;
        private readonly int _vertexCountAddr;
        private readonly int _polygonsOffsetAddr;
        private readonly int _faceCountAddr;
        private readonly int _attributesOffsetAddr;

        public PDataStruct(IByteData data, int id, string name, int address)
        : this(data, id, name, address, 0x14) {
        }

        protected PDataStruct(IByteData data, int id, string name, int address, int size)
        : base(data, id, name, address, size) {
            _verticesOffsetAddr      = Address + 0x00; // 4 bytes
            _vertexCountAddr         = Address + 0x04; // 4 bytes
            _polygonsOffsetAddr      = Address + 0x08; // 4 bytes
            _faceCountAddr           = Address + 0x0C; // 4 bytes
            _attributesOffsetAddr    = Address + 0x10; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_verticesOffsetAddr), displayOrder: 0, isPointer: true)]
        public uint VerticesOffset {
            get => Data.GetUInt32(_verticesOffsetAddr);
            set => Data.SetUInt32(_verticesOffsetAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_vertexCountAddr), displayOrder: 1)]
        public int VertexCount {
            get => Data.GetInt32(_vertexCountAddr);
            set => Data.SetInt32(_vertexCountAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_polygonsOffsetAddr), displayOrder: 2, isPointer: true)]
        public uint PolygonsOffset {
            get => Data.GetUInt32(_polygonsOffsetAddr);
            set => Data.SetUInt32(_polygonsOffsetAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_faceCountAddr), displayOrder: 3)]
        public int FaceCount {
            get => Data.GetInt32(_faceCountAddr);
            set => Data.SetInt32(_faceCountAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_attributesOffsetAddr), displayOrder: 4, isPointer: true)]
        public uint AttributesOffset {
            get => Data.GetUInt32(_attributesOffsetAddr);
            set => Data.SetUInt32(_attributesOffsetAddr, value);
        }
    }
}
