using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class TextureIDStruct : Struct {
        private readonly int _textureIdAddr;

        public TextureIDStruct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
            _textureIdAddr = Address; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public ushort TextureID {
            get => Data.GetUInt16(_textureIdAddr);
            set => Data.SetUInt16(_textureIdAddr, value);
        }
    }
}
