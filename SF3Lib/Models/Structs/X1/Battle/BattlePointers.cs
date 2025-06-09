using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattlePointers : Struct {
        private readonly int _pointerAddr;

        public BattlePointers(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _pointerAddr = Address; // 2 bytes 
        }

        [TableViewModelColumn(addressField: nameof(_pointerAddr), displayOrder: 0, displayName: "Pointer", isPointer: true)]
        [BulkCopy]
        public int Pointer {
            get => Data.GetDouble(_pointerAddr);
            set => Data.SetDouble(_pointerAddr, value);
        }
    }
}
