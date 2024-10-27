using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models.X013 {
    public class SupportType {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int supportA;
        private readonly int supportB;
        private readonly int offset;
        private readonly int checkVersion2;

        public SupportType(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00007484; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00007390; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00007278; //scn3
            }
            else {
                offset = 0x00007154; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            SpellID = id;
            SpellName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x02);
            supportA = start; //1 byte
            supportB = start + 1;

            SpellAddress = offset + (id * 0x02);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SpellID { get; }
        public string SpellName { get; }

        public FriendshipBonusTypeValue SupportA {
            get => new FriendshipBonusTypeValue(_fileEditor.GetByte(supportA));
            set => _fileEditor.SetByte(supportA, (byte) value);
        }

        public FriendshipBonusTypeValue SupportB {
            get => new FriendshipBonusTypeValue(_fileEditor.GetByte(supportB));
            set => _fileEditor.SetByte(supportB, (byte) value);
        }

        public int SpellAddress { get; }
    }
}
