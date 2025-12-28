using System;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.KAO {
    public class FaceChunk : Struct, ITextureData {
        private readonly int _paletteAddr;
        private readonly int _imageDataAddr;

        public FaceChunk(IByteData data, int id, string name, int address, int actualAddress, CompressedData compressedData)
        : base(data, id, name, address, 0x22) {
            ActualAddress      = actualAddress;
            CompressedData     = compressedData;

            _paletteAddr       = Address + 0x22; // 0x200 bytes
            _imageDataAddr     = Address + 0x222; // width * height bytes

            Header = new FaceHeader(data, 0, nameof(FaceHeader), 0);
            Header.OnDimensionsChanged += (s, e) => {
                _textureData.Width  = Header.Width;
                _textureData.Height = Header.Height;
            };

            _textureData = new TextureData(Data.Data, _imageDataAddr, Header.Width, Header.Height, TexturePixelFormat.Palette1, Palette, false, true, true);
        }

        public FaceHeader Header { get; }

        public void SetImageData8Bit(byte[,] data, Palette palette) => _textureData.SetImageData8Bit(data, palette);
        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB1555(highlightEndcodes);
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB8888(highlightEndcodes);
        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) => _textureData.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize);
        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize) => _textureData.Validate16BitImageData(data, oldStoredSize, newStoredSize);

        [TableViewModelColumn(displayOrder: -1.5f, displayFormat: "X4", displayGroup: "Metadata")]
        public int ActualAddress { get; }

        public CompressedData CompressedData { get; }

        [TableViewModelColumn(displayOrder: -1.4f, displayFormat: "X4", displayGroup: "Metadata")]
        public int CompressedSize => CompressedData.Length;

        [TableViewModelColumn(displayOrder: -1.3f, displayFormat: "X4", isReadOnly: true, displayGroup: "Metadata")]
        public int? MaxCompressedSize { get; set; }

        [TableViewModelColumn(displayOrder: -1.2f, displayFormat: "X4", displayGroup: "Metadata")]
        public int DecompressedSize => Data.Length;

        public int Width => _textureData.Width;
        public int Height => _textureData.Height;
        public int BytesPerPixel => _textureData.BytesPerPixel;
        public TexturePixelFormat PixelFormat => _textureData.PixelFormat;
        public byte[,] ImageData8Bit => _textureData.ImageData8Bit;
        public ushort[,] ImageData16Bit { get => _textureData.ImageData16Bit; set => _textureData.ImageData16Bit = value; }
        public byte[] BitmapDataARGB1555 => _textureData.BitmapDataARGB1555;
        public byte[] BitmapDataARGB8888 => _textureData.BitmapDataARGB8888;
        public string Hash => _textureData.Hash;

        public Palette Palette {
            get {
                var colors = new ushort[0x100];
                var colorAddr = _paletteAddr;
                for (int i = 0; i < 0x100; i++) {
                    colors[i] = (ushort) Data.GetWord(colorAddr);
                    colorAddr += 2;
                }
                return new Palette(colors);
            }
            set {
                var colorAddr = _paletteAddr;
                for (int i = 0; i < 0x100; i++) {
                    Data.SetWord(colorAddr, value[i].ToARGB1555());
                    colorAddr += 2;
                }
                _textureData.Palette = value;
            }
        }

        public bool CanSetImageData8Bit => _textureData.CanSetImageData8Bit;
        public bool CanSetImageData16Bit => _textureData.CanSetImageData16Bit;

        private readonly TextureData _textureData;
    }
}
