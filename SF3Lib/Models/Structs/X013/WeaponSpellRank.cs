using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class WeaponSpellRank : Struct {
        private readonly int rankNone;
        private readonly int rankC;
        private readonly int rankB;
        private readonly int rankA;
        private readonly int rankS;

        public WeaponSpellRank(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x05) {
            rankNone = Address;     // 1 byte
            rankC    = Address + 1; // 1 byte
            rankB    = Address + 2; // 1 byte
            rankA    = Address + 3; // 1 byte
            rankS    = Address + 4; // 1 byte
        }

        [BulkCopy]
        public int RankNone {
            get => Editor.GetByte(rankNone);
            set => Editor.SetByte(rankNone, (byte) value);
        }

        [BulkCopy]
        public int RankC {
            get => Editor.GetByte(rankC);
            set => Editor.SetByte(rankC, (byte) value);
        }

        [BulkCopy]
        public int RankB {
            get => Editor.GetByte(rankB);
            set => Editor.SetByte(rankB, (byte) value);
        }

        [BulkCopy]
        public int RankA {
            get => Editor.GetByte(rankA);
            set => Editor.SetByte(rankA, (byte) value);
        }

        [BulkCopy]
        public int RankS {
            get => Editor.GetByte(rankS);
            set => Editor.SetByte(rankS, (byte) value);
        }
    }
}
