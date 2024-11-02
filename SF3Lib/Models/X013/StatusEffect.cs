using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class StatusEffect : Model {
        private readonly int luck0;
        private readonly int luck1;
        private readonly int luck2;
        private readonly int luck3;
        private readonly int luck4;
        private readonly int luck5;
        private readonly int luck6;
        private readonly int luck7;
        private readonly int luck8;
        private readonly int luck9;

        //private int npcOffset;
        private readonly int offset;
        private readonly int checkVersion2;

        /*public int NPCTableAddress1
        {
            get => FileEditor.GetDouble(npcOffset);
            set => FileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public StatusEffect(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x18) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00007408; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007314; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x000071fc; //scn3
            }
            else {
                offset = 0x000070d8; //pd
            }

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x18);

            luck0 = start;
            luck1 = start + 0x02;
            luck2 = start + 0x04;
            luck3 = start + 0x06;
            luck4 = start + 0x08;
            luck5 = start + 0x0A;
            luck6 = start + 0x0C;
            luck7 = start + 0x0E;
            luck8 = start + 0x10;
            luck9 = start + 0x12;

            //unknown42 = start + 52;
            Address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        public int StatusLuck0 {
            get => Editor.GetByte(luck0);
            set => Editor.SetByte(luck0, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck1 {
            get => Editor.GetByte(luck1);
            set => Editor.SetByte(luck1, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck2 {
            get => Editor.GetByte(luck2);
            set => Editor.SetByte(luck2, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck3 {
            get => Editor.GetByte(luck3);
            set => Editor.SetByte(luck3, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck4 {
            get => Editor.GetByte(luck4);
            set => Editor.SetByte(luck4, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck5 {
            get => Editor.GetByte(luck5);
            set => Editor.SetByte(luck5, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck6 {
            get => Editor.GetByte(luck6);
            set => Editor.SetByte(luck6, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck7 {
            get => Editor.GetByte(luck7);
            set => Editor.SetByte(luck7, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck8 {
            get => Editor.GetByte(luck8);
            set => Editor.SetByte(luck8, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck9 {
            get => Editor.GetByte(luck9);
            set => Editor.SetByte(luck9, (byte) value);
        }
    }
}
