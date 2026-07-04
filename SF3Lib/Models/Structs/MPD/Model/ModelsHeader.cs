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
        [TableViewModelColumn(addressField: nameof(_collisionLinesHeaderOffsetAddr), displayOrder: 0, isPointer: true, minWidth: 80)]
        public uint CollisionLinesHeaderOffset {
            get => (uint) Data.GetInt32(_collisionLinesHeaderOffsetAddr);
            set => Data.SetInt32(_collisionLinesHeaderOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_collisionBlocksOffsetAddr), displayOrder: 1, isPointer: true, minWidth: 80)]
        public uint CollisionBlocksOffset {
            get => (uint) Data.GetInt32(_collisionBlocksOffsetAddr);
            set => Data.SetInt32(_collisionBlocksOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_numModelsAddress), displayOrder: 2)]
        public ushort NumModels {
            get => Data.GetUInt16(_numModelsAddress);
            set => Data.SetUInt16(_numModelsAddress, value);
        }
    }
}
