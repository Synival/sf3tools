using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.TextureCollection;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class TextureChunk : TableFile {
        protected TextureChunk(
            IByteData data, INameGetterContext nameContext, int address, string name,
            MPD_CollectionType collection, Dictionary<int, TexturePixelFormat> pixelFormats,
            int chunkIndex, int? firstTextureId, IMPD_File mpdFile
        ) : base(data, nameContext) {
            Address      = address;
            Name         = name;
            Collection   = collection;
            PixelFormats = pixelFormats;
            ChunkIndex   = chunkIndex;
            FirstTextureID = firstTextureId;
            MPD_File     = mpdFile;
        }

        public static TextureChunk Create(
            IByteData data, INameGetterContext nameContext, int address, string name,
            MPD_CollectionType collection, Dictionary<int, TexturePixelFormat> pixelFormats,
            int chunkIndex, int? firstTextureId, IMPD_File mpdFile
        ) {
            var newFile = new TextureChunk(data, nameContext, address, name, collection, pixelFormats, chunkIndex, firstTextureId, mpdFile);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            TextureHeaderTable = TextureHeaderTable.Create(Data, "TexturesHeader", 0x00);
            var header = TextureHeaderTable[0];

            var startId = FirstTextureID ?? header.TextureIdStart;
            return new List<ITable>() {
                TextureHeaderTable,
                (TextureTable = TextureTable.Create(Data, "Textures", 0x04, Collection, header.NumTextures, startId, PixelFormats, ChunkIndex, MPD_File)),
            };
        }

        protected override void OnDispose(bool disposing) {
            base.OnDispose(disposing);
            if (disposing)
                TextureTable?.Dispose();
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public MPD_CollectionType Collection { get; }
        public Dictionary<int, TexturePixelFormat> PixelFormats { get; }
        public int ChunkIndex { get; }
        public int? FirstTextureID { get; }
        public IMPD_File MPD_File { get; }
        [BulkCopyRecurse]
        public TextureHeaderTable TextureHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; private set; }
    }
}
