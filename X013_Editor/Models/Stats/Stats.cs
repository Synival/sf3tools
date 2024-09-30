using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Stats
{
    public class Stat
    {
        private int sLvlStat1;
        private int sLvlStat2;
        private int sLvlStat3;
        private int sLvlStat4;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Stat(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x000074b5; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00007409; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x000072f1; //scn3
            }
            else
                offset = 0x000071cd; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 4);
            sLvlStat1 = start; //1 bytes
            sLvlStat2 = start + 1; //1 byte
            sLvlStat3 = start + 2; //1 byte
            sLvlStat4 = start + 3; //1 byte
            address = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);

        }

        public int StatID => index;
        public string StatName => name;

        public int SLvlStat1
        {
            get
            {
                return FileEditor.getByte(sLvlStat1);
            }
            set
            {
                FileEditor.setByte(sLvlStat1, (byte)value);
            }
        }
        public int SLvlStat2
        {
            get
            {
                return FileEditor.getByte(sLvlStat2);
            }
            set
            {
                FileEditor.setByte(sLvlStat2, (byte)value);
            }
        }
        public int SLvlStat3
        {
            get
            {
                return FileEditor.getByte(sLvlStat3);
            }
            set
            {
                FileEditor.setByte(sLvlStat3, (byte)value);
            }
        }
        public int SLvlStat4
        {
            get
            {
                return FileEditor.getByte(sLvlStat4);
            }
            set
            {
                FileEditor.setByte(sLvlStat4, (byte)value);
            }
        }

        public int StatAddress => (address);
    }
}
