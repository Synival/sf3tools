using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class StatBoost {
        private readonly IX002_FileEditor _fileEditor;

        private readonly int stat;
        private readonly int offset;
        private readonly int checkVersion2;

        public StatBoost(IX002_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00004537; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x000048ab; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x0000537b; //scn3
            }
            else {
                offset = 0x0000542b; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            StatID = id;
            StatName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x01);
            stat = start; //1 bytes
            StatAddress = offset + (id * 0x01);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int StatID { get; }
        public string StatName { get; }

        public int Stat {
            get => _fileEditor.GetByte(stat);
            set => _fileEditor.SetByte(stat, (byte) value);
        }

        public int StatAddress { get; }
    }
}
