using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X002.AttackResist
{
    public class AttackResist
    {
        private IX002_FileEditor _fileEditor;

        private int attack;
        private int resist;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public AttackResist(IX002_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

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

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int AttackResistID => index;
        public string AttackResistName => name;

        public int Attack
        {
            get => _fileEditor.GetByte(attack);
            set => _fileEditor.SetByte(attack, (byte)value);
        }
        public int Resist
        {
            get => _fileEditor.GetByte(resist);
            set => _fileEditor.SetByte(resist, (byte)value);
        }

        public int AttackResistAddress => (address);
    }
}
