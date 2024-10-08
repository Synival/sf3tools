using SF3.Editor;
using SF3.Types;

namespace SF3.X002_Editor.Models.Presets
{
    public class Preset
    {
        private int spell;
        private int weaponLv0;
        private int weaponLv1;
        private int weaponLv2;
        private int weaponLv3;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public Preset(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            checkVersion2 = FileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004738; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00004b60; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00005734; //scn3
            }
            else
                offset = 0x00005820; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 5);
            spell = start; //2 bytes
            weaponLv0 = start + 1; //1 byte
            weaponLv1 = start + 2; //1 byte
            weaponLv2 = start + 3; //1 byte
            weaponLv3 = start + 4; //1 byte
            address = offset + (id * 0x05);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int PresetID => index;
        public string PresetName => name;

        public int SpellID2
        {
            get => FileEditor.GetByte(spell);
            set => FileEditor.SetByte(spell, (byte)value);
        }
        public int Weapon0
        {
            get => FileEditor.GetByte(weaponLv0);
            set => FileEditor.SetByte(weaponLv0, (byte)value);
        }
        public int Weapon1
        {
            get => FileEditor.GetByte(weaponLv1);
            set => FileEditor.SetByte(weaponLv1, (byte)value);
        }
        public int Weapon2
        {
            get => FileEditor.GetByte(weaponLv2);
            set => FileEditor.SetByte(weaponLv2, (byte)value);
        }
        public int Weapon3
        {
            get => FileEditor.GetByte(weaponLv3);
            set => FileEditor.SetByte(weaponLv3, (byte)value);
        }

        public int PresetAddress => (address);
    }
}
