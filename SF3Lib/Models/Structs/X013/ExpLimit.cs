using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class ExpLimit : Struct {
        private readonly int expCheck;
        private readonly int expReplacement;

        public ExpLimit(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x07) {
            expCheck       = Address;     // 1 byte
            expReplacement = Address + 6; // 1 byte
        }

        [BulkCopy]
        public int ExpCheck {
            get => Editor.GetByte(expCheck);
            set => Editor.SetByte(expCheck, (byte) value);
        }

        [BulkCopy]
        public int ExpReplacement {
            get => Editor.GetByte(expReplacement);
            set => Editor.SetByte(expReplacement, (byte) value);
        }
    }
}