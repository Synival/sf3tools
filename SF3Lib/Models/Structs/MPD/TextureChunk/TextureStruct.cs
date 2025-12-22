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
            TexturePixelFormat pixelFormat, int? chunkIndex, int? nextImageDataOffset, IMPD_File mpdFile
        ) : base(
            data, id, name, address, GlobalSize, GuessPixelFormat(pixelFormat, data, address, nextImageDataOffset),
            isCompressed: false, zeroIsTransparent: true, chunkIndex: chunkIndex
        ) {
            Collection       = collection;
            ImportExportName = "Texture_" + ((collection == CollectionType.Primary) ? "" : $"{collection}_") + $"{id:X2}";

            _widthAddr           = Address;     // 1 byte
            _heightAddr          = Address + 1; // 1 byte
            _imageDataOffsetAddr = Address + 2; // 2 bytes

            PixelFormatKnown = pixelFormat != TexturePixelFormat.Unknown;
            MPD_File = mpdFile;
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

        public override void OnSetImageData() {}

        public static int GlobalSize => 0x04;

        public int Frame => 0;
        public int Duration => 0;

        public Dictionary<TagKey, TagValue> Tags { get; set; } = null;

        [TableViewModelColumn(addressField: null, displayOrder: -2.66f, displayName: "Collection", minWidth: 130)]
        public CollectionType Collection { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_widthAddr), displayOrder: 0)]
        public override int Width {
            get => Data.GetByte(_widthAddr);
            set {
                Data.SetByte(_widthAddr, (byte) value);
                InvalidateImage();
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_heightAddr), displayOrder: 1)]
        public override int Height {
            get => Data.GetByte(_heightAddr);
            set {
                Data.SetByte(_heightAddr, (byte) value);
                InvalidateImage();
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_imageDataOffsetAddr), displayOrder: 2, displayFormat: "X4")]
        public override int ImageDataOffset {
            get => Data.GetWord(_imageDataOffsetAddr);
            set {
                Data.SetWord(_imageDataOffsetAddr, value);
                InvalidateImage();
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 2.5f)]
        public bool PixelFormatKnown { get; }
        public IMPD_File MPD_File { get; }

        public override Palette Palette {
            get => PixelFormat == TexturePixelFormat.ABGR1555 ? null : MPD_File.CreatePalette(2);
            protected set {}
        }

        [TableViewModelColumn(addressField: null, displayName: "Tags", displayOrder: 5, minWidth: 200)]
        public string TagsStr => (Tags == null) ? "" : string.Join(", ", Tags.Select(x => x.Key + "|" + x.Value));

        public override bool HasImage => true;

        public string ImportExportName { get; }

        public override bool CanLoadImage => true;
    }
}
