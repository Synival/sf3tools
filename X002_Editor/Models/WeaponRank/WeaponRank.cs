using SF3.Editor;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.WeaponRank
{
    public class WeaponRank
    {
        private int skill0;
        private int skill1;
        private int skill2;
        private int skill3;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public WeaponRank(int id, string text)
        {
            checkVersion2 = FileEditor.getByte(0x0000000B);

            if (Globals.scenario == 1)
            {
                offset = 0x000029f8; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00002d00; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Globals.scenario == 3)
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

        public int WeaponRankID => index;
        public string WeaponRankName => name;

        public int Skill0
        {
            get => FileEditor.getByte(skill0);
            set => FileEditor.setByte(skill0, (byte)value);
        }
        public int Skill1
        {
            get => FileEditor.getByte(skill1);
            set => FileEditor.setByte(skill1, (byte)value);
        }
        public int Skill2
        {
            get => FileEditor.getByte(skill2);
            set => FileEditor.setByte(skill2, (byte)value);
        }
        public int Skill3
        {
            get => FileEditor.getByte(skill3);
            set => FileEditor.setByte(skill3, (byte)value);
        }

        public int WeaponRankAddress => (address);
    }
}
