using System;
using CommonLib.Attributes;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.TextureChunk {
    public class TextureModel : Struct {
        private readonly int widthAddress;
        private readonly int heightAddress;
        private readonly int imageDataOffsetAddress;

        public TextureModel(IRawData data, int id, string name, int address, int? nextImageDataOffset = null)
        : base(data, id, name, address, GlobalSize) {
            widthAddress           = Address;     // 1 byte
            heightAddress          = Address + 1; // 1 byte
            imageDataOffsetAddress = Address + 2; // 2 bytes

            if (nextImageDataOffset.HasValue && Width > 0 && Height > 0) {
                var imageDataSize = nextImageDataOffset.Value - ImageDataOffset;
                var bytesPerPixel =  imageDataSize / (double) Width /  Height;
                if (bytesPerPixel == 2.00) {
                    BytesPerPixel = 2;
                    AssumedPixelFormat2 = TexturePixelFormat.ABGR1555;
                }
                else if (bytesPerPixel == 1.00) {
                    BytesPerPixel = 1;
                    AssumedPixelFormat2 = TexturePixelFormat.UnknownPalette;
                }
                else {
                    try {
                        throw new ArgumentException("Unhandled bytes per pixel: " + bytesPerPixel.ToString());
                    }
                    catch { }

                    BytesPerPixel = 2;
                    AssumedPixelFormat2 = TexturePixelFormat.ABGR1555;
                }
            }
            else
                AssumedPixelFormat2 = TexturePixelFormat.ABGR1555;

            _readyForImageData = true;
            _ = FetchAndCacheTexture();
        }

        public static int GlobalSize => 0x04;

        private readonly bool _readyForImageData = false;

        private bool FetchAndCacheTexture() {
            if (!_readyForImageData)
                return false;

            try {
                Texture =AssumedPixelFormat2 == TexturePixelFormat.ABGR1555
                    ? new TextureABGR1555(RawImageData16Bit)
                    : (ITexture) new TextureIndexed(RawImageData8Bit);
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
                FetchAndCacheTexture();
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1)]
        public int Height {
            get => Data.GetByte(heightAddress);
            set {
                Data.SetByte(heightAddress, (byte) value);
                FetchAndCacheTexture();
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffset {
            get => Data.GetWord(imageDataOffsetAddress);
            set {
                Data.SetWord(imageDataOffsetAddress, value);
                FetchAndCacheTexture();
            }
        }

        public bool TextureIsLoaded => Texture != null;

        public int BytesPerPixel { get; }

        [TableViewModelColumn(displayName: "(Assumed) Pixel Format", displayOrder: 3)]
        public TexturePixelFormat AssumedPixelFormat2 { get; }

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

                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        Data.SetByte(off++, value[x, y]);

                FetchAndCacheTexture();
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

                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        Data.SetWord(off, value[x, y]);
                        off += 2;
                    }
                }

                FetchAndCacheTexture();
            }
        }

        public ITexture Texture { get; private set; }
    }
}
