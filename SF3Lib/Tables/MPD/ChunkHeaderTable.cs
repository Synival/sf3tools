﻿using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class ChunkHeaderTable : Table<ChunkHeader> {
        public ChunkHeaderTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ChunkHeader(FileEditor, id, "Chunk" + (id + 1), address));

        public override int? MaxSize => 32;
    }
}