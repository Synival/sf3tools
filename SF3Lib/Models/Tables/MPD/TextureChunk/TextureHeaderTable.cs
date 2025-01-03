using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.MPD.TextureChunk {
    public class TextureHeaderTable : Table<TextureHeader> {
        protected TextureHeaderTable(IByteData data, int address) : base(data, address) {
        }

        public static TextureHeaderTable Create(IByteData data, int address) {
            var newTable = new TextureHeaderTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new TextureHeader(Data, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
