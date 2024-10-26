using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tables.X013.ExpLimit {
    public class ExpLimit {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int expCheck;
        private readonly int expReplacement;
        private readonly int offset;
        private readonly int checkVersion2;

        public ExpLimit(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00002173; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x68;
                }
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x0000234f; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x0000218b; //scn3
            }
            else {
                offset = 0x000021ab; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ExpLimitID = id;
            ExpLimitName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 7);
            expCheck = start; //1 byte
            expReplacement = start + 6; //1 byte
            ExpLimitAddress = offset + (id * 0x7);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int ExpLimitID { get; }
        public string ExpLimitName { get; }

        public int ExpCheck {
            get => _fileEditor.GetByte(expCheck);
            set => _fileEditor.SetByte(expCheck, (byte) value);
        }

        public int ExpReplacement {
            get => _fileEditor.GetByte(expReplacement);
            set => _fileEditor.SetByte(expReplacement, (byte) value);
        }

        public int ExpLimitAddress { get; }
    }
}
