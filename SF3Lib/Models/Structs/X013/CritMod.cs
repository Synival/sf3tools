using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class CritMod : Struct {
        private readonly int advantage;
        private readonly int disadvantage;

        public CritMod(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x12) {
            advantage    = Address + 0x01; // 1 byte
            disadvantage = Address + 0x11; // 1 byte
        }

        [BulkCopy]
        public int Advantage {
            get => Data.GetByte(advantage);
            set => Data.SetByte(advantage, (byte) value);
        }

        [BulkCopy]
        public int Disadvantage {
            get => Data.GetByte(disadvantage);
            set => Data.SetByte(disadvantage, (byte) value);
        }
    }
}
