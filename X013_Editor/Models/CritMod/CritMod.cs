using static SF3.X013_Editor.Forms.frmMain;



namespace SF3.X013_Editor.Models.CritMod
{
    public class CritMod
    {
        private int advantage;
        private int disadvantage;
        private int address;
        private int offset;

        private int index;
        private string name;

        public CritMod(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00002e74; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00003050; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00002d58; //scn3
            }
            else
                offset = 0x00002d78; //pd



            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x0b);
            advantage = start + 0x01; //1 bytes
            disadvantage = start + 0x11; //1 byte
            address = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);

        }

        public int CritModID
        {
            get
            {
                return index;
            }
        }
        public string CritModName
        {
            get
            {
                return name;
            }
        }

        public int Advantage
        {
            get
            {
                return FileEditor.getByte(advantage);
            }
            set
            {
                FileEditor.setByte(advantage, (byte)value);
            }
        }
        public int Disadvantage
        {
            get
            {
                return FileEditor.getByte(disadvantage);
            }
            set
            {
                FileEditor.setByte(disadvantage, (byte)value);
            }
        }


        public int CritModAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
