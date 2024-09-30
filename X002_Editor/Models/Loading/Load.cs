using static SF3.X002_Editor.Forms.frmMain;



namespace SF3.X002_Editor.Models.Loading
{
    public class Loading
    {
        private int locationID;
        private int x1;
        private int chp;
        private int x5;
        private int music;
        private int mpd;
        private int unknown;
        private int chr;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public Loading(int id, string text)
        {
            checkVersion2 = FileEditor.getByte(0x0000000B);

            if (Globals.scenario == 1)
            {
                offset = 0x000047A4; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00004bd8; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }


            }
            else if (Globals.scenario == 3)
            {
                offset = 0x000057d0; //scn3
            }
            else
                offset = 0x000058bc; //pd



            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x10);
            locationID = start; //2 bytes
            x1 = start + 0x02; //2 byte
            chp = start + 0x04; //2 byte
            x5 = start + 0x06; //2 byte
            music = start + 0x08; //2 byte
            mpd = start + 0x0a; //2 bytes
            unknown = start + 0x0c; //2 bytes
            chr = start + 0x0e; //2 bytes
            address = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);

        }

        public int LoadID
        {
            get
            {
                return index;
            }
        }
        public string LoadName
        {
            get
            {
                return name;
            }
        }

        public int LocationID
        {
            get
            {
                return FileEditor.getWord(locationID);
            }
            set
            {
                FileEditor.setWord(locationID, value);
            }
        }

        public int X1
        {
            get
            {
                return FileEditor.getWord(x1);
            }
            set
            {
                FileEditor.setWord(x1, value);
            }
        }

        public int CHP
        {
            get
            {
                return FileEditor.getWord(chp);
            }
            set
            {
                FileEditor.setWord(chp, value);
            }
        }

        public int X5
        {
            get
            {
                return FileEditor.getWord(x5);
            }
            set
            {
                FileEditor.setWord(x5, value);
            }
        }

        public int Music
        {
            get
            {
                return FileEditor.getWord(music);
            }
            set
            {
                FileEditor.setWord(music, value);
            }
        }

        public int MPD
        {
            get
            {
                return FileEditor.getWord(mpd);
            }
            set
            {
                FileEditor.setWord(mpd, value);
            }
        }

        public int LoadUnknown
        {
            get
            {
                return FileEditor.getWord(unknown);
            }
            set
            {
                FileEditor.setWord(unknown, value);
            }
        }

        public int CHR
        {
            get
            {
                return FileEditor.getWord(chr);
            }
            set
            {
                FileEditor.setWord(chr, value);
            }
        }

        public int LoadAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
