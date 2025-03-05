using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class ModelsHeader : Struct {
        private readonly int _collisionSegmentsOffsetAddr;
        private readonly int _collisionBlocksOffsetAddr;
        private readonly int _numModelsAddress;

        public ModelsHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0C) {
            _collisionSegmentsOffsetAddr = Address + 0x00; // 4 bytes
            _collisionBlocksOffsetAddr   = Address + 0x04; // 4 bytes
            _numModelsAddress            = Address + 0x08; // 2 bytes
            // (2 bytes of padding)
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, isPointer: true)]
        public int CollisionSegmentsOffset {
            get => Data.GetDouble(_collisionSegmentsOffsetAddr);
            set => Data.SetDouble(_collisionSegmentsOffsetAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, isPointer: true)]
        public int CollisionBlocksOffset {
            get => Data.GetDouble(_collisionBlocksOffsetAddr);
            set => Data.SetDouble(_collisionBlocksOffsetAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2)]
        public int NumModels {
            get => Data.GetWord(_numModelsAddress);
            set => Data.SetWord(_numModelsAddress, value);
        }
    }
}
