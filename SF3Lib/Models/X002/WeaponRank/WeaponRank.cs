using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X002.WeaponRank
{
    public class WeaponRank
    {
        private IX002_FileEditor _fileEditor;

        private int skill0;
        private int skill1;
        private int skill2;
        private int skill3;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public WeaponRank(IX002_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x000029f8; //scn1
                if (checkVersion2 == 0x10) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00002d00; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000339c; //scn3
            }
            else
                offset = 0x0000344c; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x04);
            skill0 = start; //1 byte
            skill1 = start + 1; //1 byte
            skill2 = start + 2; //1 byte
            skill3 = start + 3; //1 byte

            address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int WeaponRankID => index;
        public string WeaponRankName => name;

        public int Skill0
        {
            get => _fileEditor.GetByte(skill0);
            set => _fileEditor.SetByte(skill0, (byte)value);
        }
        public int Skill1
        {
            get => _fileEditor.GetByte(skill1);
            set => _fileEditor.SetByte(skill1, (byte)value);
        }
        public int Skill2
        {
            get => _fileEditor.GetByte(skill2);
            set => _fileEditor.SetByte(skill2, (byte)value);
        }
        public int Skill3
        {
            get => _fileEditor.GetByte(skill3);
            set => _fileEditor.SetByte(skill3, (byte)value);
        }

        public int WeaponRankAddress => (address);
    }
}
