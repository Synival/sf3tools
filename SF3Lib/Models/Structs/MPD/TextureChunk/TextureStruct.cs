using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.MPD.TextureChunk {
    public class TextureStruct : TextureStructBase, ITexture {
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _imageDataOffsetAddr;

        public TextureStruct(
            IByteData data, CollectionType collection, int id, string name, int address,
            TexturePixelFormat pixelFormat, int chunkIndex, int? nextImageDataOffset, IMPD_File mpdFile
        ) : base(
            data, id, name, address, GlobalSize, GuessPixelFormat(pixelFormat, data, address, nextImageDataOffset),
            isCompressed: false, zeroIsTransparent: true
        ) {
            Collection       = collection;
            ImportExportName = "Texture_" + ((collection == CollectionType.Primary) ? "" : $"{collection}_") + $"{id:X2}";
            ChunkIndex       = chunkIndex;

            _widthAddr           = Address;     // 1 byte
            _heightAddr          = Address + 1; // 1 byte
            _imageDataOffsetAddr = Address + 2; // 2 bytes

            PixelFormatKnown = pixelFormat != TexturePixelFormat.Unknown;
            MPD_File = mpdFile;

            LoadImageData();
        }

        private static TexturePixelFormat GuessPixelFormat(TexturePixelFormat inputFormat, IByteData data, int address, int? nextImageDataOffset) {
            if (inputFormat != TexturePixelFormat.Unknown || !nextImageDataOffset.HasValue)
                return inputFormat;

            var width  = data.GetByte(address + 0);
            var height = data.GetByte(address + 1);
            var offset = data.GetWord(address + 2);

            if (width > 0 && height > 0) {
                var imageDataSize = nextImageDataOffset - offset;
                var bytesPerPixel =  imageDataSize / (double) width / height;
                if (bytesPerPixel == 2.00)
                    return TexturePixelFormat.ABGR1555;
                else if (bytesPerPixel == 1.00)
                    return TexturePixelFormat.UnknownPalette;
                else {
                    try {
                        throw new ArgumentException("Unhandled bytes per pixel: " + bytesPerPixel.ToString());
                    }
                    catch { }
                }
            }

            return TexturePixelFormat.ABGR1555;
        }

        protected override void OnImageUpdated() {}

        public static int GlobalSize => 0x04;

        public int Frame => 0;
        public int Duration => 0;

        public Dictionary<TagKey, TagValue> Tags { get; set; } = null;

        [TableViewModelColumn(addressField: null, displayOrder: -2.66f, displayName: "Collection", minWidth: 130)]
        public CollectionType Collection { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -2.33f, displayName: "Chunk #")]
        public int ChunkIndex { get; }

        [BulkCopy]
        protected override int StructWidth {
            get => Data.GetByte(_widthAddr);
            set => Data.SetByte(_widthAddr, (byte) value);
        }

        [BulkCopy]
        protected override int StructHeight {
            get => Data.GetByte(_heightAddr);
            set => Data.SetByte(_heightAddr, (byte) value);
        }

        [BulkCopy]
        protected override int StructImageDataOffset {
            get => Data.GetWord(_imageDataOffsetAddr);
            set => Data.SetWord(_imageDataOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 2.5f)]
        public bool PixelFormatKnown { get; }
        public IMPD_File MPD_File { get; }

        protected override Palette StructPalette {
            get => PixelFormat == TexturePixelFormat.ABGR1555 ? null : MPD_File.CreatePalette(2);
            set {}
        }

        [TableViewModelColumn(addressField: null, displayName: "Tags", displayOrder: 5, minWidth: 200)]
        public string TagsStr => (Tags == null) ? "" : string.Join(", ", Tags.Select(x => x.Key + "|" + x.Value));

        public override bool HasImage => true;

        public string ImportExportName { get; }

        public override bool CanLoadImage => true;
    }
}
