using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class WeaponSpellRank : Model {
        private readonly int rankNone;
        private readonly int rankC;
        private readonly int rankB;
        private readonly int rankA;
        private readonly int rankS;
        private readonly int offset;
        private readonly int checkVersion2;

        public WeaponSpellRank(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x05) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000070F0; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00006FC8; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00006D04; //scn3
            }
            else {
                offset = 0x00006BE0; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd
            //int start = 0x354c + (id * 24);

            var start = offset + (id * 5);
            rankNone = start; //1 bytes
            rankC = start + 1; //1 byte
            rankB = start + 2; //1 byte
            rankA = start + 3; //1 byte
            rankS = start + 4;
            Address = offset + (id * 0x5);
            //address = 0x0354c + (id * 0x18);
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
