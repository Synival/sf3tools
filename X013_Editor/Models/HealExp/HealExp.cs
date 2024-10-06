using SF3.Editor;
using SF3.Types;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.HealExp
{
    public class HealExp
    {
        private int healExp;
        private int address;
        private int offset;

        private int index;
        private string name;

        public HealExp(int id, string text)
        {
            if (Globals.scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004c8b; //scn1
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                offset = 0x00004ebf; //scn2
            }
            else if (Globals.scenario == ScenarioType.Scenario3)
            {
                offset = 0x00004aed; //scn3
            }
            else
                offset = 0x00004b01; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 1);
            healExp = start; //1 byte
            address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        public int HealExpID => index;
        public string HealExpName => name;

        public int HealBonus
        {
            get => FileEditor.getByte(healExp);
            set => FileEditor.setByte(healExp, (byte)value);
        }

        public int HealExpAddress => (address);
    }
}
