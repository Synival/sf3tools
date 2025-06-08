using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SpecialStatusEffect : Struct {
        private readonly int _specialAddr;

        public SpecialStatusEffect(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            _specialAddr  = Address; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_specialAddr), displayOrder: 0, minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special {
            get => Data.GetByte(_specialAddr);
            set => Data.SetByte(_specialAddr, (byte) value);
        }
    }
}
