using CommonLib.Attributes;
using SF3.ByteData;

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

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public uint UpdateSlot {
            get => (uint) Data.GetDouble(_updateSlotAddr);
            set => Data.SetDouble(_updateSlotAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 1, isPointer: true)]
        public uint FunctionAddr {
            get => (uint) Data.GetDouble(_functionAddr);
            set => Data.SetDouble(_functionAddr, (int) value);
        }
    }
}
