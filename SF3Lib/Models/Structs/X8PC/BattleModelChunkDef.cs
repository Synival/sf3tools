using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class BattleModelChunkDef : Struct {
        private readonly int _offsetAddr;
        private readonly int _dataSizeAddr;
        private readonly int _chunkSizeAddr;
        private readonly int _paddingAddr;

        public BattleModelChunkDef(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _offsetAddr    = Address + 0x00; // 4 bytes
            _dataSizeAddr  = Address + 0x04; // 4 bytes
            _chunkSizeAddr = Address + 0x08; // 4 bytes
            _paddingAddr   = Address + 0x0C; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_offsetAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public uint Offset {
            get => Data.GetUInt32(_offsetAddr);
            set => Data.SetUInt32(_offsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dataSizeAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public uint DataSize {
            get => Data.GetUInt32(_dataSizeAddr);
            set => Data.SetUInt32(_dataSizeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_chunkSizeAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public uint ChunkSize {
            get => Data.GetUInt32(_chunkSizeAddr);
            set => Data.SetUInt32(_chunkSizeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paddingAddr), displayName: "Padding (0x0c)", displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public uint Padding_0x0C {
            get => Data.GetUInt32(_paddingAddr);
            set => Data.SetUInt32(_paddingAddr, value);
        }
    }
}
