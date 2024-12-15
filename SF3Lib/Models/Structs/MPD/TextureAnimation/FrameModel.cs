using System;
using CommonLib.Arrays;
using CommonLib.Attributes;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.TextureAnimation {
    public class FrameModel : Struct {
        private readonly int _compressedTextureOffsetAddress;
        private readonly int _unknownAddress;

        public FrameModel(IByteData data, int id, string name, int address, bool is32Bit, int texId, int width, int height, int texAnimId, int frameNum)
        : base(data, id, name, address, is32Bit ? 0x08 : 0x04) {
            Is32Bit   = is32Bit;
            TextureID = texId;
            Width     = width;
            Height    = height;
            texAnimID = texAnimId;
            FrameNum  = frameNum;

            _bytesPerProperty = is32Bit ? 0x04 : 0x02;

            _compressedTextureOffsetAddress = Address + 0 * _bytesPerProperty;
            _unknownAddress                 = Address + 1 * _bytesPerProperty;
        }

        public void FetchAndCacheTexture(IByteData data, TexturePixelFormat assumedPixelFormat, ITexture referenceTexture) {
            if (assumedPixelFormat == TexturePixelFormat.ABGR1555)
                FetchAndCacheTextureABGR1555(data, referenceTexture);
            else
                FetchAndCacheTextureIndexed(data, assumedPixelFormat, referenceTexture);
        }

        private void FetchAndCacheTextureABGR1555(IByteData data, ITexture referenceTexture) {
            var imageData = new ushort[Width, Height];
            var off = 0;
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var texPixel = (ushort) data.GetWord(off);
                    off += 2;
                    imageData[x, y] = texPixel;
                }
            }

            Texture = new TextureABGR1555(imageData, referenceTexture?.Hash ?? "NOTEX");
        }

        private void FetchAndCacheTextureIndexed(IByteData data, TexturePixelFormat assumedPixelFormat, ITexture referenceTexture) {
            var imageData = new byte[Width, Height];
            var off = 0;
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    imageData[x, y] = (byte) data.GetByte(off++);

            Texture = new TextureIndexed(imageData, assumedPixelFormat, referenceTexture?.Hash ?? "NOTEX");
        }

        public ushort[,] UpdateTextureABGR1555(IByteData data, ushort[,] imageData, ITexture referenceTexture) {
            if (imageData.GetLength(0) != Width || imageData.GetLength(1) != Height)
                throw new ArgumentException("Incoming data dimensions must match specified width/height");

            var newData = new ByteData(new ByteArray(Width * Height * 2));
            var off = 0;
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    newData.SetWord(off, imageData[x, y]);
                    off += 2;
                }
            }
            data.Data.SetDataTo(newData.GetDataCopy());

            Texture = new TextureABGR1555(imageData, referenceTexture?.Hash ?? "NOTEX");
            return imageData;
        }

        // TODO: UpdateTextureIndexed()

        /// <summary>
        /// Used for Scenario 3 and PD, which has 32-bit member variables instead of 16-bit.
        /// </summary>
        public bool Is32Bit { get; }

        public bool TextureIsLoaded => Texture != null;

        public TexturePixelFormat AssumedPixelFormat => TexturePixelFormat.ABGR1555;

        [TableViewModelColumn(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID { get; }

        [TableViewModelColumn(displayName: "Width", displayOrder: 1)]
        public int Width { get; }

        [TableViewModelColumn(displayName: "Height", displayOrder: 2)]
        public int Height { get; }

        [TableViewModelColumn(displayName: "Tex. Anim ID", displayOrder: 3, displayFormat: "X2")]
        public int texAnimID { get; }

        public int FrameNum { get; }

        [TableViewModelColumn(displayName: "Frame #", displayOrder: 4)]
        public string FrameNumStr => FrameNum == 0 ? "" : FrameNum.ToString();

        [BulkCopy]
        [TableViewModelColumn(displayName: "Texture Offset", displayOrder: 5, displayFormat: "X4")]
        public uint CompressedTextureOffset {
            get => Data.GetData(_compressedTextureOffsetAddress, _bytesPerProperty);
            set => Data.SetData(_compressedTextureOffsetAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Duration (30fps)", displayOrder: 6)]
        public uint Duration {
            get => Data.GetData(_unknownAddress, _bytesPerProperty);
            set => Data.SetData(_unknownAddress, value, _bytesPerProperty);
        }

        [TableViewModelColumn(displayName: "Internal Hash", displayOrder: 7, minWidth: 450)]
        public string Hash => Texture?.Hash ?? "";        

        private readonly int _bytesPerProperty;

        public ITexture Texture { get; private set; }
    }
}
