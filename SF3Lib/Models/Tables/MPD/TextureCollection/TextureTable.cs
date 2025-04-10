using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureTable : FixedSizeTable<TextureModel> {
        protected TextureTable(
            IByteData data, string name, int address,
            TextureCollectionType collection, int textureCount, int startId, Dictionary<int, TexturePixelFormat> pixelFormats, Dictionary<TexturePixelFormat, Palette> palettes,
            int? chunkIndex
        ) : base(data, name, address, textureCount) {
            if (textureCount > 255)
                throw new ArgumentOutOfRangeException(nameof(textureCount));
            Collection   = collection;
            StartID      = startId;
            PixelFormats = pixelFormats;
            Palettes     = palettes;
            ChunkIndex   = chunkIndex;
        }

        public static TextureTable Create(
            IByteData data, string name, int address,
            TextureCollectionType collection, int textureCount, int startId, Dictionary<int, TexturePixelFormat> pixelFormats, Dictionary<TexturePixelFormat, Palette> palettes,
            int? chunkIndex
        ) {
            var newTable = new TextureTable(data, name, address, collection, textureCount, startId, pixelFormats, palettes, chunkIndex);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = TextureModel.GlobalSize;
            return Load((id, address) => {
                var pixelFormat = PixelFormats.TryGetValue(StartID + id, out var pixelFormatOut) ? pixelFormatOut : TexturePixelFormat.Unknown;
                var nextImageDataOffset = id + 1 >= Size
                    ? Data.Length
                    : Data.GetWord(address + size + 2);
                return new TextureModel(
                    Data, Collection, StartID + id, "Texture" + (StartID + id).ToString("D3"), address,
                    pixelFormat, GetPalette(pixelFormat), ChunkIndex, nextImageDataOffset
                );
            });
        }

        private Palette GetPalette(TexturePixelFormat format) {
            switch (format) {
                case TexturePixelFormat.UnknownPalette:
                case TexturePixelFormat.Palette1: {
                    return Palettes.TryGetValue(TexturePixelFormat.Palette1, out var outPalette) ? outPalette : null;
                }
                case TexturePixelFormat.Palette2: {
                    return Palettes.TryGetValue(TexturePixelFormat.Palette2, out var outPalette) ? outPalette : null;
                }
                case TexturePixelFormat.Palette3: {
                    return Palettes.TryGetValue(TexturePixelFormat.Palette3, out var outPalette) ? outPalette : null;
                }
                default:
                    return null;
            }
        }

        public TextureCollectionType Collection { get; }
        public int StartID { get; }
        public Dictionary<int, TexturePixelFormat> PixelFormats { get; }
        public Dictionary<TexturePixelFormat, Palette> Palettes { get; }
        public int? ChunkIndex { get; }
    }
}
