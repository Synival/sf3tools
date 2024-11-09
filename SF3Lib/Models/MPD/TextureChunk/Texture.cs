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

        public ushort[,] ImageData { get; private set; }
    }
}
