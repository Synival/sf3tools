using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.TextureCollection;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class TextureCollection : TableFile {
        protected TextureCollection(IByteData data, INameGetterContext nameContext, int address, string name, TextureCollectionType collection, int? chunkIndex)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            Collection = collection;
            ChunkIndex = chunkIndex;
        }

        public static TextureCollection Create(IByteData data, INameGetterContext nameContext, int address, string name, TextureCollectionType collection, int? chunkIndex) {
            var newFile = new TextureCollection(data, nameContext, address, name, collection, chunkIndex);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            TextureHeaderTable = TextureHeaderTable.Create(Data, 0x00);
            var header = TextureHeaderTable[0];

            return new List<ITable>() {
                TextureHeaderTable,
                (TextureTable = TextureTable.Create(Data, 0x04, Collection, header.NumTextures, header.TextureIdStart, ChunkIndex)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public TextureCollectionType Collection { get; }
        public int? ChunkIndex { get; }


        [BulkCopyRecurse]
        public TextureHeaderTable TextureHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; private set; }
    }
}
