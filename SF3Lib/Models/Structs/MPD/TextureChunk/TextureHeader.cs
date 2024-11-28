using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD.TextureChunk {
    public class TextureHeader : Struct {
        private readonly int numTexturesAddress;
        private readonly int textureIdStartAddress;

        public TextureHeader(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x4) {
            numTexturesAddress    = Address;
            textureIdStartAddress = Address + 2;
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "# Textures", displayOrder: 0)]
        public int NumTextures {
            get => Data.GetWord(numTexturesAddress);
            set => Data.SetWord(numTexturesAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Texture ID Start", displayOrder: 1, displayFormat: "X2")]
        public int TextureIdStart {
            get => Data.GetWord(textureIdStartAddress);
            set => Data.SetWord(textureIdStartAddress, value);
        }
    }
}
