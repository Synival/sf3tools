using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class ModelIDStruct : Struct {
        public ModelIDStruct(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
        }

        [TableViewModelColumn(addressField: nameof(Address), displayOrder: 0, displayName: "ModelID", displayFormat: "X4")]
        public ushort ModelID {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }
    }
}
