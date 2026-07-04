using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class MapUpdateFunc : Struct {
        private readonly int _updateSlotAddr;
        private readonly int _functionAddr;

        public MapUpdateFunc(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            // TODO: enum for the update types
            _updateSlotAddr = Address + 0x00; // 4 bytes
            _functionAddr   = Address + 0x04; // 4 bytes
        }

        [NameGetter(NamedValueType.MapUpdateFunc)]
        [TableViewModelColumn(addressField: nameof(_updateSlotAddr), displayOrder: 0, displayFormat: "X2", minWidth: 150)]
        public uint UpdateSlot {
            get => (uint) Data.GetInt32(_updateSlotAddr);
            set => Data.SetInt32(_updateSlotAddr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_functionAddr), displayOrder: 1, isPointer: true)]
        public uint Function {
            get => (uint) Data.GetInt32(_functionAddr);
            set => Data.SetInt32(_functionAddr, (int) value);
        }
    }
}
