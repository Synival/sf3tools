using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureTable : FixedSizeTable<TextureStruct> {
        protected TextureTable(
            IByteData data, string name, int address,
            CollectionType collection, int textureCount, int startId, Dictionary<int, TexturePixelFormat> pixelFormats,
            int chunkIndex, IMPD_File mpdFile
        ) : base(data, name, address, textureCount) {
            if (textureCount > 255)
                throw new ArgumentOutOfRangeException(nameof(textureCount));
            Collection   = collection;
            StartID      = startId;
            PixelFormats = pixelFormats;
            ChunkIndex   = chunkIndex;
            MPD_File     = mpdFile;
        }

        public static TextureTable Create(
            IByteData data, string name, int address,
            CollectionType collection, int textureCount, int startId, Dictionary<int, TexturePixelFormat> pixelFormats,
            int chunkIndex, IMPD_File mpdFile
        )
            => Create(() => new TextureTable(data, name, address, collection, textureCount, startId, pixelFormats, chunkIndex, mpdFile));

        public override bool Load() {
            var size = TextureStruct.GlobalSize;
            return Load((id, address) => {
                var pixelFormat =
                    (Collection != CollectionType.Primary) ? TexturePixelFormat.ABGR1555 :
                    PixelFormats.TryGetValue(StartID + id, out var pixelFormatOut) ? pixelFormatOut :
                    TexturePixelFormat.Unknown;

                var nextImageDataOffset = id + 1 >= Size
                    ? Data.Length
                    : Data.GetWord(address + size + 2);

                var texId = StartID + id;
                return new TextureStruct(
                    Data, Collection, StartID + id, $"Texture{(int) Collection}_{texId:X2}", address, pixelFormat, ChunkIndex, nextImageDataOffset, MPD_File
                );
            });
        }

        public CollectionType Collection { get; }
        public int StartID { get; }
        public Dictionary<int, TexturePixelFormat> PixelFormats { get; }
        public Dictionary<TexturePixelFormat, Palette> Palettes { get; }
        public int ChunkIndex { get; }
        public IMPD_File MPD_File { get; }
    }
}
