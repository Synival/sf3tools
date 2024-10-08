using SF3.Editor;
using SF3.Types;

namespace SF3.X002_Editor.Models.AttackResist
{
    public class AttackResist
    {
        private int attack;
        private int resist;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public AttackResist(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            checkVersion2 = FileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000cb5; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000d15; //scn2

                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x40;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000dcd; //scn3
            }
            else
                offset = 0x00000dd9; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0xd3);
            attack = start; //1 byte
            resist = start + 0xd2; //1 byte

            address = offset + (id * 0xd3);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int AttackResistID => index;
        public string AttackResistName => name;

        public int Attack
        {
            get => FileEditor.GetByte(attack);
            set => FileEditor.SetByte(attack, (byte)value);
        }
        public int Resist
        {
            get => FileEditor.GetByte(resist);
            set => FileEditor.SetByte(resist, (byte)value);
        }

        public int AttackResistAddress => (address);
    }
}
