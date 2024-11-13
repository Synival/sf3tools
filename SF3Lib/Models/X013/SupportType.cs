using CommonLib.Attributes;
using SF3.StreamEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class SupportType : Model {
        private readonly int supportA;
        private readonly int supportB;

        public SupportType(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x02) {
            supportA = Address;     // 1 byte
            supportB = Address + 1; // 1 byte
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportA {
            get => Editor.GetByte(supportA);
            set => Editor.SetByte(supportA, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportB {
            get => Editor.GetByte(supportB);
            set => Editor.SetByte(supportB, (byte) value);
        }
    }
}
