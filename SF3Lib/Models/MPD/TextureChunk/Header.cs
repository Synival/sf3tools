using CommonLib.Attributes;
using SF3.StreamEditors;

namespace SF3.Models.MPD.TextureChunk {
    public class Header : Model {
        private readonly int numTexturesAddress;
        private readonly int textureIdStartAddress;

        public Header(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x4) {
            numTexturesAddress    = Address;
            textureIdStartAddress = Address + 2;
        }

        [BulkCopy]
        public int NumTextures {
            get => Editor.GetWord(numTexturesAddress);
            set => Editor.SetWord(numTexturesAddress, value);
        }

        [BulkCopy]
        public int TextureIdStart {
            get => Editor.GetWord(textureIdStartAddress);
            set => Editor.SetWord(textureIdStartAddress, value);
        }
    }
}
