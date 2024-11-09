using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.MPD.TextureChunk {
    public class Texture : Model {
        private readonly int widthAddress;
        private readonly int heightAddress;
        private readonly int imageDataOffsetAddress;

        public Texture(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x4) {
            widthAddress           = Address;     // 1 byte
            heightAddress          = Address + 1; // 1 byte
            imageDataOffsetAddress = Address + 2; // 2 bytes

            _readyForImageData = true;
            UpdateImageData();
        }

        private bool _readyForImageData = false;

        private bool UpdateImageData() {
            if (!_readyForImageData)
                return false;

            try {
                var data = new ushort[Width, Height];
                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = (ushort) Editor.GetWord(off);
                        off += 2;
                        data[x, y] = texPixel;
                    }
                }

                ImageData = data;
                return true;
            }
            catch {
                return false;   
            }
        }

        [BulkCopy]
        public int Width {
            get => Editor.GetByte(widthAddress);
            set {
                Editor.SetByte(widthAddress, (byte) value);
                UpdateImageData();
            }
        }

        [BulkCopy]
        public int Height {
            get => Editor.GetByte(heightAddress);
            set {
                Editor.SetByte(heightAddress, (byte) value);
                UpdateImageData();
            }
        }

        [BulkCopy]
        public int ImageDataOffset {
            get => Editor.GetWord(imageDataOffsetAddress);
            set {
                Editor.SetWord(imageDataOffsetAddress, value);
                UpdateImageData();
            }
        }

        private ushort[,] _imageData = null;

        public ushort[,] ImageData {
            get => _imageData;
            set {
                if (_imageData != value) {
                    _imageData = value;

                    var imageDataBytes = new byte[value.GetLength(0) * value.GetLength(1) * 2];
                    int pos = 0;
                    for (int y = 0; y < value.GetLength(1); y++) {
                        for (int x = 0; x < value.GetLength(0); x++) {
                            // Swap red and blue channels.
                            var lowerChannel = value[x, y] & 0x001F;
                            var upperChannel = value[x, y] & 0x7F00;
                            var newBits = (value[x, y] & ~0x7C1F) | (lowerChannel << 10) | (upperChannel >> 10);
                            imageDataBytes[pos++] = (byte) ((newBits & 0x00FF));
                            imageDataBytes[pos++] = (byte) ((newBits & 0xFF00) >> 8);
                        }
                    }

                    BitmapDataARGB1555 = imageDataBytes;
                }
            }
        }

        public byte[] BitmapDataARGB1555 { get; private set; }
    }
}
