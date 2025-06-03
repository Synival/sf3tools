using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SpecialAnimationAssignment : Struct {
        private readonly int _specialIdAddr;
        private readonly int _modelIndexAddr;
        private readonly int _effectIdAddr;

        public SpecialAnimationAssignment(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _specialIdAddr  = Address + 0x00; // 2 bytes
            _modelIndexAddr = Address + 0x02; // 1 byte
            _effectIdAddr   = Address + 0x03; // 1 byte
        }

        [TableViewModelColumn(displayOrder: 0, minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int SpecialId {
            get => Data.GetWord(_specialIdAddr);
            set => Data.SetWord(_specialIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte ModelIndex {
            get => (byte) Data.GetByte(_modelIndexAddr);
            set => Data.SetByte(_modelIndexAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, minWidth: 200, displayFormat: "X2")]
        [NameGetter(NamedValueType.SpecialAnimation)]
        [BulkCopy]
        public int EffectId {
            get => Data.GetByte(_effectIdAddr);
            set => Data.SetByte(_modelIndexAddr, (byte) value);
        }
    }
}
