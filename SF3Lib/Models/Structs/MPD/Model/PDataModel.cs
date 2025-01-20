using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class PDataModel : Struct {
        public readonly int _verticesOffsetAddr;
        public readonly int _vertexCountAddr;
        public readonly int _polygonsOffsetAddr;
        public readonly int _polygonCountAddr;
        public readonly int _attributesOffsetAddr;

        public PDataModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x14) {
            _verticesOffsetAddr   = Address + 0x00; // 4 bytes
            _vertexCountAddr      = Address + 0x04; // 4 bytes
            _polygonsOffsetAddr   = Address + 0x08; // 4 bytes
            _polygonCountAddr     = Address + 0x0C; // 4 bytes
            _attributesOffsetAddr = Address + 0x10; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, isPointer: true)]
        public int VerticesOffset {
            get => Data.GetDouble(_verticesOffsetAddr);
            set => Data.SetDouble(_verticesOffsetAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1)]
        public int VertexCount {
            get => Data.GetDouble(_vertexCountAddr);
            set => Data.SetDouble(_vertexCountAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, isPointer: true)]
        public int PolygonsOffset {
            get => Data.GetDouble(_polygonsOffsetAddr);
            set => Data.SetDouble(_polygonsOffsetAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3)]
        public int PolygonCount {
            get => Data.GetDouble(_polygonCountAddr);
            set => Data.SetDouble(_polygonCountAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, isPointer: true)]
        public int AttributesOffset {
            get => Data.GetDouble(_attributesOffsetAddr);
            set => Data.SetDouble(_attributesOffsetAddr, value);
        }
    }
}
