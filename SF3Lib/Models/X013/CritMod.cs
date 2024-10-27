using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class CritMod {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int advantage;
        private readonly int disadvantage;
        private readonly int offset;
        private readonly int checkVersion2;

        public CritMod(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00002e74; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x70;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00003050; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00002d58; //scn3
            }
            else {
                offset = 0x00002d78; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            CritModID = id;
            CritModName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0b);
            advantage = start + 0x01; //1 bytes
            disadvantage = start + 0x11; //1 byte
            CritModAddress = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int CritModID { get; }
        public string CritModName { get; }

        public int Advantage {
            get => _fileEditor.GetByte(advantage);
            set => _fileEditor.SetByte(advantage, (byte) value);
        }

        public int Disadvantage {
            get => _fileEditor.GetByte(disadvantage);
            set => _fileEditor.SetByte(disadvantage, (byte) value);
        }

        public int CritModAddress { get; }
    }
}
