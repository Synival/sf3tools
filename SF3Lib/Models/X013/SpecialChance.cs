using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class SpecialChance : Model {
        private readonly int twoSpecials2;
        private readonly int threeSpecials3;
        private readonly int threeSpecials2;
        private readonly int fourSpecials4;
        private readonly int fourSpecials3;
        private readonly int fourSpecials2;
        private readonly int offset;
        private readonly int checkVersion2;

        public SpecialChance(IX013_FileEditor editor, int id, string name, int address, bool hasLargeTable)
        : base(editor, id, name, address, hasLargeTable ? 0x4a : 0x3a) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000027ae; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x70;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x000029c6; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x000027a2; //scn3
            }
            else {
                offset = 0x000027c2; //pd
            }

            if (hasLargeTable) {
                Address = offset + (id * 0x4a);
                twoSpecials2   = Address + 0x01; // 1 byte
                threeSpecials3 = Address + 0x15; // 1 byte
                threeSpecials2 = Address + 0x1d; // 1 byte
                fourSpecials4  = Address + 0x31; // 1 byte
                fourSpecials3  = Address + 0x3d; // 1 byte
                fourSpecials2  = Address + 0x49; // 1 byte
            }
            else {
                Address = offset + (id * 0x3a);
                twoSpecials2   = Address + 0x01; // 1 byte
                threeSpecials3 = Address + 0x0f; // 1 byte
                threeSpecials2 = Address + 0x19; // 1 byte
                fourSpecials4  = Address + 0x21; // 1 byte
                fourSpecials3  = Address + 0x2d; // 1 byte
                fourSpecials2  = Address + 0x39; // 1 byte
            }
        }

        [BulkCopy]
        public int TwoSpecials2 {
            get => Editor.GetByte(twoSpecials2);
            set => Editor.SetByte(twoSpecials2, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecials3 {
            get => Editor.GetByte(threeSpecials3);
            set => Editor.SetByte(threeSpecials3, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecials2 {
            get => Editor.GetByte(threeSpecials2);
            set => Editor.SetByte(threeSpecials2, (byte) value);
        }

        [BulkCopy]
        public int FourSpecials4 {
            get => Editor.GetByte(fourSpecials4);
            set => Editor.SetByte(fourSpecials4, (byte) value);
        }

        [BulkCopy]
        public int FourSpecials3 {
            get => Editor.GetByte(fourSpecials3);
            set => Editor.SetByte(fourSpecials3, (byte) value);
        }

        [BulkCopy]
        public int FourSpecials2 {
            get => Editor.GetByte(fourSpecials2);
            set => Editor.SetByte(fourSpecials2, (byte) value);
        }
    }
}
