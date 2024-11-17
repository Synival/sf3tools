using CommonLib.Attributes;
using SF3.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD.TextureChunk {
    public class Header : Model {
        private readonly int numTexturesAddress;
        private readonly int textureIdStartAddress;

        public Header(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x4) {
            numTexturesAddress    = Address;
            textureIdStartAddress = Address + 2;
        }

        [BulkCopy]
        [Metadata(displayName: "# Textures", displayOrder: 0)]
        public int NumTextures {
            get => Editor.GetWord(numTexturesAddress);
            set => Editor.SetWord(numTexturesAddress, value);
        }

        [BulkCopy]
        [Metadata(displayName: "Texture ID Start", displayOrder: 1, displayFormat: "X2")]
        public int TextureIdStart {
            get => Editor.GetWord(textureIdStartAddress);
            set => Editor.SetWord(textureIdStartAddress, value);
        }
    }
}
