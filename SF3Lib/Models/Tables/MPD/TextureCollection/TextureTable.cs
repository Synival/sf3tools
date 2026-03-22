using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureTable : FixedSizeTable<TextureStruct>, IDisposable {
        protected TextureTable(
            IByteData data, string name, int address,
            MPD_CollectionType collection, int textureCount, int startId, Dictionary<int, TexturePixelFormat> pixelFormats,
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

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    foreach (var tex in Rows)
                        tex.Dispose();

                _disposedValue = true;
            }
        }

        public static TextureTable Create(
            IByteData data, string name, int address,
            MPD_CollectionType collection, int textureCount, int startId, Dictionary<int, TexturePixelFormat> pixelFormats,
            int chunkIndex, IMPD_File mpdFile
        )
            => Create(() => new TextureTable(data, name, address, collection, textureCount, startId, pixelFormats, chunkIndex, mpdFile));

        public override bool Load() {
            var size = TextureStruct.GlobalSize;
            return Load((id, address) => {
                var pixelFormat =
                    (Collection != MPD_CollectionType.Primary) ? TexturePixelFormat.ABGR1555 :
                    PixelFormats.TryGetValue(StartID + id, out var pixelFormatOut) ? pixelFormatOut : (TexturePixelFormat?) null;

                var nextImageDataOffset = id + 1 >= Size
                    ? Data.Length
                    : Data.GetWord(address + size + 2);

                var texId = StartID + id;
                return new TextureStruct(
                    Data, Collection, StartID + id, $"Texture{(int) Collection}_{texId:X2}", address, pixelFormat, ChunkIndex, nextImageDataOffset, MPD_File
                );
            });
        }

        public MPD_CollectionType Collection { get; }
        public int StartID { get; }
        public Dictionary<int, TexturePixelFormat> PixelFormats { get; }
        public Dictionary<TexturePixelFormat, Palette> Palettes { get; }
        public int ChunkIndex { get; }
        public IMPD_File MPD_File { get; }

        private bool _disposedValue;
    }
}
