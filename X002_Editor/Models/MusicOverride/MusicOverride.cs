using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.MusicOverride
{
    public class MusicOverride
    {
        private int mapID;
        private int synMusic;
        private int medMusic;
        private int julMusic;
        private int extraMusic;
        private int unknown1;
        private int unknown2;
        private int unknown3;
        private int unknown4;
        private int synChr;
        private int medChr;
        private int julChr;
        private int extraChr;

        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public MusicOverride(int id, string text)
        {
            checkVersion2 = FileEditor.getByte(0x0000000B);

            if (Globals.scenario == 1)
            {
                offset = 0x0000527a; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x000058be; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }

            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00006266; //scn3
            }
            else
                offset = 0x00005aa2; //pd. assumed location if it were to exist

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x28);
            mapID = start; //2 bytes
            synMusic = start + 0x02; //1 byte
            medMusic = start + 0x03; //1 byte
            julMusic = start + 0x04; //1 byte
            extraMusic = start + 0x05; //1 byte
            unknown1 = start + 0x06; //4 bytes mpd synbios?
            unknown2 = start + 0x0a; //4 bytes mpd medion
            unknown3 = start + 0x0e; //4 bytes mpd julian?
            unknown4 = start + 0x12; //4 bytes mpd extra?
            synChr = start + 0x16; //4 bytes chr synbios?
            medChr = start + 0x1a; //4 bytes chr medion
            julChr = start + 0x1e; //4 bytes chr julian?
            extraChr = start + 0x22; //4 bytes chr extra?
            address = offset + (id * 0x28);
            //address = 0x0354c + (id * 0x18);

        }

        public int MusicOverrideID => index;
        public string MusicOverrideName => name;

        public int MOMapID
        {
            get => FileEditor.getWord(mapID);
            set => FileEditor.setWord(mapID, value);
        }

        public int SynMusic
        {
            get
            {
                return FileEditor.getByte(synMusic);
            }
            set
            {
                FileEditor.setByte(synMusic, (byte)value);

            }
        }

        public int MedMusic
        {
            get => FileEditor.getByte(medMusic);
            set => FileEditor.setByte(medMusic, (byte)value);
        }

        public int JulMusic
        {
            get => FileEditor.getByte(julMusic);
            set => FileEditor.setByte(julMusic, (byte)value);
        }

        public int ExtraMusic
        {
            get => FileEditor.getByte(extraMusic);
            set => FileEditor.setByte(extraMusic, (byte)value);
        }

        public int MOUnknown1
        {
            get => FileEditor.getDouble(unknown1);
            set => FileEditor.setDouble(unknown1, value);
        }

        public int MOUnknown2
        {
            get => FileEditor.getDouble(unknown2);
            set => FileEditor.setDouble(unknown2, value);
        }

        public int MOUnknown3
        {
            get => FileEditor.getDouble(unknown3);
            set => FileEditor.setDouble(unknown3, value);
        }

        public int MOUnknown4
        {
            get => FileEditor.getDouble(unknown4);
            set => FileEditor.setDouble(unknown4, value);
        }

        public int SynChr
        {
            get => FileEditor.getDouble(synChr);
            set => FileEditor.setDouble(synChr, value);
        }

        public int MedChr
        {
            get => FileEditor.getDouble(medChr);
            set => FileEditor.setDouble(medChr, value);
        }

        public int JulChr
        {
            get => FileEditor.getDouble(julChr);
            set => FileEditor.setDouble(julChr, value);
        }

        public int ExtraChr
        {
            get => FileEditor.getDouble(extraChr);
            set => FileEditor.setDouble(extraChr, value);
        }

        public int MusicOverrideAddress => (address);
    }
}
