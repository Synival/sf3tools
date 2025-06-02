using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class AttackAnimationIdModel : Struct {
        private readonly int _attackAnimationIdAddr;

        public AttackAnimationIdModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
            _attackAnimationIdAddr = Address + 0x00; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public short AttackAnimationId {
            get => (short) Data.GetWord(_attackAnimationIdAddr);
            set => Data.SetWord(_attackAnimationIdAddr, value);
        }
    }
}
