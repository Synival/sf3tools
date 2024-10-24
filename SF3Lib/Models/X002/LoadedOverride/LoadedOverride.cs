using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models.X002.LoadedOverride {
    public class LoadedOverride {
        private IX002_FileEditor _fileEditor;

        private int mapID;
        private int synMusic;
        private int medMusic;
        private int julMusic;
        private int extraMusic;
        private int synMpd;
        private int medMpd;
        private int julMpd;
        private int extraMpd;
        private int synChr;
        private int medChr;
        private int julChr;
        private int extraChr;

        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public LoadedOverride(IX002_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x0000527a; //scn1
                if (checkVersion2 == 0x10) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x000058be; //scn2
                if (checkVersion2 == 0x2C) {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3) {
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
            synMpd = start + 0x06; //4 bytes mpd synbios?
            medMpd = start + 0x0a; //4 bytes mpd medion
            julMpd = start + 0x0e; //4 bytes mpd julian?
            extraMpd = start + 0x12; //4 bytes mpd extra?
            synChr = start + 0x16; //4 bytes chr synbios?
            medChr = start + 0x1a; //4 bytes chr medion
            julChr = start + 0x1e; //4 bytes chr julian?
            extraChr = start + 0x22; //4 bytes chr extra?
            address = offset + (id * 0x28);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int LoadedOverrideID => index;
        public string LoadedOverrideName => name;

        public int MOMapID {
            get => _fileEditor.GetWord(mapID);
            set => _fileEditor.SetWord(mapID, value);
        }

        public int SynMusic {
            get => _fileEditor.GetByte(synMusic);
            set => _fileEditor.SetByte(synMusic, (byte) value);
        }

        public int MedMusic {
            get => _fileEditor.GetByte(medMusic);
            set => _fileEditor.SetByte(medMusic, (byte) value);
        }

        public int JulMusic {
            get => _fileEditor.GetByte(julMusic);
            set => _fileEditor.SetByte(julMusic, (byte) value);
        }

        public int ExtraMusic {
            get => _fileEditor.GetByte(extraMusic);
            set => _fileEditor.SetByte(extraMusic, (byte) value);
        }

        public FileIndexValue SynMpd {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(synMpd));
            set => _fileEditor.SetDouble(synMpd, value.Value);
        }

        public FileIndexValue MedMpd {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(medMpd));
            set => _fileEditor.SetDouble(medMpd, value.Value);
        }

        public FileIndexValue JulMpd {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(julMpd));
            set => _fileEditor.SetDouble(julMpd, value.Value);
        }

        public FileIndexValue ExtraMpd {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(extraMpd));
            set => _fileEditor.SetDouble(extraMpd, value.Value);
        }

        public FileIndexValue SynChr {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(synChr));
            set => _fileEditor.SetDouble(synChr, value.Value);
        }

        public FileIndexValue MedChr {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(medChr));
            set => _fileEditor.SetDouble(medChr, value.Value);
        }

        public FileIndexValue JulChr {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(julChr));
            set => _fileEditor.SetDouble(julChr, value.Value);
        }

        public FileIndexValue ExtraChr {
            get => new FileIndexValue(Scenario, _fileEditor.GetDouble(extraChr));
            set => _fileEditor.SetDouble(extraChr, value.Value);
        }

        public int LoadedOverrideAddress => (address);
    }
}
