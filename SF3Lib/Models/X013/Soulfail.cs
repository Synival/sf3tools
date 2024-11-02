using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Soulfail : Model {
        private readonly int expLost;
        private readonly int offset;
        private readonly int checkVersion2;

        public Soulfail(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00005e5f; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x36;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x0000650b; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00006077; //scn3
            }
            else {
                offset = 0x00005f37; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 1);
            expLost = start; //1 bytes
            Address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        public int ExpLost {
            get => Editor.GetByte(expLost);
            set => Editor.SetByte(expLost, (byte) value);
        }
    }
}
