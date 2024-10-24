using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013.Presets {
    public class FriendshipExp {
        private IX013_FileEditor _fileEditor;

        private int sLvl0;
        private int sLvl1;
        private int sLvl2;
        private int sLvl3;
        private int sLvl4;
        private int address;
        private int offset;

        private int index;
        private string name;
        private int checkVersion2;

        public FriendshipExp(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x0000747c; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00007388; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00007270; //scn3
            }
            else
                offset = 0x0000714c; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 4);
            sLvl0 = start; //1 byte
            sLvl1 = start + 1; //1 byte
            sLvl2 = start + 2; //1 byte
            sLvl3 = start + 3; //1 byte
            sLvl4 = start + 4; //1 byte
            address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int PresetID => index;
        public string PresetName => name;

        public int SLvl0 {
            get => _fileEditor.GetByte(sLvl0);
            set => _fileEditor.SetByte(sLvl0, (byte) value);
        }
        public int SLvl1 {
            get => _fileEditor.GetByte(sLvl1);
            set => _fileEditor.SetByte(sLvl1, (byte) value);
        }
        public int SLvl2 {
            get => _fileEditor.GetByte(sLvl2);
            set => _fileEditor.SetByte(sLvl2, (byte) value);
        }
        public int SLvl3 {
            get => _fileEditor.GetByte(sLvl3);
            set => _fileEditor.SetByte(sLvl3, (byte) value);
        }
        public int SLvl4 {
            get => _fileEditor.GetByte(sLvl4);
            set => _fileEditor.SetByte(sLvl4, (byte) value);
        }

        public int PresetAddress => (address);
    }
}
