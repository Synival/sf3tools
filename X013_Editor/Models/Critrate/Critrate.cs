using SF3.Editor;
using SF3.Types;

namespace SF3.X013_Editor.Models.Critrate
{
    public class Critrate
    {
        private int noSpecial;
        private int oneSpecial;
        private int twoSpecial;
        private int threeSpecial;
        private int fourSpecial;
        private int fiveSpecial;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Critrate(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x000073f8; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00007304; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x000071dc; //scn3
            }
            else
                offset = 0x000070b8; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 8);
            noSpecial = start; //1 bytes
            oneSpecial = start + 1; //1 byte
            twoSpecial = start + 2; //1 byte
            threeSpecial = start + 3; //1 byte
            fourSpecial = start + 4;
            fiveSpecial = start + 5;
            address = offset + (id * 0x8);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int CritrateID => index;
        public string CritrateName => name;

        public int NoSpecial
        {
            get => FileEditor.GetByte(noSpecial);
            set => FileEditor.SetByte(noSpecial, (byte)value);
        }
        public int OneSpecial
        {
            get => FileEditor.GetByte(oneSpecial);
            set => FileEditor.SetByte(oneSpecial, (byte)value);
        }
        public int TwoSpecial
        {
            get => FileEditor.GetByte(twoSpecial);
            set => FileEditor.SetByte(twoSpecial, (byte)value);
        }
        public int ThreeSpecial
        {
            get => FileEditor.GetByte(threeSpecial);
            set => FileEditor.SetByte(threeSpecial, (byte)value);
        }
        public int FourSpecial
        {
            get => FileEditor.GetByte(fourSpecial);
            set => FileEditor.SetByte(fourSpecial, (byte)value);
        }

        public int FiveSpecial
        {
            get => FileEditor.GetByte(fiveSpecial);
            set => FileEditor.SetByte(fiveSpecial, (byte)value);
        }

        public int CritrateAddress => (address);
    }
}
