using CommonLib.Attributes;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class ChunkHeader : Struct {
        private readonly int chunkAddressAddress;
        private readonly int chunkSizeAddress;

        public ChunkHeader(IRawData data, int id, string name, int address)
        : base(data, id, name, address, 8) {
            chunkAddressAddress = Address;     // 4 bytes
            chunkSizeAddress    = Address + 4; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk Address", isPointer: true, displayOrder: 0)]
        public int ChunkAddress {
            get => Data.GetDouble(chunkAddressAddress);
            set => Data.SetDouble(chunkAddressAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk Size", displayFormat: "X4", displayOrder: 1)]
        public int ChunkSize {
            get => Data.GetDouble(chunkSizeAddress);
            set => Data.SetDouble(chunkSizeAddress, value);
        }

        [TableViewModelColumn(displayName: "Exists", displayOrder: 2)]
        public bool Exists => ChunkAddress > 0;

        [BulkCopy]
        [TableViewModelColumn(displayName: "Compression Type", displayOrder: 3, isReadOnly: true, minWidth: 150)]
        public string CompressionType { get; set; } = "(Unset)";

        [BulkCopy]
        [TableViewModelColumn(displayName: "Un/decompressed Size", displayFormat: "X4", displayOrder: 4, isReadOnly: true)]
        public int DecompressedSize { get; set; } = 0;
    }
}
