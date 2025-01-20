using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureTable : Table<TextureModel> {
        protected TextureTable(IByteData data, int address, TextureCollectionType collection, int textureCount, int startId, int? chunkIndex) : base(data, address) {
            if (textureCount > 255)
                throw new ArgumentOutOfRangeException(nameof(textureCount));
            MaxSize    = Math.Min(255, textureCount);
            StartID    = startId;
            Collection = collection;
            ChunkIndex = chunkIndex;
        }

        public static TextureTable Create(IByteData data, int address, TextureCollectionType collection, int textureCount, int startId, int? chunkIndex) {
            var newTable = new TextureTable(data, address, collection, textureCount, startId, chunkIndex);
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
                return new TextureModel(Data, Collection, StartID + id, "Texture" + (StartID + id).ToString("D3"), address, ChunkIndex, nextImageDataOffset);
            });
        }

        public override int? MaxSize { get; }

        public int StartID { get; }
        public TextureCollectionType Collection { get; }
        public int? ChunkIndex { get; }
    }
}
