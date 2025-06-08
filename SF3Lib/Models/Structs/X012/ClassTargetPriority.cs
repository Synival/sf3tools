using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X012 {
    public class ClassTargetPriority : Struct {
        public ClassTargetPriority(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x01) {
        }

        [TableViewModelColumn(addressField: nameof(Address), displayOrder: 0, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.CharacterClass)]
        [BulkCopy]
        public byte ClassID {
            get => (byte) Data.GetByte(Address + 0x00);
            set => Data.SetByte(Address + 0x00, value);
        }
    }
}
