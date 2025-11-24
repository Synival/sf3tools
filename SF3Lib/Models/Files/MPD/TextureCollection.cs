using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.TextureCollection;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class TextureCollection : TableFile {
        protected TextureCollection(
            IByteData data, INameGetterContext nameContext, int address, string name,
            TextureCollectionType collection, Dictionary<int, TexturePixelFormat> pixelFormats, Dictionary<TexturePixelFormat, Palette> palettes,
            int? chunkIndex, int? firstTextureId
        ) : base(data, nameContext) {
            Address      = address;
            Name         = name;
            Collection   = collection;
            PixelFormats = pixelFormats;
            Palettes     = palettes;
            ChunkIndex   = chunkIndex;
            FirstTextureID = firstTextureId;
        }

        public static TextureCollection Create(
            IByteData data, INameGetterContext nameContext, int address, string name,
            TextureCollectionType collection, Dictionary<int, TexturePixelFormat> pixelFormats, Dictionary<TexturePixelFormat, Palette> palettes,
            int? chunkIndex, int? firstTextureId
        ) {
            var newFile = new TextureCollection(data, nameContext, address, name, collection, pixelFormats, palettes, chunkIndex, firstTextureId);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            TextureHeaderTable = TextureHeaderTable.Create(Data, "TexturesHeader", 0x00);
            var header = TextureHeaderTable[0];

            var startId = FirstTextureID ?? header.TextureIdStart;
            return new List<ITable>() {
                TextureHeaderTable,
                (TextureTable = TextureTable.Create(Data, "Textures", 0x04, Collection, header.NumTextures, startId, PixelFormats, Palettes, ChunkIndex)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public TextureCollectionType Collection { get; }
        public Dictionary<int, TexturePixelFormat> PixelFormats { get; }
        public Dictionary<TexturePixelFormat, Palette> Palettes { get; }
        public int? ChunkIndex { get; }
        public int? FirstTextureID { get; }

        [BulkCopyRecurse]
        public TextureHeaderTable TextureHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; private set; }
    }
}
