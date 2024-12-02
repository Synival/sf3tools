using System;
using CommonLib.Attributes;
using CommonLib.Utils;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.TextureAnimation {
    public class FrameModel : Struct, ITexture {
        private readonly int _compressedTextureOffsetAddress;
        private readonly int _unknownAddress;

        public FrameModel(IRawData data, int id, string name, int address, bool is32Bit, int texId, int width, int height, int texAnimId, int frameNum)
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

        public ushort[,] FetchAndAssignImageData(IRawData data) {
            var imageData = new ushort[Width, Height];
            var off = 0;
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var texPixel = (ushort) data.GetWord(off);
                    off += 2;
                    imageData[x, y] = texPixel;
                }
            }

            ImageData16Bit = imageData;
            ImageIsLoaded = true;
            return imageData;
        }

        public ushort[,] UpdateImageData(IRawData data, ushort[,] imageData) {
            if (imageData.GetLength(0) != Width || imageData.GetLength(1) != Height)
                throw new ArgumentException("Incoming data dimensions must match specified width/height");

            var off = 0;
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    data.SetWord(off, imageData[x, y]);
                    off += 2;
                }
            }

            ImageData16Bit = imageData;
            ImageIsLoaded = true;
            return imageData;
        }

        /// <summary>
        /// Used for Scenario 3 and PD, which has 32-bit member variables instead of 16-bit.
        /// </summary>
        public bool Is32Bit { get; }

        public bool ImageIsLoaded { get; private set; } = false;

        public int BytesPerPixel => 2;

        public TexturePixelFormat AssumedPixelFormat => TexturePixelFormat.ABGR1555;

        // Not supported. Images are always non-indexed.
        public byte[,] ImageData8Bit {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public ushort[,] _imageData16Bit = null;

        public ushort[,] ImageData16Bit {
            get => _imageData16Bit;
            private set {
                if (_imageData16Bit != value) {
                    _imageData16Bit = value;
                    BitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToABGR1555BitmapData(value);
                }
            }
        }

        // Not supported. Images are always non-indexed.
        public byte[] BitmapDataIndexed => throw new NotSupportedException();

        public byte[] BitmapDataARGB1555 { get; private set; } = null;

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

        private readonly int _bytesPerProperty;
    }
}
