using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class TextureIDModel : Struct {
        private readonly int _textureIdAddr;

        public TextureIDModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
            _textureIdAddr = Address; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public ushort TextureID {
            get => (ushort) Data.GetWord(_textureIdAddr);
            set => Data.SetWord(_textureIdAddr, value);
        }
    }
}
