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
        }

        [BulkCopy]
        public int Width {
            get => Editor.GetByte(widthAddress);
            set => Editor.SetByte(widthAddress, (byte) value);
        }

        [BulkCopy]
        public int Height {
            get => Editor.GetByte(heightAddress);
            set => Editor.SetByte(heightAddress, (byte) value);
        }

        [BulkCopy]
        public int ImageDataOffset {
            get => Editor.GetWord(imageDataOffsetAddress);
            set => Editor.SetWord(imageDataOffsetAddress, value);
        }
    }
}
