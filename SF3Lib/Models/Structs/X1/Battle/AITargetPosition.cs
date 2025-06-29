using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class AITargetPosition : Struct {
        private readonly int _targetXAddr;
        private readonly int _targetZAddr;

        public AITargetPosition(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _targetXAddr = Address + 0x00; // 2 bytes
            _targetZAddr = Address + 0x02; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_targetXAddr), displayOrder: 0)]
        [BulkCopy]
        public int TargetX {
            get => Data.GetWord(_targetXAddr);
            set => Data.SetWord(_targetXAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_targetZAddr), displayOrder: 1)]
        [BulkCopy]
        public int TargetZ {
            get => Data.GetWord(_targetZAddr);
            set => Data.SetWord(_targetZAddr, value);
        }
    }
}
