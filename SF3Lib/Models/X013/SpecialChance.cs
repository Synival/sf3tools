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

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd
            //int start = 0x354c + (id * 24);

            if (editor.Scenario == ScenarioType.Scenario1) {
                var start = offset + (id * 0x4a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x15; //1 byte
                threeSpecials2 = start + 0x1d; //1 byte
                fourSpecials4 = start + 0x31; //1 byte
                fourSpecials3 = start + 0x3d; //1 byte
                fourSpecials2 = start + 0x49; //1 byte

                Address = offset + (id * 0x4a);
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                var start = offset + (id * 0x4a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x15; //1 byte
                threeSpecials2 = start + 0x1d; //1 byte
                fourSpecials4 = start + 0x31; //1 byte
                fourSpecials3 = start + 0x3d; //1 byte
                fourSpecials2 = start + 0x49; //1 byte

                Address = offset + (id * 0x4a);
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                var start = offset + (id * 0x3a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x0f; //1 byte
                threeSpecials2 = start + 0x19; //1 byte
                fourSpecials4 = start + 0x21; //1 byte
                fourSpecials3 = start + 0x2d; //1 byte
                fourSpecials2 = start + 0x39; //1 byte

                Address = offset + (id * 0x3a);
            }
            else {
                var start = offset + (id * 0x3a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x0f; //1 byte
                threeSpecials2 = start + 0x19; //1 byte
                fourSpecials4 = start + 0x21; //1 byte
                fourSpecials3 = start + 0x2d; //1 byte
                fourSpecials2 = start + 0x39; //1 byte

                Address = offset + (id * 0x3a);
            }

            //address = 0x0354c + (id * 0x18);
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
