using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.FileEditors;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Loading {
        private readonly IX002_FileEditor _fileEditor;

        private readonly int locationID;
        private readonly int x1;
        private readonly int chp;
        private readonly int x5;
        private readonly int music;
        private readonly int mpd;
        private readonly int unknown;
        private readonly int chr;
        private readonly int offset;
        private readonly int checkVersion2;

        public Loading(IX002_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x000047A4; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00004bd8; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x000057d0; //scn3
            }
            else {
                offset = 0x000058bc; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            LoadID = id;
            LoadName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x10);
            locationID = start; //2 bytes
            x1 = start + 0x02; //2 byte
            chp = start + 0x04; //2 byte
            x5 = start + 0x06; //2 byte
            music = start + 0x08; //2 byte
            mpd = start + 0x0a; //2 bytes
            unknown = start + 0x0c; //2 bytes
            chr = start + 0x0e; //2 bytes
            LoadAddress = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);
        }

        private NameAndInfo GetFileIndexName(int value) => ValueNames.GetFileIndexName(Scenario, value);

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int LoadID { get; }
        public string LoadName { get; }

        public int LocationID {
            get => _fileEditor.GetWord(locationID);
            set => _fileEditor.SetWord(locationID, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int X1 {
            get => _fileEditor.GetWord(x1);
            set => _fileEditor.SetWord(x1, value);
        }

        public int CHP {
            get => _fileEditor.GetWord(chp);
            set => _fileEditor.SetWord(chp, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int X5 {
            get => _fileEditor.GetWord(x5);
            set => _fileEditor.SetWord(x5, value);
        }

        public int Music {
            get => _fileEditor.GetWord(music);
            set => _fileEditor.SetWord(music, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int MPD {
            get => _fileEditor.GetWord(mpd);
            set => _fileEditor.SetWord(mpd, value);
        }

        public int LoadUnknown {
            get => _fileEditor.GetWord(unknown);
            set => _fileEditor.SetWord(unknown, value);
        }

        [NameGetter(nameof(GetFileIndexName))]
        public int CHR {
            get => _fileEditor.GetWord(chr);
            set => _fileEditor.SetWord(chr, value);
        }

        public int LoadAddress { get; }
    }
}
