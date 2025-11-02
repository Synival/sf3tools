using System;
using CommonLib.Arrays;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public abstract class TextureModelBase : Struct {
        public TextureModelBase(IByteData data, int id, string name, int address, int width, int height, TexturePixelFormat pixelFormat, Palette palette)
        : base(data, id, name, address, width * height) {
            Width  = width;
            Height = height;
            PixelFormat = pixelFormat;
            BytesPerPixel = PixelFormat.BytesPerPixel();
            Size *= BytesPerPixel;
            Palette = palette;

            _ = FetchAndCacheTexture();
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0)]
        public int Width { get; }

        [TableViewModelColumn(addressField: null, displayOrder: 1)]
        public int Height { get; }

        public abstract int ImageDataOffset { get; }

        protected bool FetchAndCacheTexture() {
            try {
                Texture = PixelFormat == TexturePixelFormat.ABGR1555
                    ? new TextureABGR1555(ID, 0, 0, RawImageData16Bit)
                    : (ITexture) new TextureIndexed(ID, 0, 0, RawImageData8Bit, PixelFormat, Palette, zeroIsTransparent: true);
                return true;
            }
            catch {
                return false;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 3, displayFormat: "X4")]
        public int ImageDataSize => Width * Height * BytesPerPixel;

        [TableViewModelColumn(addressField: null, displayName: "Pixel Format", displayOrder: 4)]
        public TexturePixelFormat PixelFormat { get; }

        public int BytesPerPixel { get; }

        public Palette Palette { get; }

        [TableViewModelColumn(addressField: null, displayName: "Internal Hash", displayOrder: 4, minWidth: 225)]
        public string Hash => Texture?.Hash ?? "";

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
                var newData = new ByteData.ByteData(new ByteArray(Width * Height));
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        newData.SetByte(off++, value[x, y]);
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopy());

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

                var off = 0;
                var newData = new ByteData.ByteData(new ByteArray(Width * Height * 2));
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        newData.SetWord(off, value[x, y]);
                        off += 2;
                    }
                }
                Data.Data.SetDataAtTo(ImageDataOffset, newData.Length, newData.GetDataCopy());

                FetchAndCacheTexture();
            }
        }

        public ITexture Texture { get; private set; }
    }
}
