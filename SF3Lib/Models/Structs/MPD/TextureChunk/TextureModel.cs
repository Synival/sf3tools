using System;
using CommonLib.Arrays;
using CommonLib.Attributes;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.TextureChunk {
    public class TextureModel : Struct {
        private readonly int widthAddress;
        private readonly int heightAddress;
        private readonly int imageDataOffsetAddress;

        public TextureModel(IByteData data, int id, string name, int address, int? nextImageDataOffset = null)
        : base(data, id, name, address, GlobalSize) {
            widthAddress           = Address;     // 1 byte
            heightAddress          = Address + 1; // 1 byte
            imageDataOffsetAddress = Address + 2; // 2 bytes

            if (nextImageDataOffset.HasValue && Width > 0 && Height > 0) {
                var imageDataSize = nextImageDataOffset.Value - ImageDataOffset;
                var bytesPerPixel =  imageDataSize / (double) Width /  Height;
                if (bytesPerPixel == 2.00) {
                    BytesPerPixel = 2;
                    AssumedPixelFormat = TexturePixelFormat.ABGR1555;
                }
                else if (bytesPerPixel == 1.00) {
                    BytesPerPixel = 1;
                    AssumedPixelFormat = TexturePixelFormat.UnknownPalette;
                }
                else {
                    try {
                        throw new ArgumentException("Unhandled bytes per pixel: " + bytesPerPixel.ToString());
                    }
                    catch { }

                    BytesPerPixel = 2;
                    AssumedPixelFormat = TexturePixelFormat.ABGR1555;
                }
            }
            else
                AssumedPixelFormat = TexturePixelFormat.ABGR1555;

            _readyForImageData = true;
            _ = FetchAndCacheTexture("(tags not implemented)");
        }

        public static int GlobalSize => 0x04;

        private readonly bool _readyForImageData = false;

        private bool FetchAndCacheTexture(string tag) {
            if (!_readyForImageData)
                return false;

            try {
                Texture = AssumedPixelFormat == TexturePixelFormat.ABGR1555
                    ? new TextureABGR1555(RawImageData16Bit, tag: tag)
                    : (ITexture) new TextureIndexed(RawImageData8Bit, tag: tag);
                return true;
            }
            catch {
                return false;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0)]
        public int Width {
            get => Data.GetByte(widthAddress);
            set {
                Data.SetByte(widthAddress, (byte) value);
                FetchAndCacheTexture(Texture?.Tag ?? "");
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1)]
        public int Height {
            get => Data.GetByte(heightAddress);
            set {
                Data.SetByte(heightAddress, (byte) value);
                FetchAndCacheTexture(Texture?.Tag ?? "");
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffset {
            get => Data.GetWord(imageDataOffsetAddress);
            set {
                Data.SetWord(imageDataOffsetAddress, value);
                FetchAndCacheTexture(Texture?.Tag ?? "");
            }
        }

        [TableViewModelColumn(displayName: "(Assumed) Pixel Format", displayOrder: 3)]
        public TexturePixelFormat AssumedPixelFormat { get; }

        [TableViewModelColumn(displayName: "Internal Hash", displayOrder: 4, minWidth: 225)]
        public string Hash => Texture?.Hash ?? "";        

        [TableViewModelColumn(displayName: "Tag", displayOrder: 5, minWidth: 200)]
        public string Tag => Texture?.Tag ?? "";        

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
                var newData = new ByteData(new ByteArray(Width * Height));
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        newData.SetByte(off++, value[x, y]);
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopy());

                FetchAndCacheTexture(Texture?.Tag ?? "");
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
                var newData = new ByteData(new ByteArray(Width * Height * 2));
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        newData.SetWord(off, value[x, y]);
                        off += 2;
                    }
                }
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopy());

                FetchAndCacheTexture(Texture?.Tag ?? "");
            }
        }

        public ITexture Texture { get; private set; }
    }
}
