using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.TextureChunk;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureTable : Table<TextureModel> {
        protected TextureTable(IByteData data, int address, int textureCount, int startId, int? chunkIndex) : base(data, address) {
            MaxSize    = textureCount;
            StartID    = startId;
            ChunkIndex = chunkIndex;
        }

        public static TextureTable Create(IByteData data, int address, int textureCount, int startId, int? chunkIndex) {
            var newTable = new TextureTable(data, address, textureCount, startId, chunkIndex);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = TextureModel.GlobalSize;
            return LoadUntilMax((id, address) => {
                var nextImageDataOffset = id + 1 >= MaxSize
                    ? Data.Length
                    : Data.GetWord(address + size + 2);
                return new TextureModel(Data, StartID + id, "Texture" + (StartID + id).ToString("D3"), address, ChunkIndex, nextImageDataOffset);
            });
        }

        public override int? MaxSize { get; }

        public int StartID { get; }
        public int? ChunkIndex { get; }
    }
}
