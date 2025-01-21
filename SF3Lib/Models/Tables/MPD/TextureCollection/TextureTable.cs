﻿using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureTable : FixedSizeTable<TextureModel> {
        protected TextureTable(IByteData data, string name, int address, TextureCollectionType collection, int textureCount, int startId, int? chunkIndex) : base(data, name, address, textureCount) {
            if (textureCount > 255)
                throw new ArgumentOutOfRangeException(nameof(textureCount));
            StartID    = startId;
            Collection = collection;
            ChunkIndex = chunkIndex;
        }

        public static TextureTable Create(IByteData data, string name, int address, TextureCollectionType collection, int textureCount, int startId, int? chunkIndex) {
            var newTable = new TextureTable(data, name, address, collection, textureCount, startId, chunkIndex);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = TextureModel.GlobalSize;
            return Load((id, address) => {
                var nextImageDataOffset = id + 1 >= Size
                    ? Data.Length
                    : Data.GetWord(address + size + 2);
                return new TextureModel(Data, Collection, StartID + id, "Texture" + (StartID + id).ToString("D3"), address, ChunkIndex, nextImageDataOffset);
            });
        }

        public int StartID { get; }
        public TextureCollectionType Collection { get; }
        public int? ChunkIndex { get; }
    }
}
