using SF3.Editor;
using SF3.Types;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.StatBoost
{
    public class StatBoost
    {
        private int stat;
        private int address;
        private int offset;

        private int index;
        private string name;
        private int checkVersion2;

        public StatBoost(int id, string text)
        {
            checkVersion2 = FileEditor.getByte(0x0000000B);

            if (Globals.scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004537; //scn1
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                offset = 0x000048ab; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Globals.scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000537b; //scn3
            }
            else
                offset = 0x0000542b; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x01);
            stat = start; //1 bytes
            address = offset + (id * 0x01);
            //address = 0x0354c + (id * 0x18);
        }

        public int StatID => index;
        public string StatName => name;

        public int Stat
        {
            get => FileEditor.getByte(stat);
            set => FileEditor.setByte(stat, (byte)value);
        }

        public int StatAddress => (address);
    }
}
