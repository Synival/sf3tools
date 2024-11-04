using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class HealExp : Model {
        private readonly int healExp;
        private readonly int offset;
        private readonly int checkVersion2;

        public HealExp(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00004c8b; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x64;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00004ebf; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00004aed; //scn3
            }
            else {
                offset = 0x00004b01; //pd
            }

            var start = offset + (id * 1);
            healExp = start; //1 byte
            Address = offset + (id * 0x1);
        }

        [BulkCopy]
        public int HealBonus {
            get => Editor.GetByte(healExp);
            set => Editor.SetByte(healExp, (byte) value);
        }
    }
}
