using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Soulmate : Model {
        private readonly int chance;
        private readonly int offset;
        private readonly int checkVersion2;

        public Soulmate(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00007530; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007484; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000736c; //scn3
            }
            else {
                offset = 0x00007248; //pd
            }

            var start = offset + (id * 1);
            chance = start; //2 bytes
            Address = offset + (id * 0x1);
        }

        [BulkCopy]
        public int Chance {
            get => Editor.GetByte(chance);
            set => Editor.SetByte(chance, (byte) value);
        }
    }
}
