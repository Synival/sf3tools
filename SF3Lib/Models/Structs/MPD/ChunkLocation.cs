using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class ChunkLocation : Struct {
        private const int c_mpdRAMLocation = 0x290000;

        private readonly int _chunkAddressAddr;
        private readonly int _chunkSizeAddr;

        public ChunkLocation(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 8) {
            _chunkAddressAddr = Address;     // 4 bytes
            _chunkSizeAddr    = Address + 4; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk RAM Address", isPointer: true, displayOrder: 0)]
        public int ChunkRAMAddress {
            get => Data.GetDouble(_chunkAddressAddr);
            set => Data.SetDouble(_chunkAddressAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk File Address", displayFormat: "X6", displayOrder: 0.5f)]
        public int ChunkFileAddress {
            get {
                var addr = ChunkRAMAddress;
                return (addr == 0) ? 0 : (addr - c_mpdRAMLocation);
            }
            set => ChunkRAMAddress = (value == 0) ? 0 : (value + c_mpdRAMLocation);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk Size", displayFormat: "X4", displayOrder: 1)]
        public int ChunkSize {
            get => Data.GetDouble(_chunkSizeAddr);
            set => Data.SetDouble(_chunkSizeAddr, value);
        }

        [TableViewModelColumn(displayName: "Exists", displayOrder: 2)]
        public bool Exists => ChunkRAMAddress > 0 && ChunkSize != 0;

        public ChunkType ChunkType { get; set; } = ChunkType.Unset;

        [TableViewModelColumn(displayName: "Type", displayOrder: 3, isReadOnly: true, minWidth: 150)]
        public string ChunkTypeForView => Exists ? ChunkType.ToString() : "--";

        public CompressionType CompressionType { get; set; } = CompressionType.Unset;

        [TableViewModelColumn(displayName: "Compression Type", displayOrder: 3.1f, isReadOnly: true, minWidth: 150)]
        public string CompressionTypeForView => Exists ? CompressionType.ToString() : "--";

        [BulkCopy]
        [TableViewModelColumn(displayName: "Un/decompressed Size", displayFormat: "X4", displayOrder: 4, isReadOnly: true)]
        public int DecompressedSize { get; set; } = 0;
    }
}
