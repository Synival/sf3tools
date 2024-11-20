using CommonLib.Attributes;
using SF3.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD {
    public class ChunkHeader : Model {
        private readonly int chunkAddressAddress;
        private readonly int chunkSizeAddress;

        public ChunkHeader(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 8) {
            chunkAddressAddress = Address;     // 4 bytes
            chunkSizeAddress    = Address + 4; // 4 bytes
        }

        [BulkCopy]
        [ViewModelData(displayName: "Chunk Address", isPointer: true, displayOrder: 0)]
        public int ChunkAddress {
            get => Editor.GetDouble(chunkAddressAddress);
            set => Editor.SetDouble(chunkAddressAddress, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Chunk Size", displayFormat: "X4", displayOrder: 1)]
        public int ChunkSize {
            get => Editor.GetDouble(chunkSizeAddress);
            set => Editor.SetDouble(chunkSizeAddress, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Compression Type", displayOrder: 2, isReadOnly: true)]
        public string CompressionType { get; set; } = "(Unset)";

        [BulkCopy]
        [ViewModelData(displayName: "Un/decompressed Size", displayFormat: "X4", displayOrder: 3, isReadOnly: true)]
        public int DecompressedSize { get; set; } = 0;
    }
}
