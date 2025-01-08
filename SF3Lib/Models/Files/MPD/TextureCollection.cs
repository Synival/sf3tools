using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Models.Files.MPD.Objects {
    public class TextureCollection : TableFile {
        protected TextureCollection(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            ChunkIndex = chunkIndex;
        }

        public static TextureCollection Create(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex) {
            var newFile = new TextureCollection(data, nameContext, address, name, chunkIndex);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            TextureHeaderTable = TextureHeaderTable.Create(Data, 0x00);
            var header = TextureHeaderTable.Rows[0];

            return new List<ITable>() {
                TextureHeaderTable,
                (TextureTable = TextureTable.Create(Data, 0x04, header.NumTextures, header.TextureIdStart, ChunkIndex)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public int? ChunkIndex { get; }


        [BulkCopyRecurse]
        public TextureHeaderTable TextureHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; private set; }
    }
}
