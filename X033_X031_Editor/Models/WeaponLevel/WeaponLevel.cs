using SF3.Editor;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.WeaponLevel
{
    public class WeaponLevel
    {
        //starting stat table
        private int level1;
        private int level2;
        private int level3;
        private int level4;

        private int address;
        private int offset;
        private int checkType;
        private int checkVersion2;

        private int index;
        private string name;

        public WeaponLevel(int id, string text)
        {
            checkType = FileEditor.getByte(0x00000009); //if it's 0x07 we're in a x033.bin
            checkVersion2 = FileEditor.getByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2

            if (Globals.scenario == 1)
            {
                if (checkType == 0x07)
                {
                    offset = 0x00000d94; //scn1. x033
                }
                else
                {
                    offset = 0x00000d64; //x031
                }

            }
            else if (Globals.scenario == 2)
            {
                if (checkType == 0x07)
                {
                    if (checkVersion2 == 0x8c)
                    {
                        offset = 0x00000ed0; //scn2 ver 1.003
                    }
                    else
                    {
                        offset = 0x00000ef8; //scn2
                    }

                }
                else
                {
                    if (checkVersion2 == 0x4c)
                    {
                        offset = 0x00000e94; //scn2 ver 1.003
                    }
                    else
                    {
                        offset = 0x00000ea4;
                    }

                }
            }
            else if (Globals.scenario == 3)
            {
                if (checkType == 0x07)
                {
                    offset = 0x00001020; //scn3
                }
                else
                {
                    offset = 0x00000fe4;
                }
            }
            else if (Globals.scenario == 4)
            {
                if (checkType == 0x07)
                {
                    offset = 0x000011f4; //pd
                }
                else
                {
                    offset = 0x000011ac;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x11);

            level1 = start + 0x02;
            level2 = start + 0x06;
            level3 = start + 0x0a;
            level4 = start + 0x0e;

            address = offset + (id * 0x11);
            //address = 0x0354c + (id * 0x18);

        }

        public int WeaponLevelID => index;
        public string WeaponLevelName => name;

        public int WLevel1
        {
            get => FileEditor.getWord(level1);
            set => FileEditor.setWord(level1, value);
        }
        public int WLevel2
        {
            get => FileEditor.getWord(level2);
            set => FileEditor.setWord(level2, value);
        }
        public int WLevel3
        {
            get => FileEditor.getWord(level3);
            set => FileEditor.setWord(level3, value);
        }
        public int WLevel4
        {
            get => FileEditor.getWord(level4);
            set => FileEditor.setWord(level4, value);
        }

        public int WeaponLevelAddress => (address);
    }
}
