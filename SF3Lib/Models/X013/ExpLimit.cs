using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class ExpLimit : Model {
        private readonly int expCheck;
        private readonly int expReplacement;
        private readonly int offset;
        private readonly int checkVersion2;

        public ExpLimit(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x07) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00002173; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x68;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x0000234f; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000218b; //scn3
            }
            else {
                offset = 0x000021ab; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 7);
            expCheck = start; //1 byte
            expReplacement = start + 6; //1 byte
            ExpLimitAddress = offset + (id * 0x7);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        public int ExpCheck {
            get => Editor.GetByte(expCheck);
            set => Editor.SetByte(expCheck, (byte) value);
        }

        [BulkCopy]
        public int ExpReplacement {
            get => Editor.GetByte(expReplacement);
            set => Editor.SetByte(expReplacement, (byte) value);
        }

        public int ExpLimitAddress { get; }
    }
}
