using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class PDataModel : Struct {
        public readonly int _verticesOffsetAddr;
        public readonly int _vertexCountAddr;
        public readonly int _polygonsOffsetAddr;
        public readonly int _polygonCountAddr;
        public readonly int _attributesOffsetAddr;

        public PDataModel(IByteData data, int id, string name, int address,
            ModelCollectionType collection, int chunkIndex, int index, int refs
        ) : base(data, id, name, address, 0x14) {
            Collection = collection;
            ChunkIndex = chunkIndex;
            Index      = index;
            Refs       = refs;

            _verticesOffsetAddr   = Address + 0x00; // 4 bytes
            _vertexCountAddr      = Address + 0x04; // 4 bytes
            _polygonsOffsetAddr   = Address + 0x08; // 4 bytes
            _polygonCountAddr     = Address + 0x0C; // 4 bytes
            _attributesOffsetAddr = Address + 0x10; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: -2.66f, displayName: "Collection", minWidth: 110)]
        public ModelCollectionType Collection { get; }

        [TableViewModelColumn(displayOrder: -2.33f, displayName: "Chunk #")]
        public int ChunkIndex { get; }

        [TableViewModelColumn(displayOrder: -2.15f, displayName: "Index")]
        public int Index { get; }

        [TableViewModelColumn(displayOrder: 0)]
        public int Refs { get; }

        [TableViewModelColumn(displayOrder: 0.25f, isPointer: true, isReadOnly: true)]
        public uint RamAddress { get; set; }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.5f, isPointer: true)]
        public uint VerticesOffset {
            get => (uint) Data.GetDouble(_verticesOffsetAddr);
            set => Data.SetDouble(_verticesOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1)]
        public int VertexCount {
            get => Data.GetDouble(_vertexCountAddr);
            set => Data.SetDouble(_vertexCountAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, isPointer: true)]
        public uint PolygonsOffset {
            get => (uint) Data.GetDouble(_polygonsOffsetAddr);
            set => Data.SetDouble(_polygonsOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3)]
        public int PolygonCount {
            get => Data.GetDouble(_polygonCountAddr);
            set => Data.SetDouble(_polygonCountAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, isPointer: true)]
        public uint AttributesOffset {
            get => (uint) Data.GetDouble(_attributesOffsetAddr);
            set => Data.SetDouble(_attributesOffsetAddr, (int) value);
        }
    }
}
