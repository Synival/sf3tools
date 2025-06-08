using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X012 {
    public class ClassTargetUnknown : Struct {
        private readonly int _classIdAddr;
        private readonly int _unknownAddr;

        public ClassTargetUnknown(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
            _classIdAddr = Address + 0x00; // 1 byte
            _unknownAddr = Address + 0x01; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_classIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.CharacterClass)]
        [BulkCopy]
        public byte ClassID {
            get => (byte) Data.GetByte(_classIdAddr);
            set => Data.SetByte(_classIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknownAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte Unknown {
            get => (byte) Data.GetByte(_unknownAddr);
            set => Data.SetByte(_unknownAddr, value);
        }
    }
}
