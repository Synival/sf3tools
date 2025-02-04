using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class ChunkHeader : Struct {
        private readonly int chunkAddressAddress;
        private readonly int chunkSizeAddress;

        public ChunkHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 8) {
            chunkAddressAddress = Address;     // 4 bytes
            chunkSizeAddress    = Address + 4; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk RAM Address", isPointer: true, displayOrder: 0)]
        public int ChunkAddress {
            get => Data.GetDouble(chunkAddressAddress);
            set => Data.SetDouble(chunkAddressAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk File Address", displayFormat: "X6", displayOrder: 0.5f)]
        public int ChunkFileAddress {
            get {
                var addr = ChunkAddress;
                return (addr == 0) ? 0 : (ChunkAddress - 0x290000);
            }
            set => ChunkAddress = (value == 0) ? 0 : + 0x290000;
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk Size", displayFormat: "X4", displayOrder: 1)]
        public int ChunkSize {
            get => Data.GetDouble(chunkSizeAddress);
            set => Data.SetDouble(chunkSizeAddress, value);
        }

        [TableViewModelColumn(displayName: "Exists", displayOrder: 2)]
        public bool Exists => ChunkAddress > 0 && ChunkSize != 0;

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
