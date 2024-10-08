using SF3.Editor;
using SF3.Types;

namespace SF3.X013_Editor.Models.ExpLimit
{
    public class ExpLimit
    {
        private int expCheck;
        private int expReplacement;
        private int address;
        private int offset;

        private int index;
        private string name;

        public ExpLimit(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00002173; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x0000234f; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000218b; //scn3
            }
            else
                offset = 0x000021ab; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 7);
            expCheck = start; //1 byte
            expReplacement = start + 6; //1 byte
            address = offset + (id * 0x7);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int ExpLimitID => index;
        public string ExpLimitName => name;

        public int ExpCheck
        {
            get => FileEditor.GetByte(expCheck);
            set => FileEditor.SetByte(expCheck, (byte)value);
        }
        public int ExpReplacement
        {
            get => FileEditor.GetByte(expReplacement);
            set => FileEditor.SetByte(expReplacement, (byte)value);
        }

        public int ExpLimitAddress => (address);
    }
}
