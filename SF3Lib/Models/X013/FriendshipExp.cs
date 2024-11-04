using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class FriendshipExp : Model {
        private readonly int sLvl0;
        private readonly int sLvl1;
        private readonly int sLvl2;
        private readonly int sLvl3;
        private readonly int sLvl4;
        private readonly int offset;
        private readonly int checkVersion2;

        public FriendshipExp(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x0000747c; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007388; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00007270; //scn3
            }
            else {
                offset = 0x0000714c; //pd
            }

            var start = offset + (id * 4);
            sLvl0 = start; //1 byte
            sLvl1 = start + 1; //1 byte
            sLvl2 = start + 2; //1 byte
            sLvl3 = start + 3; //1 byte
            sLvl4 = start + 4; //1 byte
            Address = offset + (id * 0x04);
        }

        [BulkCopy]
        public int SLvl0 {
            get => Editor.GetByte(sLvl0);
            set => Editor.SetByte(sLvl0, (byte) value);
        }

        [BulkCopy]
        public int SLvl1 {
            get => Editor.GetByte(sLvl1);
            set => Editor.SetByte(sLvl1, (byte) value);
        }

        [BulkCopy]
        public int SLvl2 {
            get => Editor.GetByte(sLvl2);
            set => Editor.SetByte(sLvl2, (byte) value);
        }

        [BulkCopy]
        public int SLvl3 {
            get => Editor.GetByte(sLvl3);
            set => Editor.SetByte(sLvl3, (byte) value);
        }

        [BulkCopy]
        public int SLvl4 {
            get => Editor.GetByte(sLvl4);
            set => Editor.SetByte(sLvl4, (byte) value);
        }
    }
}
