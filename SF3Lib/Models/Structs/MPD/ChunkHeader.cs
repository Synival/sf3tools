using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class ChunkHeader : Struct {
        private readonly int chunkAddressAddress;
        private readonly int chunkSizeAddress;

        public ChunkHeader(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 8) {
            chunkAddressAddress = Address;     // 4 bytes
            chunkSizeAddress    = Address + 4; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk Address", isPointer: true, displayOrder: 0)]
        public int ChunkAddress {
            get => Editor.GetDouble(chunkAddressAddress);
            set => Editor.SetDouble(chunkAddressAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Chunk Size", displayFormat: "X4", displayOrder: 1)]
        public int ChunkSize {
            get => Editor.GetDouble(chunkSizeAddress);
            set => Editor.SetDouble(chunkSizeAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Compression Type", displayOrder: 2, isReadOnly: true)]
        public string CompressionType { get; set; } = "(Unset)";

        [BulkCopy]
        [TableViewModelColumn(displayName: "Un/decompressed Size", displayFormat: "X4", displayOrder: 3, isReadOnly: true)]
        public int DecompressedSize { get; set; } = 0;
    }
}
