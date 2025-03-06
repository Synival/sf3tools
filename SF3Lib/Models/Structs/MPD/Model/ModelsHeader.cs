using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class ModelsHeader : Struct {
        private readonly int _collisionLinesHeaderOffsetAddr;
        private readonly int _collisionBlocksOffsetAddr;
        private readonly int _numModelsAddress;

        public ModelsHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0C) {
            _collisionLinesHeaderOffsetAddr = Address + 0x00; // 4 bytes
            _collisionBlocksOffsetAddr      = Address + 0x04; // 4 bytes
            _numModelsAddress               = Address + 0x08; // 2 bytes
            // (2 bytes of padding)
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, isPointer: true, minWidth: 80)]
        public uint CollisionLinesHeaderOffset {
            get => (uint) Data.GetDouble(_collisionLinesHeaderOffsetAddr);
            set => Data.SetDouble(_collisionLinesHeaderOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, isPointer: true, minWidth: 80)]
        public uint CollisionBlocksOffset {
            get => (uint) Data.GetDouble(_collisionBlocksOffsetAddr);
            set => Data.SetDouble(_collisionBlocksOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2)]
        public int NumModels {
            get => Data.GetWord(_numModelsAddress);
            set => Data.SetWord(_numModelsAddress, value);
        }
    }
}
