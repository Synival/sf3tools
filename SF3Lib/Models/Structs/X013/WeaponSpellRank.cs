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

        public WeaponSpellRank(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x05) {
            rankNone = Address;     // 1 byte
            rankC    = Address + 1; // 1 byte
            rankB    = Address + 2; // 1 byte
            rankA    = Address + 3; // 1 byte
            rankS    = Address + 4; // 1 byte
        }

        [BulkCopy]
        public int RankNone {
            get => Data.GetByte(rankNone);
            set => Data.SetByte(rankNone, (byte) value);
        }

        [BulkCopy]
        public int RankC {
            get => Data.GetByte(rankC);
            set => Data.SetByte(rankC, (byte) value);
        }

        [BulkCopy]
        public int RankB {
            get => Data.GetByte(rankB);
            set => Data.SetByte(rankB, (byte) value);
        }

        [BulkCopy]
        public int RankA {
            get => Data.GetByte(rankA);
            set => Data.SetByte(rankA, (byte) value);
        }

        [BulkCopy]
        public int RankS {
            get => Data.GetByte(rankS);
            set => Data.SetByte(rankS, (byte) value);
        }
    }
}
