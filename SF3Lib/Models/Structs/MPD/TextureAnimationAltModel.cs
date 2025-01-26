using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class TextureAnimationAltModel : Struct {
        public TextureAnimationAltModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public ushort TextureID {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }
    }
}
