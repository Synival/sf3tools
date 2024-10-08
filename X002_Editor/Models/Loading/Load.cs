using SF3.Editor;
using SF3.Types;

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

        public Loading(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            checkVersion2 = FileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x000047A4; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00004bd8; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
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

        public ScenarioType Scenario { get; }
        public int LoadID => index;
        public string LoadName => name;

        public int LocationID
        {
            get => FileEditor.GetWord(locationID);
            set => FileEditor.SetWord(locationID, value);
        }

        public int X1
        {
            get => FileEditor.GetWord(x1);
            set => FileEditor.SetWord(x1, value);
        }

        public int CHP
        {
            get => FileEditor.GetWord(chp);
            set => FileEditor.SetWord(chp, value);
        }

        public int X5
        {
            get => FileEditor.GetWord(x5);
            set => FileEditor.SetWord(x5, value);
        }

        public int Music
        {
            get => FileEditor.GetWord(music);
            set => FileEditor.SetWord(music, value);
        }

        public int MPD
        {
            get => FileEditor.GetWord(mpd);
            set => FileEditor.SetWord(mpd, value);
        }

        public int LoadUnknown
        {
            get => FileEditor.GetWord(unknown);
            set => FileEditor.SetWord(unknown, value);
        }

        public int CHR
        {
            get => FileEditor.GetWord(chr);
            set => FileEditor.SetWord(chr, value);
        }

        public int LoadAddress => (address);
    }
}
