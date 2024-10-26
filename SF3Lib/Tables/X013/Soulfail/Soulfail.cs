using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tables.X013.Soulfail {
    public class Soulfail {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int expLost;
        private readonly int offset;
        private readonly int checkVersion2;

        public Soulfail(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00005e5f; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x36;
                }
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x0000650f; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00006077; //scn3
            }
            else {
                offset = 0x00005f37; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            SoulfailID = id;
            SoulfailName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 1);
            expLost = start; //1 bytes
            SoulfailAddress = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SoulfailID { get; }
        public string SoulfailName { get; }

        public int ExpLost {
            get => _fileEditor.GetByte(expLost);
            set => _fileEditor.SetByte(expLost, (byte) value);
        }

        public int SoulfailAddress { get; }
    }
}
