using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.TextureChunk {
    public class TextureModel : Struct {
        private readonly int widthAddress;
        private readonly int heightAddress;
        private readonly int imageDataOffsetAddress;

        public TextureModel(IByteData data, TextureCollectionType collection, int id, string name, int address, TexturePixelFormat pixelFormat, int? chunkIndex, int? nextImageDataOffset)
        : base(data, id, name, address, GlobalSize) {
            Collection  = collection;
            ChunkIndex  = chunkIndex;

            widthAddress           = Address;     // 1 byte
            heightAddress          = Address + 1; // 1 byte
            imageDataOffsetAddress = Address + 2; // 2 bytes

            if (pixelFormat != TexturePixelFormat.Unknown) {
                PixelFormatKnown = true;
                PixelFormat = pixelFormat;
            }
            else {
                PixelFormatKnown = false;
                PixelFormat = nextImageDataOffset.HasValue ? GuessPixelFormat(nextImageDataOffset.Value) : TexturePixelFormat.Unknown;
            }
            BytesPerPixel = PixelFormat.BytesPerPixel();

            _readyForImageData = true;
            _ = FetchAndCacheTexture(null);
        }

        private TexturePixelFormat GuessPixelFormat(int nextImageDataOffset) {
            if (Width > 0 && Height > 0) {
                var imageDataSize = nextImageDataOffset - ImageDataOffset;
                var bytesPerPixel =  imageDataSize / (double) Width /  Height;
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
        public static int GlobalSize => 0x04;

        private readonly bool _readyForImageData = false;

        private bool FetchAndCacheTexture(Dictionary<TagKey, TagValue> tags) {
            if (!_readyForImageData)
                return false;

            try {
                Texture = PixelFormat == TexturePixelFormat.ABGR1555
                    ? new TextureABGR1555(ID, 0, 0, RawImageData16Bit, tags: tags)
                    : (ITexture) new TextureIndexed(ID, 0, 0, RawImageData8Bit, tags: tags);
                return true;
            }
            catch {
                return false;
            }
        }

        [TableViewModelColumn(displayOrder: -2.66f, displayName: "Collection", minWidth: 110)]
        public TextureCollectionType Collection { get; }

        [TableViewModelColumn(displayOrder: -2.33f, displayName: "Chunk #")]
        public int? ChunkIndex { get; }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0)]
        public int Width {
            get => Data.GetByte(widthAddress);
            set {
                Data.SetByte(widthAddress, (byte) value);
                FetchAndCacheTexture(Texture?.Tags);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1)]
        public int Height {
            get => Data.GetByte(heightAddress);
            set {
                Data.SetByte(heightAddress, (byte) value);
                FetchAndCacheTexture(Texture?.Tags);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffset {
            get => Data.GetWord(imageDataOffsetAddress);
            set {
                Data.SetWord(imageDataOffsetAddress, value);
                FetchAndCacheTexture(Texture?.Tags);
            }
        }

        [TableViewModelColumn(displayName: "Pixel Format Known", displayOrder: 2.5f)]
        public bool PixelFormatKnown { get; }

        [TableViewModelColumn(displayName: "Pixel Format", displayOrder: 3)]
        public TexturePixelFormat PixelFormat { get; }

        [TableViewModelColumn(displayName: "Internal Hash", displayOrder: 4, minWidth: 225)]
        public string Hash => Texture?.Hash ?? "";

        [TableViewModelColumn(displayName: "Tags", displayOrder: 5, minWidth: 200)]
        public string Tags => (Texture.Tags == null) ? "" : string.Join(", ", Texture.Tags.Select(x => x.Key + "|" + x.Value));

        public bool TextureIsLoaded => Texture != null;

        public int BytesPerPixel { get; }

        public byte[,] RawImageData8Bit {
            get {
                if (BytesPerPixel != 1)
                    throw new InvalidOperationException();

                var data = new byte[Width, Height];
                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = (byte) Data.GetByte(off++);
                        data[x, y] = texPixel;
                    }
                }
                return data;
            }
            set {
                if (BytesPerPixel != 1)
                    throw new InvalidOperationException();
                if (value.GetLength(0) != Width || value.GetLength(1) != Height)
                    throw new ArgumentException("Incoming data dimensions must match specified width/height");

                var off = 0;
                var newData = new SF3.ByteData.ByteData(new ByteArray(Width * Height));
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        newData.SetByte(off++, value[x, y]);
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopy());

                FetchAndCacheTexture(Texture?.Tags);
            }
        }

        public ushort[,] RawImageData16Bit {
            get {
                if (BytesPerPixel != 2)
                    throw new InvalidOperationException();

                var data = new ushort[Width, Height];
                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = (ushort) Data.GetWord(off);
                        off += 2;
                        data[x, y] = texPixel;
                    }
                }
                return data;
            }
            set {
                if (BytesPerPixel != 2)
                    throw new InvalidOperationException();
                if (value.GetLength(0) != Width || value.GetLength(1) != Height)
                    throw new ArgumentException("Incoming data dimensions must match specified width/height");

                var off = 0;
                var newData = new SF3.ByteData.ByteData(new ByteArray(Width * Height * 2));
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        newData.SetWord(off, value[x, y]);
                        off += 2;
                    }
                }
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopy());

                FetchAndCacheTexture(Texture?.Tags);
            }
        }

        public ITexture Texture { get; private set; }
    }
}
