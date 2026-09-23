using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCModelChunkHeader : Struct {
        private readonly int _modelsOffset;
        private readonly int _rigOffsetAddr;

        public PCModelChunkHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            _modelsOffset  = Address + 0x00; // 4 bytes
            _rigOffsetAddr = Address + 0x04; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_modelsOffset), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public uint ModelsOffset {
            get => Data.GetUInt32(_modelsOffset);
            set => Data.SetUInt32(_modelsOffset, value);
        }

        [TableViewModelColumn(addressField: nameof(_rigOffsetAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public uint RigOffset {
            get => Data.GetUInt32(_rigOffsetAddr);
            set => Data.SetUInt32(_rigOffsetAddr, value);
        }
    }
}
