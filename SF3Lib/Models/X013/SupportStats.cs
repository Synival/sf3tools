using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class SupportStats {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int sLvlStat1;
        private readonly int sLvlStat2;
        private readonly int sLvlStat3;
        private readonly int sLvlStat4;
        private readonly int offset;
        private readonly int checkVersion2;

        public SupportStats(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x000074b5; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00007409; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x000072f1; //scn3
            }
            else {
                offset = 0x000071cd; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            StatID = id;
            StatName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 4);
            sLvlStat1 = start; //1 bytes
            sLvlStat2 = start + 1; //1 byte
            sLvlStat3 = start + 2; //1 byte
            sLvlStat4 = start + 3; //1 byte
            StatAddress = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int StatID { get; }

        [BulkCopyRowName]
        public string StatName { get; }

        [BulkCopy]
        public int SLvlStat1 {
            get => _fileEditor.GetByte(sLvlStat1);
            set => _fileEditor.SetByte(sLvlStat1, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat2 {
            get => _fileEditor.GetByte(sLvlStat2);
            set => _fileEditor.SetByte(sLvlStat2, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat3 {
            get => _fileEditor.GetByte(sLvlStat3);
            set => _fileEditor.SetByte(sLvlStat3, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat4 {
            get => _fileEditor.GetByte(sLvlStat4);
            set => _fileEditor.SetByte(sLvlStat4, (byte) value);
        }

        public int StatAddress { get; }
    }
}
