using SF3.Types;

namespace SF3.X002_Editor.Models.MusicOverride
{
    public class MusicOverride
    {
        private IX002_FileEditor _fileEditor;

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

        public MusicOverride(IX002_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x0000527a; //scn1
                if (checkVersion2 == 0x10) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x000058be; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
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

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int MusicOverrideID => index;
        public string MusicOverrideName => name;

        public int MOMapID
        {
            get => _fileEditor.GetWord(mapID);
            set => _fileEditor.SetWord(mapID, value);
        }

        public int SynMusic
        {
            get => _fileEditor.GetByte(synMusic);
            set => _fileEditor.SetByte(synMusic, (byte)value);
        }

        public int MedMusic
        {
            get => _fileEditor.GetByte(medMusic);
            set => _fileEditor.SetByte(medMusic, (byte)value);
        }

        public int JulMusic
        {
            get => _fileEditor.GetByte(julMusic);
            set => _fileEditor.SetByte(julMusic, (byte)value);
        }

        public int ExtraMusic
        {
            get => _fileEditor.GetByte(extraMusic);
            set => _fileEditor.SetByte(extraMusic, (byte)value);
        }

        public int MOUnknown1
        {
            get => _fileEditor.GetDouble(unknown1);
            set => _fileEditor.SetDouble(unknown1, value);
        }

        public int MOUnknown2
        {
            get => _fileEditor.GetDouble(unknown2);
            set => _fileEditor.SetDouble(unknown2, value);
        }

        public int MOUnknown3
        {
            get => _fileEditor.GetDouble(unknown3);
            set => _fileEditor.SetDouble(unknown3, value);
        }

        public int MOUnknown4
        {
            get => _fileEditor.GetDouble(unknown4);
            set => _fileEditor.SetDouble(unknown4, value);
        }

        public int SynChr
        {
            get => _fileEditor.GetDouble(synChr);
            set => _fileEditor.SetDouble(synChr, value);
        }

        public int MedChr
        {
            get => _fileEditor.GetDouble(medChr);
            set => _fileEditor.SetDouble(medChr, value);
        }

        public int JulChr
        {
            get => _fileEditor.GetDouble(julChr);
            set => _fileEditor.SetDouble(julChr, value);
        }

        public int ExtraChr
        {
            get => _fileEditor.GetDouble(extraChr);
            set => _fileEditor.SetDouble(extraChr, value);
        }

        public int MusicOverrideAddress => (address);
    }
}
