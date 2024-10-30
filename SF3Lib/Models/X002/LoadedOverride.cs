using CommonLib;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models.X002 {
    public class LoadedOverride {
        private readonly IX002_FileEditor _fileEditor;

        private readonly int mapID;
        private readonly int synMusic;
        private readonly int medMusic;
        private readonly int julMusic;
        private readonly int extraMusic;
        private readonly int synMpd;
        private readonly int medMpd;
        private readonly int julMpd;
        private readonly int extraMpd;
        private readonly int synChr;
        private readonly int medChr;
        private readonly int julChr;
        private readonly int extraChr;
        private readonly int offset;
        private readonly int checkVersion2;

        public LoadedOverride(IX002_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x0000527a; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x000058be; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00006266; //scn3
            }
            else {
                offset = 0x00005aa2; //pd. assumed location if it were to exist
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            LoadedOverrideID = id;
            LoadedOverrideName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x28);
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
            LoadedOverrideAddress = offset + (id * 0x28);
            //address = 0x0354c + (id * 0x18);
        }

        private NameAndInfo GetFileIndexName(int value) => ValueNames.GetFileIndexName(Scenario, value);

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int LoadedOverrideID { get; }
        public string LoadedOverrideName { get; }

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

        [NameGetter(nameof(GetFileIndexName))]
        public int SynMpd {
            get => _fileEditor.GetDouble(synMpd);
            set => _fileEditor.SetDouble(synMpd, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int MedMpd {
            get => _fileEditor.GetDouble(medMpd);
            set => _fileEditor.SetDouble(medMpd, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int JulMpd {
            get => _fileEditor.GetDouble(julMpd);
            set => _fileEditor.SetDouble(julMpd, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int ExtraMpd {
            get => _fileEditor.GetDouble(extraMpd);
            set => _fileEditor.SetDouble(extraMpd, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int SynChr {
            get => _fileEditor.GetDouble(synChr);
            set => _fileEditor.SetDouble(synChr, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int MedChr {
            get => _fileEditor.GetDouble(medChr);
            set => _fileEditor.SetDouble(medChr, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int JulChr {
            get => _fileEditor.GetDouble(julChr);
            set => _fileEditor.SetDouble(julChr, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int ExtraChr {
            get => _fileEditor.GetDouble(extraChr);
            set => _fileEditor.SetDouble(extraChr, value);
        }

        public int LoadedOverrideAddress { get; }
    }
}
