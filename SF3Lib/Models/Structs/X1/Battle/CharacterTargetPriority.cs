using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class CharacterTargetPriority : Struct {
        public CharacterTargetPriority(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x01) {
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public byte CharacterID {
            get => (byte) Data.GetByte(Address + 0x00);
            set => Data.SetByte(Address + 0x00, value);
        }
    }
}
