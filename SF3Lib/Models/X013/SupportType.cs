using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class SupportType : Model {
        private readonly int supportA;
        private readonly int supportB;
        private readonly int offset;
        private readonly int checkVersion2;

        public SupportType(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x02) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00007484; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007390; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00007278; //scn3
            }
            else {
                offset = 0x00007154; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x02);
            supportA = start; //1 byte
            supportB = start + 1;

            Address = offset + (id * 0x02);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportA {
            get => Editor.GetByte(supportA);
            set => Editor.SetByte(supportA, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportB {
            get => Editor.GetByte(supportB);
            set => Editor.SetByte(supportB, (byte) value);
        }
    }
}
