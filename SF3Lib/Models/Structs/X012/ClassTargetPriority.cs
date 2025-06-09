using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X012 {
    public class ClassTargetPriority : Struct {
        private readonly int _classIdAddr;

        public ClassTargetPriority(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x01) {
            _classIdAddr = Address + 0x00; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_classIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.CharacterClass)]
        [BulkCopy]
        public byte ClassID {
            get => (byte) Data.GetByte(_classIdAddr);
            set => Data.SetByte(_classIdAddr, value);
        }
    }
}
