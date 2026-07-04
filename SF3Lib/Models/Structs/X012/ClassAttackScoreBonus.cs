using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X012 {
    public class ClassAttackScoreBonus : Struct {
        private readonly int _classIdAddr;
        private readonly int _valueAddr;

        public ClassAttackScoreBonus(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
            _classIdAddr = Address + 0x00; // 1 byte
            _valueAddr   = Address + 0x01; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_classIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.CharacterClass)]
        [BulkCopy]
        public byte ClassID {
            get => (byte) Data.GetUInt8(_classIdAddr);
            set => Data.SetUInt8(_classIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_valueAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte Value {
            get => (byte) Data.GetUInt8(_valueAddr);
            set => Data.SetUInt8(_valueAddr, value);
        }
    }
}
