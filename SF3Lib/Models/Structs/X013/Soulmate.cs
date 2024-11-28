using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class Soulmate : Struct {
        private readonly int chance;

        public Soulmate(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            chance = Address; // 2 bytes
        }

        [BulkCopy]
        public int Chance {
            get => Editor.GetByte(chance);
            set => Editor.SetByte(chance, (byte) value);
        }
    }
}
