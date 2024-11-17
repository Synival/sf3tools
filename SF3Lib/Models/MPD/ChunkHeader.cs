using CommonLib.Attributes;
using SF3.BulkOperations;
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
        [DataMetadata(displayName: "Chunk Address", intDisplayMode: IntDisplayMode.Hex, displayFormat: "{0:X4}", isPointer: true)]
        public int ChunkAddress {
            get => Editor.GetDouble(chunkAddressAddress);
            set => Editor.SetDouble(chunkAddressAddress, value);
        }

        [BulkCopy]
        [DataMetadata(displayName: "Chunk Size", intDisplayMode: IntDisplayMode.Decimal)]
        public int ChunkSize {
            get => Editor.GetDouble(chunkSizeAddress);
            set => Editor.SetDouble(chunkSizeAddress, value);
        }
    }
}
