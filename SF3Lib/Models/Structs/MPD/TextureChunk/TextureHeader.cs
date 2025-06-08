using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.TextureChunk {
    public class TextureHeader : Struct {
        private readonly int _numTexturesAddr;
        private readonly int _textureIdStartAddr;

        public TextureHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x4) {
            _numTexturesAddr    = Address;
            _textureIdStartAddr = Address + 2;
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_numTexturesAddr), displayName: "# Textures", displayOrder: 0)]
        public int NumTextures {
            get => Data.GetWord(_numTexturesAddr);
            set => Data.SetWord(_numTexturesAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdStartAddr), displayName: "Texture ID Start", displayOrder: 1, displayFormat: "X2")]
        public int TextureIdStart {
            get => Data.GetWord(_textureIdStartAddr);
            set => Data.SetWord(_textureIdStartAddr, value);
        }
    }
}
