using SF3.Editor;
using SF3.Types;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.WeaponSpellRank
{
    public class WeaponSpellRank
    {
        private int rankNone;
        private int rankC;
        private int rankB;
        private int rankA;
        private int rankS;
        private int address;
        private int offset;

        private int index;
        private string name;

        public WeaponSpellRank(int id, string text)
        {
            if (Globals.scenario == ScenarioType.Scenario1)
            {
                offset = 0x000070F0; //scn1
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                offset = 0x00006FC8; //scn2
            }
            else if (Globals.scenario == ScenarioType.Scenario3)
            {
                offset = 0x00006D04; //scn3
            }
            else
                offset = 0x00006BE0; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 5);
            rankNone = start; //1 bytes
            rankC = start + 1; //1 byte
            rankB = start + 2; //1 byte
            rankA = start + 3; //1 byte
            rankS = start + 4;
            address = offset + (id * 0x5);
            //address = 0x0354c + (id * 0x18);
        }

        public int WeaponSpellRankID => index;
        public string WeaponSpellRankName => name;

        public int RankNone
        {
            get => FileEditor.getByte(rankNone);
            set => FileEditor.setByte(rankNone, (byte)value);
        }
        public int RankC
        {
            get => FileEditor.getByte(rankC);
            set => FileEditor.setByte(rankC, (byte)value);
        }
        public int RankB
        {
            get => FileEditor.getByte(rankB);
            set => FileEditor.setByte(rankB, (byte)value);
        }
        public int RankA
        {
            get => FileEditor.getByte(rankA);
            set => FileEditor.setByte(rankA, (byte)value);
        }
        public int RankS
        {
            get => FileEditor.getByte(rankS);
            set => FileEditor.setByte(rankS, (byte)value);
        }

        public int WeaponSpellRankAddress => (address);
    }
}
