using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class AttackAnimationIdModel : Struct {
        private readonly int _effectFileIndexAddr;

        public AttackAnimationIdModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
            _effectFileIndexAddr = Address + 0x00; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [NameGetter(NamedValueType.EffectFileIndex)]
        [BulkCopy]
        public short EffectFileIndex {
            get => (short) Data.GetWord(_effectFileIndexAddr);
            set => Data.SetWord(_effectFileIndexAddr, value);
        }
    }
}
