using SF3.Editor;
using SF3.Types;

namespace SF3.X013_Editor.Models.HealExp
{
    public class HealExp
    {
        private int healExp;
        private int address;
        private int offset;

        private int index;
        private string name;

        public HealExp(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004c8b; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00004ebf; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
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

        public ScenarioType Scenario { get; }
        public int HealExpID => index;
        public string HealExpName => name;

        public int HealBonus
        {
            get => FileEditor.GetByte(healExp);
            set => FileEditor.SetByte(healExp, (byte)value);
        }

        public int HealExpAddress => (address);
    }
}
