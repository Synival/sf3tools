using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013.WeaponSpellRank {
    public class WeaponSpellRank {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int rankNone;
        private readonly int rankC;
        private readonly int rankB;
        private readonly int rankA;
        private readonly int rankS;
        private readonly int offset;
        private readonly int checkVersion2;

        public WeaponSpellRank(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x000070F0; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00006FC8; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00006D04; //scn3
            }
            else {
                offset = 0x00006BE0; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            WeaponSpellRankID = id;
            WeaponSpellRankName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 5);
            rankNone = start; //1 bytes
            rankC = start + 1; //1 byte
            rankB = start + 2; //1 byte
            rankA = start + 3; //1 byte
            rankS = start + 4;
            WeaponSpellRankAddress = offset + (id * 0x5);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int WeaponSpellRankID { get; }
        public string WeaponSpellRankName { get; }

        public int RankNone {
            get => _fileEditor.GetByte(rankNone);
            set => _fileEditor.SetByte(rankNone, (byte) value);
        }

        public int RankC {
            get => _fileEditor.GetByte(rankC);
            set => _fileEditor.SetByte(rankC, (byte) value);
        }

        public int RankB {
            get => _fileEditor.GetByte(rankB);
            set => _fileEditor.SetByte(rankB, (byte) value);
        }

        public int RankA {
            get => _fileEditor.GetByte(rankA);
            set => _fileEditor.SetByte(rankA, (byte) value);
        }

        public int RankS {
            get => _fileEditor.GetByte(rankS);
            set => _fileEditor.SetByte(rankS, (byte) value);
        }

        public int WeaponSpellRankAddress { get; }
    }
}
