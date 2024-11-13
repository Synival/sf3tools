﻿using CommonLib.Attributes;
using SF3.StreamEditors;

namespace SF3.Models.MPD {
    public class ChunkHeader : Model {
        private readonly int chunkAddressAddress;
        private readonly int chunkSizeAddress;

        public ChunkHeader(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 8) {
            chunkAddressAddress = Address;     // 4 bytes
            chunkSizeAddress    = Address + 4; // 4 bytes
        }

        [BulkCopy]
        public int ChunkAddress {
            get => Editor.GetDouble(chunkAddressAddress);
            set => Editor.SetDouble(chunkAddressAddress, value);
        }

        [BulkCopy]
        public int ChunkSize {
            get => Editor.GetDouble(chunkSizeAddress);
            set => Editor.SetDouble(chunkSizeAddress, value);
        }
    }
}
