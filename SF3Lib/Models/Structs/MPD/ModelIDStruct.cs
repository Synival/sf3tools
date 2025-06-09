using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class ModelIDStruct : Struct {
        private readonly int _modelIdAddr;

        public ModelIDStruct(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
            _modelIdAddr = Address + 0x00; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_modelIdAddr), displayOrder: 0, displayName: "ModelID", displayFormat: "X4")]
        public ushort ModelID {
            get => (ushort) Data.GetWord(_modelIdAddr);
            set => Data.SetWord(_modelIdAddr, value);
        }
    }
}
